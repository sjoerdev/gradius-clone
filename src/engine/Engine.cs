using System;
using System.Numerics;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;

using SharpDX.Windows;
using SharpDX.DXGI;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using SharpDX.WIC;
using SharpDX.IO;

using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Spork;

public class Engine
{
    // singleton
    private static Engine instance;
    public static Engine Instance = instance ??= new();

    // gameobjects
    private List<GameObject> gameObjects = [];
    public List<GameObject> subscribingGameObjects = [];
    public List<GameObject> unsubscribingGameObjects = [];

    // audio
    private WaveOut outputDevice;
    private MixingSampleProvider mixer;

    // directx
    public RenderTarget renderTarget;
    private RenderForm renderForm;
    private SwapChain swapChain;

    // window
    public string title = "no title currently";
    public int windowWidth = 800;
    public int windowHeight = 600;
    public Color clearColor = Color.Black;

    // input
    private List<Keys> keysPressed = [];
    private List<Keys> keysDown = [];
    private List<Keys> keysUp = [];
    private List<MouseButtons> mouseButtonsPressed = [];
    private List<MouseButtons> mouseButtonsDown = [];
    private List<MouseButtons> mouseButtonsUp = [];
    private List<Keys> keysDownLastFrame = [];
    private List<Keys> keysUpLastFrame = [];
    private List<MouseButtons> mouseButtonsDownLastFrame = [];
    private List<MouseButtons> mouseButtonsUpLastFrame = [];
    private Vector2 mousePosition = Vector2.Zero;

    // other
    public float deltaTime = 0.0f;
    public float angle = 0.0f;
    public Vector2 scale = Vector2.One;
    private SolidColorBrush currentBrush;
    private Font defaultFont;
    private Stopwatch stopwatch;
    private int vsync = 1;
    private bool canpaint = false;

