using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Numerics;

using SharpDX.Windows;
using SharpDX.DXGI;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;

using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Project;

public class GameEngine
{
    // singleton
    private static GameEngine instance;
    public static GameEngine Instance
    {
        get
        {
            instance ??= new();
            return instance;
        }
    }

    // stuff
    private List<GameObject> gameObjects = [];
    public List<GameObject> subscribingGameObjects = [];
    public List<GameObject> unsubscribingGameObjects = [];
    public Input input;

    // audio
    private WaveOut outputDevice;
    private MixingSampleProvider mixer;

    // directx
    public RenderTarget renderTarget;
    private RenderForm renderForm;
    private SwapChain swapChain;

    // window
    public string title = "no title currently";
    public string iconPath = "res/gradius_icon.ico";
    public int windowWidth = 800;
    public int windowHeight = 600;
    public SharpDX.Color clearColor = new(255, 255, 255);

    // other
    public float deltaTime = 0.0f;
    public float angle = 0.0f;
    public Vector2 scale = new(1.0f, 1.0f);
    private SolidColorBrush currentBrush;
    private Font defaultFont;
    private Stopwatch stopwatch;
    private int vsync = 1;
    private bool canpaint = false;

    private void CreateWindow()
    {
        renderForm = new RenderForm(title)
        {
            Icon = Icon.ExtractAssociatedIcon(iconPath),
            ClientSize = new Size(windowWidth, windowHeight),
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            MaximizeBox = false,
        };

        InitializeDeviceResources();
        InitializeAudioStuff();

        input = new(renderForm);
        currentBrush = new(renderTarget, SharpDX.Color.Black);
        defaultFont = new("Arial", 12.0f);
    }