    private void CreateWindow()
    {
        renderForm = new RenderForm(title)
        {
            ShowIcon = false,
            ClientSize = new Size(windowWidth, windowHeight),
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            MaximizeBox = false,
        };

        InitializeDeviceResources();
        InitializeAudioStuff();
        InitializeInputListening();
        
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
        renderTarget = new RenderTarget(new(), surface, new(new(Format.Unknown, SharpDX.Direct2D1.AlphaMode.Premultiplied)));
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

    private void InitializeInputListening()
    {
        renderForm.KeyDown += (o, e) => keysDownLastFrame.Add(e.KeyCode);
        renderForm.KeyUp += (o, e) => keysUpLastFrame.Add(e.KeyCode);
        renderForm.MouseDown += (o, e) => mouseButtonsDownLastFrame.Add(e.Button);
        renderForm.MouseUp += (o, e) => mouseButtonsUpLastFrame.Add(e.Button);
        renderForm.MouseMove += (o, e) => mousePosition = new(e.X, e.Y);
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
            renderTarget.Clear(new RawColor4(clearColor.R, clearColor.G, clearColor.B, clearColor.A));
            HandleKeyboardInput();
            HandleMouseInput();
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

    private void HandleKeyboardInput()
    {
        keysDown.Clear();
        keysUp.Clear();

        foreach (Keys key in keysDownLastFrame)
        {
            if (keysPressed.Contains(key) == false)
            {
                keysDown.Add(key);
                keysPressed.Add(key);
            }
        }

        foreach (Keys key in keysUpLastFrame)
        {
            if (keysPressed.Contains(key) == true)
            {
                keysUp.Add(key);
                keysPressed.Remove(key);
            }
        }

        keysDownLastFrame.Clear();
        keysUpLastFrame.Clear();
    }

    private void HandleMouseInput()
    {
        mouseButtonsDown.Clear();
        mouseButtonsUp.Clear();

        foreach (MouseButtons button in mouseButtonsDownLastFrame)
        {
            if (mouseButtonsPressed.Contains(button) == false)
            {
                mouseButtonsDown.Add(button);
                mouseButtonsPressed.Add(button);
            }
        }

        foreach (MouseButtons button in mouseButtonsUpLastFrame)
        {
            if (mouseButtonsPressed.Contains(button) == true)
            {
                mouseButtonsUp.Add(button);
                mouseButtonsPressed.Remove(button);
            }
        }

        mouseButtonsDownLastFrame.Clear();
        mouseButtonsUpLastFrame.Clear();
    }

    public bool GetKey(Keys key) => keysPressed.Contains(key);
    public bool GetKeyDown(Keys key) => keysDown.Contains(key);
    public bool GetKeyUp(Keys key) => keysUp.Contains(key);
    public bool GetMouseButton(int buttonID) => mouseButtonsPressed.Contains(TranslateMouseButton(buttonID));
    public bool GetMouseButtonDown(int buttonID) => mouseButtonsDown.Contains(TranslateMouseButton(buttonID));
    public bool GetMouseButtonUp(int buttonID) => mouseButtonsUp.Contains(TranslateMouseButton(buttonID));
    public Vector2 GetMousePosition() => mousePosition;
    private MouseButtons TranslateMouseButton(int buttonID)
    {
        return buttonID switch
        {
            0 => MouseButtons.Left,
            1 => MouseButtons.Right,
            2 => MouseButtons.Middle,
            _ => MouseButtons.None,
        };
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

public class GameObject
{
    public Engine engine => Engine.Instance;
    public bool isActive = true;

    public GameObject() => engine.subscribingGameObjects.Add(this);

    public virtual void Destroy() => engine.unsubscribingGameObjects.Add(this);
    public virtual void GameEnd(){}
    public virtual void GameInitialize(){}
    public virtual void GameStart(){}
    public virtual void Update(){}
    public virtual void Paint(){}

    public void SetActive(bool value)
    {
        if (isActive == value) return;
        if (value == true) engine.subscribingGameObjects.Add(this);
        else engine.unsubscribingGameObjects.Add(this);
        isActive = value;
    }
}

public class Bitmap
{
    public SharpDX.Direct2D1.Bitmap dxbitmap;
    public float Width => dxbitmap.Size.Width;
    public float Height => dxbitmap.Size.Height;

    public Bitmap(string filePath) => LoadFromFile(filePath);

    private void LoadFromFile(string filePath)
    {
        ImagingFactory imagingFactory = new();
        NativeFileStream fileStream = new(filePath, NativeFileMode.Open, NativeFileAccess.Read);
        BitmapDecoder bitmapDecoder = new(imagingFactory, fileStream, DecodeOptions.CacheOnDemand);
        BitmapFrameDecode frame = bitmapDecoder.GetFrame(0);
        FormatConverter converter = new(imagingFactory);
        converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppPRGBA);
        RenderTarget renderTarget = Engine.Instance.renderTarget;
        dxbitmap = Bitmap1.FromWicBitmap(renderTarget, converter);
    }
}

public class Font
{
    public enum Alignment
    {
        Left,
        Right,
        Center
    }

    public SharpDX.DirectWrite.TextFormat format;

    public Font(string name, float size)
    {
        SharpDX.DirectWrite.Factory fontFactory = new();
        format = new SharpDX.DirectWrite.TextFormat(fontFactory, name, size);
        fontFactory.Dispose();
    }

    public void SetHorizontalAlignment(Alignment alignment) => format.TextAlignment = (SharpDX.DirectWrite.TextAlignment)alignment;
    public void SetVerticalAlignment(Alignment alignment) => format.ParagraphAlignment = (SharpDX.DirectWrite.ParagraphAlignment)alignment;
}

public class AudioClip : ISampleProvider
{
    private float[] originalData;
    private float[] data;
    private long currentPosition;

    private float volume = 1.0f;
    private bool looping = false;

    private WaveFormat waveFormat;
    public WaveFormat WaveFormat => waveFormat;

    public AudioClip(string filePath) => LoadAudio(filePath);

    private void LoadAudio(string filePath)
    {
        using AudioFileReader audioFileReader = new(filePath);
        var provider = new SampleToWaveProvider(audioFileReader);
        var outputFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
        var resampler = new MediaFoundationResampler(provider, outputFormat);
        resampler.ResamplerQuality = 60;
        var waveToSampleProvider = new WaveToSampleProvider(resampler);
        waveFormat = waveToSampleProvider.WaveFormat;
        List<float> wholeFile = new((int)(audioFileReader.Length / 4));
        float[] readBuffer = new float[waveFormat.SampleRate * waveFormat.Channels];
        int samplesRead = waveToSampleProvider.Read(readBuffer, 0, readBuffer.Length);
        
        while (samplesRead > 0)
        {
            wholeFile.AddRange(readBuffer.Take(samplesRead));
            samplesRead = waveToSampleProvider.Read(readBuffer, 0, readBuffer.Length);
        }

        originalData = wholeFile.ToArray();
        data = originalData;
    }

    public void SetVolume(float volume)
    {
        for (int i = 0; i < originalData.Length; ++i) data[i] = originalData[i] * volume;
    }

    public void SetLooping(bool value) => looping = value;
    public float GetVolume() => volume;
    public bool IsLooping() => looping;

    public int Read(float[] buffer, int offset, int count)
    {
        long availableSamples = data.Length - currentPosition;

        if (availableSamples <= 0)
        {
            currentPosition = 0;
            availableSamples = 0;
            if (looping == true) availableSamples = data.Length - currentPosition;
        }

        long samplesToCopy = Math.Min(availableSamples, count);
        Array.Copy(data, currentPosition, buffer, offset, samplesToCopy);
        currentPosition += samplesToCopy;
        return (int)samplesToCopy;
    }
}