    private void InitializeDeviceResources()
    {
        SwapChainDescription swapChainDesc = new()
        {
            ModeDescription = new(windowWidth, windowHeight, new Rational(60, 1), Format.R8G8B8A8_UNorm),
            SampleDescription = new SampleDescription(1, 0),
            SwapEffect = SwapEffect.Discard,
            Usage = Usage.RenderTargetOutput,
            BufferCount = 1,
            OutputHandle = renderForm.Handle,
            IsWindowed = true
        };

        SharpDX.Direct3D11.Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, [SharpDX.Direct3D.FeatureLevel.Level_10_0], swapChainDesc, out SharpDX.Direct3D11.Device device, out swapChain);
        device.Dispose();
        Texture2D backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
        Surface surface = backBuffer.QueryInterface<Surface>();
        backBuffer.Dispose();
        renderTarget = new RenderTarget(new(), surface, new RenderTargetProperties(new PixelFormat(Format.Unknown, SharpDX.Direct2D1.AlphaMode.Premultiplied)));
        SharpDX.DXGI.Factory DXGIFactory = swapChain.GetParent<SharpDX.DXGI.Factory>();
        DXGIFactory.MakeWindowAssociation(renderForm.Handle, WindowAssociationFlags.IgnoreAltEnter);
        DXGIFactory.Dispose();
        renderForm.KeyDown += (o, e) => { if (e.Alt && e.KeyCode == Keys.Enter) swapChain.IsFullScreen = !swapChain.IsFullScreen; };
    }

    private void InitializeAudioStuff()
    {
        outputDevice = new();
        mixer = new(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
        mixer.ReadFully = true;
        mixer.RemoveAllMixerInputs();
        outputDevice.Init(mixer);
        outputDevice.Play();
    }

    public void Run()
    {
        UpdateSubscriptions();
        foreach (GameObject go in gameObjects) go.GameInitialize();
        CreateWindow();
        foreach (GameObject go in gameObjects) go.GameStart();
        stopwatch = new Stopwatch();
        stopwatch.Start();

        RenderLoop.Run(renderForm, () =>
        {
            renderTarget.BeginDraw();
            renderTarget.Clear(clearColor);
            input.Update();
            foreach (GameObject go in gameObjects) go.Update();
            canpaint = true;
            foreach (GameObject go in gameObjects) go.Paint();
            canpaint = false;
            renderTarget.EndDraw();
            swapChain.Present(vsync, PresentFlags.None);
            UpdateSubscriptions();
            deltaTime = (float)(stopwatch.Elapsed.TotalMilliseconds / 1000.0);
            stopwatch.Restart();
        });
    }

    private void UpdateSubscriptions()
    {
        foreach (GameObject go in subscribingGameObjects) gameObjects.Add(go);
        foreach (GameObject go in unsubscribingGameObjects) gameObjects.Remove(go);
        subscribingGameObjects.Clear();
        unsubscribingGameObjects.Clear();
    }

    public void PlayAudio(AudioClip clip) => mixer.AddMixerInput(ConvertToRightChannelCount(clip));
    public void StopAudio(AudioClip clip) => mixer.RemoveMixerInput(ConvertToRightChannelCount(clip));
    public void SetVolume(float volume) => outputDevice.Volume = volume;

    private ISampleProvider ConvertToRightChannelCount(ISampleProvider input)
    {
        if (input.WaveFormat.Channels == mixer.WaveFormat.Channels) return input;
        if (input.WaveFormat.Channels == 1 && mixer.WaveFormat.Channels == 2) return new MonoToStereoSampleProvider(input);
        return input;
    }

    private void ResetTransformMatrix() => renderTarget.Transform = SharpDX.Matrix3x2.Identity;
    private void SetTransformMatrix(Vector2 position, float angle, Vector2 scale, Vector2 transformCenter)
    {
        var matScale = SharpDX.Matrix3x2.Scaling(scale.X, scale.Y, new SharpDX.Vector2(0.0f, 0.0f));
        var matRotate = SharpDX.Matrix3x2.Rotation(angle, new SharpDX.Vector2(transformCenter.X * this.scale.X, transformCenter.Y * this.scale.Y));
        var matTranslate = SharpDX.Matrix3x2.Translation(position.X, position.Y);
        renderTarget.Transform = matScale * matRotate * matTranslate;
    }

    public void SetColor(Color color) => SetColor(color.R, color.G, color.B, color.A);
    public void SetColor(int r, int g, int b) => SetColor(r, g, b, 255);
    public void SetColor(int r, int g, int b, int a)
    {
        if (!canpaint) return;
        SharpDX.Color color = new(r, g, b, a);
        currentBrush.Dispose();
        currentBrush = new SolidColorBrush(renderTarget, color);
    }

    public Color GetColor()
    {
        var sharpdxColor = currentBrush.Color;
        var color = Color.FromArgb((int)(sharpdxColor.R * 255), (int)(sharpdxColor.G * 255), (int)(sharpdxColor.B * 255), (int)(sharpdxColor.A * 255));
        return color;
    }

    public void DrawLine(Vector2 startPoint, Vector2 endPoint, float strokeWidth) => DrawLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y, strokeWidth);
    public void DrawLine(float startPointX, float startPointY, float endPointX, float endPointY) => DrawLine(startPointX, startPointY, endPointX, endPointY, 1);
    public void DrawLine(Vector2 startPoint, Vector2 endPoint) => DrawLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
    public void DrawLine(float startPointX, float startPointY, float endPointX, float endPointY, float strokeWidth)
    {
        if (!canpaint) return;
        float width = endPointX - startPointX;
        float height = endPointY - startPointY;
        SetTransformMatrix(new Vector2(startPointX, startPointY), angle, scale, new Vector2(width * 0.5f, height * 0.5f));
        RawVector2 p1 = new(0.0f, 0.0f);
        RawVector2 p2 = new(width, height);
        renderTarget.DrawLine(p1, p2, currentBrush, strokeWidth);
        ResetTransformMatrix();
    }

    public void DrawRectangle(Rectangle rect, float strokeWidth) => DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height, strokeWidth);
    public void DrawRectangle(float x, float y, float width, float height) => DrawRectangle(x, y, width, height, 1);
    public void DrawRectangle(Rectangle rect) => DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height);
    public void DrawRectangle(float x, float y, float width, float height, float strokeWidth)
    {
        if (!canpaint) return;
        SetTransformMatrix(new Vector2(x, y), angle, scale, new Vector2(width * 0.5f, height * 0.5f));
        RawRectangleF rect = new(0.0f, 0.0f, width, height);
        renderTarget.DrawRectangle(rect, currentBrush, strokeWidth);
        ResetTransformMatrix();
    }
    
    public void FillRectangle(Rectangle rect) => FillRectangle(rect.X, rect.Y, rect.Width, rect.Height);
    public void FillRectangle(float x, float y, float width, float height)
    {
        if (!canpaint) return;
        SetTransformMatrix(new Vector2(x, y), angle, scale, new Vector2(width * 0.5f, height * 0.5f));
        RawRectangleF rect = new(0.0f, 0.0f, width, height);
        renderTarget.FillRectangle(rect, currentBrush);
        ResetTransformMatrix();
    }
    
    public void DrawRoundedRectangle(Rectangle rect, Vector2 radius, int strokeWidth) => DrawRoundedRectangle(rect.X, rect.Y, rect.Width, rect.Height, radius.X, radius.Y, strokeWidth);
    public void DrawRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY) => DrawRoundedRectangle(x, y, width, height, radiusX, radiusY, 1);
    public void DrawRoundedRectangle(Rectangle rect, Vector2 radius) => DrawRoundedRectangle(rect.X, rect.Y, rect.Width, rect.Height, radius.X, radius.Y);
    public void DrawRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, float strokeWidth)
    {
        if (!canpaint) return;
        SetTransformMatrix(new Vector2(x, y), angle, scale, new Vector2(width * 0.5f, height * 0.5f));
        RawRectangleF rect = new(0.0f, 0.0f, width, height);
        RoundedRectangle roundedRect = new()
        {
            Rect = rect,
            RadiusX = radiusX,
            RadiusY = radiusY
        };
        renderTarget.DrawRoundedRectangle(roundedRect, currentBrush, strokeWidth);
        ResetTransformMatrix();
    }
    
    public void FillRoundedRectangle(Rectangle rect, Vector2 radius) => FillRoundedRectangle(rect.X, rect.Y, rect.Width, rect.Height, radius.X, radius.Y);
    public void FillRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY)
    {
        if (!canpaint) return;
        SetTransformMatrix(new Vector2(x, y), angle, scale, new Vector2(width * 0.5f, height * 0.5f));
        RawRectangleF rect = new(0.0f, 0.0f, width, height);
        RoundedRectangle roundedRect = new()
        {
            Rect = rect,
            RadiusX = radiusX,
            RadiusY = radiusY
        };
        renderTarget.FillRoundedRectangle(ref roundedRect, currentBrush);
        ResetTransformMatrix();
    }

    public void DrawEllipse(Rectangle rect, float strokeWidth) => DrawEllipse(rect.X, rect.Y, rect.Width, rect.Height, strokeWidth);
    public void DrawEllipse(float x, float y, float width, float height) => DrawEllipse(x, y, width, height, 1);
    public void DrawEllipse(Rectangle rect) => DrawEllipse(rect.X, rect.Y, rect.Width, rect.Height);
    public void DrawEllipse(float x, float y, float width, float height, float strokeWidth)
    {
        if (!canpaint) return;
        SetTransformMatrix(new Vector2(x, y), angle, scale, new Vector2(0.0f, 0.0f));
        Ellipse ellipse = new(new RawVector2(0.0f, 0.0f), width, height);
        renderTarget.DrawEllipse(ellipse, currentBrush, strokeWidth);
        ResetTransformMatrix();
    }
    
    public void FillEllipse(Rectangle rect) => FillEllipse(rect.X, rect.Y, rect.Width, rect.Height);
    public void FillEllipse(float x, float y, float width, float height)
    {
        if (!canpaint) return;
        SetTransformMatrix(new Vector2(x, y), angle, scale, new Vector2(0.0f, 0.0f));
        Ellipse ellipse = new(new RawVector2(0.0f, 0.0f), width, height);
        renderTarget.FillEllipse(ellipse, currentBrush);
        ResetTransformMatrix();
    }

    public void DrawBitmap(Bitmap bitmap, int x, int y, Rectangle sourceRect) => DrawBitmap(bitmap, x, y, sourceRect.X, sourceRect.Y, sourceRect.Width, sourceRect.Height);
    public void DrawBitmap(Bitmap bitmap, Vector2 position, Rectangle sourceRect) => DrawBitmap(bitmap, position.X, position.Y, sourceRect.X, sourceRect.Y, sourceRect.Width, sourceRect.Height);
    public void DrawBitmap(Bitmap bitmap, float x, float y) => DrawBitmap(bitmap, x, y, 0, 0, 0, 0);
    public void DrawBitmap(Bitmap bitmap, Vector2 position) => DrawBitmap(bitmap, position.X, position.Y);
    public void DrawBitmap(Bitmap bitmap, float x, float y, float sourceX, float sourceY, float sourceWidth, float sourceHeight)
    {
        if (!canpaint) return;
        SharpDX.Direct2D1.Bitmap D2DBitmap = bitmap.dxbitmap;
        if (sourceWidth == 0)  sourceWidth = D2DBitmap.Size.Width;
        if (sourceHeight == 0) sourceHeight = D2DBitmap.Size.Height;
        SetTransformMatrix(new Vector2(x, y), angle, scale, new Vector2(sourceWidth * 0.5f, sourceHeight * 0.5f));
        RawRectangleF sourceRect = new(sourceX, sourceY, (sourceX + sourceWidth), (sourceY + sourceHeight));
        renderTarget.DrawBitmap(D2DBitmap, currentBrush.Color.A, SharpDX.Direct2D1.BitmapInterpolationMode.NearestNeighbor, sourceRect);
        ResetTransformMatrix();
    }

    public void DrawString(Font font, string text, Rectangle rect) => DrawString(font, text, rect.X, rect.Y, rect.Width, rect.Height);
    public void DrawString(string text, float x, float y, float width, float height) => DrawString(null, text, x, y, width, height);
    public void DrawString(string text, Rectangle rect) => DrawString(text, rect.X, rect.Y, rect.Width, rect.Height);
    public void DrawString(Font font, string text, float x, float y, float width, float height)
    {
        if (!canpaint) return;
        if (font == null) font = defaultFont;
        SetTransformMatrix(new Vector2(x, y), angle, scale, new Vector2(width * 0.5f, height * 0.5f));
        RawRectangleF rect = new(0.0f, 0.0f, width, height);
        renderTarget.DrawText(text, font.format, rect, currentBrush);
        ResetTransformMatrix();
    }
}