using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

//SharpDX
using SharpDX.Windows;
using System.Drawing;
using SharpDX.DXGI;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.Mathematics.Interop;
using SharpDX.WIC;
using SharpDX.IO;

using NAudio.Wave;

using D3D11 = SharpDX.Direct3D11;
using Device = SharpDX.Direct3D11.Device;
using D2DFactory = SharpDX.Direct2D1.Factory;
using DXGIFactory = SharpDX.DXGI.Factory;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using System.Windows.Forms;
using Color = SharpDX.Color;
using NAudio.Wave.SampleProviders;


namespace GameEngine
{
    //A stripped version of the System.Windows.Forms.Keys enum to avoid extra includes for students.
    #region RawDataTypes

    public enum Key
    {
        //Modifiers = -65536,
        //None = 0,
        //LButton = 1,
        //RButton = 2,
        //Cancel = 3,
        //MButton = 4,
        //XButton1 = 5,
        //XButton2 = 6,
        Back = 8,
        Tab = 9,
        //LineFeed = 10,
        Clear = 12,
        Return = 13,
        Enter = 13,
        ShiftKey = 16,
        ControlKey = 17,
        Menu = 18,
        Pause = 19,
        //Capital = 20,
        CapsLock = 20,
        //KanaMode = 21,
        //HanguelMode = 21,
        //HangulMode = 21,
        //JunjaMode = 23,
        //FinalMode = 24,
        //HanjaMode = 25,
        //KanjiMode = 25,
        Escape = 27,
        //IMEConvert = 28,
        //IMENonconvert = 29,
        //IMEAccept = 30,
        //IMEAceept = 30,
        //IMEModeChange = 31,
        Space = 32,
        Prior = 33,
        PageUp = 33,
        Next = 34,
        PageDown = 34,
        End = 35,
        Home = 36,
        Left = 37,
        Up = 38,
        Right = 39,
        Down = 40,
        Select = 41,
        Print = 42,
        Execute = 43,
        Snapshot = 44,
        PrintScreen = 44,
        Insert = 45,
        Delete = 46,
        Help = 47,
        D0 = 48,
        D1 = 49,
        D2 = 50,
        D3 = 51,
        D4 = 52,
        D5 = 53,
        D6 = 54,
        D7 = 55,
        D8 = 56,
        D9 = 57,
        A = 65,
        B = 66,
        C = 67,
        D = 68,
        E = 69,
        F = 70,
        G = 71,
        H = 72,
        I = 73,
        J = 74,
        K = 75,
        L = 76,
        M = 77,
        N = 78,
        O = 79,
        P = 80,
        Q = 81,
        R = 82,
        S = 83,
        T = 84,
        U = 85,
        V = 86,
        W = 87,
        X = 88,
        Y = 89,
        Z = 90,
        LWin = 91,
        RWin = 92,
        //Apps = 93,
        Sleep = 95,
        NumPad0 = 96,
        NumPad1 = 97,
        NumPad2 = 98,
        NumPad3 = 99,
        NumPad4 = 100,
        NumPad5 = 101,
        NumPad6 = 102,
        NumPad7 = 103,
        NumPad8 = 104,
        NumPad9 = 105,
        Multiply = 106,
        Add = 107,
        Separator = 108,
        Subtract = 109,
        Decimal = 110,
        Divide = 111,
        F1 = 112,
        F2 = 113,
        F3 = 114,
        F4 = 115,
        F5 = 116,
        F6 = 117,
        F7 = 118,
        F8 = 119,
        F9 = 120,
        F10 = 121,
        F11 = 122,
        F12 = 123,
        F13 = 124,
        F14 = 125,
        F15 = 126,
        F16 = 127,
        F17 = 128,
        F18 = 129,
        F19 = 130,
        F20 = 131,
        F21 = 132,
        F22 = 133,
        F23 = 134,
        F24 = 135,
        NumLock = 144,
        Scroll = 145,
        LShiftKey = 160,
        RShiftKey = 161,
        LControlKey = 162,
        RControlKey = 163,
        //LMenu = 164,
        //RMenu = 165,
        //BrowserBack = 166,
        //BrowserForward = 167,
        //BrowserRefresh = 168,
        //BrowserStop = 169,
        //BrowserSearch = 170,
        //BrowserFavorites = 171,
        //BrowserHome = 172,
        //VolumeMute = 173,
        //VolumeDown = 174,
        //VolumeUp = 175,
        //MediaNextTrack = 176,
        //MediaPreviousTrack = 177,
        //MediaStop = 178,
        //MediaPlayPause = 179,
        //LaunchMail = 180,
        //SelectMedia = 181,
        //LaunchApplication1 = 182,
        //LaunchApplication2 = 183,
        //OemSemicolon = 186,
        //Oem1 = 186,
        //Oemplus = 187,
        //Oemcomma = 188,
        //OemMinus = 189,
        //OemPeriod = 190,
        //OemQuestion = 191,
        //Oem2 = 191,
        //Oemtilde = 192,
        //Oem3 = 192,
        //OemOpenBrackets = 219,
        //Oem4 = 219,
        //OemPipe = 220,
        //Oem5 = 220,
        //OemCloseBrackets = 221,
        //Oem6 = 221,
        //OemQuotes = 222,
        //Oem7 = 222,
        //Oem8 = 223,
        //OemBackslash = 226,
        //Oem102 = 226,
        //ProcessKey = 229,
        //Packet = 231,
        //Attn = 246,
        //Crsel = 247,
        //Exsel = 248,
        //EraseEof = 249,
        //Play = 250,
        //Zoom = 251,
        //NoName = 252,
        //Pa1 = 253,
        //OemClear = 254,
        //KeyCode = 65535,
        //Shift = 65536,
        //Control = 131072,
        //Alt = 262144
    }

    public struct Color
    {
        public static Color White
        {
            get { return new Color(255, 255, 255); }
        }
        public static Color Black
        {
            get { return new Color(0, 0, 0); }
        }
        public static Color Red
        {
            get { return new Color(255, 0, 0); }
        }
        public static Color Green
        {
            get { return new Color(0, 255, 0); }
        }
        public static Color Blue
        {
            get { return new Color(0, 0, 255); }
        }
        public static Color Yellow
        {
            get { return new Color(255, 255, 0); }
        }
        public static Color Cyan
        {
            get { return new Color(0, 255, 255); }
        }
        public static Color Magenta
        {
            get { return new Color(255, 0, 255); }
        }
        public static Color Gray
        {
            get { return new Color(128, 128, 128); }
        }
        public static Color AppelBlauwZeeGroen
        {
            get { return new Color(0, 167, 141); }
        }

        private int m_R;
        public int R
        {
            get { return m_R; }
            set { m_R = value; }
        }

        private int m_G;
        public int G
        {
            get { return m_G; }
            set { m_G = value; }
        }

        private int m_B;
        public int B
        {
            get { return m_B; }
            set { m_B = value; }
        }

        private int m_A;
        public int A
        {
            get { return m_A; }
            set { m_A = value; }
        }

        public Color(int r, int g, int b)
        {
            m_R = r;
            m_G = g;
            m_B = b;
            m_A = 255;
        }

        public Color(int r, int g, int b, int a)
        {
            m_R = r;
            m_G = g;
            m_B = b;
            m_A = a;
        }
    }

    public struct Vector2
    {
        public static Vector2 zero
        {
            get { return new Vector2(0, 0); }
        }

        private int m_X;
        public int X
        {
            get { return m_X; }
            set { m_X = value; }
        }

        private int m_Y;
        public int Y
        {
            get { return m_Y; }
            set { m_Y = value; }
        }

        public Vector2(int x, int y)
        {
            m_X = x;
            m_Y = y;
        }

        public static Vector2 operator +(Vector2 vec1, Vector2 vec2)
        {
            return new Vector2(vec1.X + vec2.X, vec1.Y + vec2.Y);
        }

        public static Vector2 operator -(Vector2 vec1, Vector2 vec2)
        {
            return new Vector2(vec1.X - vec2.X, vec1.Y - vec2.Y);
        }
    }

    public struct Vector2f
    {
        public static Vector2f zero
        {
            get { return new Vector2f(0.0f, 0.0f); }
        }

        private float m_X;
        public float X
        {
            get { return m_X; }
            set { m_X = value; }
        }

        private float m_Y;
        public float Y
        {
            get { return m_Y; }
            set { m_Y = value; }
        }

        public Vector2f(float x, float y)
        {
            m_X = x;
            m_Y = y;
        }

        public static Vector2f operator +(Vector2f vec1, Vector2f vec2)
        {
            return new Vector2f(vec1.X + vec2.X, vec1.Y + vec2.Y);
        }

        public static Vector2f operator -(Vector2f vec1, Vector2f vec2)
        {
            return new Vector2f(vec1.X - vec2.X, vec1.Y - vec2.Y);
        }
    }

    //Vector4
    public struct Rectanglef
    {
        private float m_X;
        public float X
        {
            get { return m_X; }
            set { m_X = value; }
        }

        private float m_Y;
        public float Y
        {
            get { return m_Y; }
            set { m_Y = value; }
        }

        private float m_Width;
        public float Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        private float m_Height;
        public float Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }

        public Rectanglef(float x, float y, float width, float height)
        {
            m_X = x;
            m_Y = y;
            m_Width = width;
            m_Height = height;
        }
    }

    #endregion

    /// <summary>
    /// Used for most game engine functionality.
    /// </summary>
    public class GameEngine : IDisposable
    {
        //Singleton
        private static GameEngine m_Instance;

        //Subscribed game objects
        private List<GameObject> m_GameObjects;
        private List<GameObject> m_SubscribingGameObjects;   //Objects that want to subscribe this frame
        private List<GameObject> m_UnsubscribingGameObjects; //Objects that want to unsubscribe this frame

        //DirectX
        private RenderForm m_RenderForm;
        private SwapChain m_SwapChain;
        private D2DFactory m_Factory;
        private RenderTarget m_RenderTarget;
        public RenderTarget RenderTarget
        {
            //Used a property on purpose as students don't need to use this. (only the Bitmap class does)
            get { return m_RenderTarget; }
        }

        //Subdevisions
        private InputManager m_InputManager;
        private AudioManager m_AudioManager;

        //Window properties
        private string m_Title = "Why did you remove the SetTitle line in AbstractGame?";
        private string m_IconPath = "../../Assets/icon.ico";
        private int m_Width = 800;
        private int m_Height = 600;
        private SharpDX.Color m_ClearColor = new SharpDX.Color(255, 255, 255);

        private float m_Angle = 0.0f;
        private Vector2f m_Scale = new Vector2f(1.0f, 1.0f);

        private bool m_DisposeAtEndOfFrame = false;

        //Camera properties
        //private Vector2f m_CameraPosition;
        //private float m_CameraAngle;

        //Default properties
        private SolidColorBrush m_CurrentBrush;
        private Font m_DefaultFont; //So students can quickly get text on the screen without messing around with fonts.

        //For deltaTime calculation
        private Stopwatch m_Stopwatch;
        private float m_LastDeltaTime = 0.0f;
        private int m_VSync = 1;

        //To force students to draw only in the paint function!
        private bool m_CanIPaint = false;

        //--------------------------
        // Functions
        //--------------------------

        //Core
        public GameEngine()
        {
            m_GameObjects = new List<GameObject>();
            m_SubscribingGameObjects = new List<GameObject>();
            m_UnsubscribingGameObjects = new List<GameObject>();
        }

        public void Dispose()
        {
            //This is the destructor
            for (int i = 0; i < m_GameObjects.Count; ++i)
                m_GameObjects[i].GameEnd();

            m_GameObjects.Clear();
            m_SubscribingGameObjects.Clear();
            m_UnsubscribingGameObjects.Clear();

            m_RenderForm.Close();
            m_RenderForm.Dispose();

            m_SwapChain.Dispose();
            m_RenderTarget.Dispose();

            m_Factory.Dispose();

            m_DefaultFont.Dispose();

            m_AudioManager.Dispose();
        }

        private void CreateWindow()
        {
            m_RenderForm = new RenderForm(m_Title);
            m_RenderForm.Icon = Icon.ExtractAssociatedIcon(m_IconPath);
            m_RenderForm.ClientSize = new Size(m_Width, m_Height);
            m_RenderForm.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            //Disables rescaling
            m_RenderForm.MaximizeBox = false;

            InitializeDeviceResources();

            //Initialize the external managers
            m_InputManager = new InputManager(m_RenderForm);
            m_AudioManager = new AudioManager(44100, 2);

            //Default the brush & font
            m_CurrentBrush = new SolidColorBrush(m_RenderTarget, SharpDX.Color.Black);
            m_DefaultFont = new Font("Arial", 12.0f);
        }

        private void InitializeDeviceResources()
        {
            //DXGI_MODE_DESC (width, height, refresh rate, color format)
            ModeDescription modeDesc = new ModeDescription(m_Width, m_Height, new Rational(60, 1), Format.R8G8B8A8_UNorm);

            //DXGI_SWAP_CHAIN_DESC
            SwapChainDescription swapChainDesc = new SwapChainDescription()
            {
                ModeDescription = modeDesc,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput,
                BufferCount = 1,
                OutputHandle = m_RenderForm.Handle,
                IsWindowed = true
            };

            //Create our device & swap chain
            Device device;
            Device.CreateWithSwapChain(DriverType.Hardware,
                                       DeviceCreationFlags.BgraSupport,
                                       new SharpDX.Direct3D.FeatureLevel[] { SharpDX.Direct3D.FeatureLevel.Level_10_0 },
                                       swapChainDesc,
                                       out device,
                                       out m_SwapChain);

            //We don't need this ref as we only use 2D
            device.Dispose();

            //Create backbuffer & rendertarget
            Texture2D backBuffer = Texture2D.FromSwapChain<Texture2D>(m_SwapChain, 0);
            Surface surface = backBuffer.QueryInterface<Surface>();
            backBuffer.Dispose();

            m_Factory = new D2DFactory();
            m_RenderTarget = new RenderTarget(m_Factory, surface, new RenderTargetProperties(new SharpDX.Direct2D1.PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));

            // Disable automatic ALT+Enter processing because it doesn't work properly with WinForms
            DXGIFactory DXGIFactory = m_SwapChain.GetParent<DXGIFactory>();
            DXGIFactory.MakeWindowAssociation(m_RenderForm.Handle, WindowAssociationFlags.IgnoreAltEnter);
            DXGIFactory.Dispose();

            //And replace it with a custom bind
            m_RenderForm.KeyDown += (o, e) =>
            {
                if (e.Alt && e.KeyCode == Keys.Enter)
                    m_SwapChain.IsFullScreen = !m_SwapChain.IsFullScreen;
            };
        }

        public void Run()
        {
            //Make sure the root GameObject is in the list
            UpdateGameObjectList();

            if (m_GameObjects.Count == 0)
            {
                LogError("We are trying to run an undefined game!");
                return;
            }

            //Initialize the game
            foreach (GameObject go in m_GameObjects)
                go.GameInitialize();

            //Initialize the engine (window)
            CreateWindow();

            foreach (GameObject go in m_GameObjects)
                go.GameStart();

            //Start the core game / renderloop
            m_Stopwatch = new Stopwatch();
            m_Stopwatch.Start();

            RenderLoop.Run(m_RenderForm, () =>
            {
                m_RenderTarget.BeginDraw();
                m_RenderTarget.Clear(m_ClearColor); //Clearcolor

                //Update
                m_InputManager.Update();

                foreach (GameObject go in m_GameObjects)
                    go.Update();

                //Draw
                m_CanIPaint = true;

                foreach (GameObject go in m_GameObjects)
                    go.Paint();

                m_CanIPaint = false;

                m_RenderTarget.EndDraw();

                m_SwapChain.Present(m_VSync, PresentFlags.None); //1 for VSync

                //Add & remove GameObjects from the list
                UpdateGameObjectList();

                m_LastDeltaTime = (float)(m_Stopwatch.Elapsed.TotalMilliseconds / 1000.0);
                m_Stopwatch.Restart();

                //Quit the game if requested
                if (m_DisposeAtEndOfFrame)
                {
                    Dispose();
                }
            });
        }

        public void Quit()
        {
            m_DisposeAtEndOfFrame = true;
        }

        private bool PaintCheck()
        {
            if (m_CanIPaint == false)
            {
                LogError("You are painting outside of the paint function!");
            }

            return m_CanIPaint;
        }

        private void UpdateGameObjectList()
        {
            foreach (GameObject go in m_SubscribingGameObjects)
                m_GameObjects.Add(go);

            foreach (GameObject go in m_UnsubscribingGameObjects)
                m_GameObjects.Remove(go);

            m_SubscribingGameObjects.Clear();
            m_UnsubscribingGameObjects.Clear();
        }

        public void SubscribeGameObject(GameObject go)
        {
            m_SubscribingGameObjects.Add(go);
        }

        public void UnsubscribeGameObject(GameObject go)
        {
            m_UnsubscribingGameObjects.Add(go);
        }

        //Window mutators & accessors (we are not using C# properties to make everything more clear.)
        public static GameEngine GetInstance()
        {
            if (m_Instance == null) { m_Instance = new GameEngine(); }
                return m_Instance;
        }

        /// <summary>
        /// Returns in milliseconds how long it took for the pc to calculate and display the previous frame. 
        /// </summary>
        /// <example><code>Console.WriteLine(GAME_ENGINE.GetDeltaTime());</code></example>
        public float GetDeltaTime()
        {
            return m_LastDeltaTime;
        }

        /// <summary>
        /// Returns the width of the window in pixels.
        /// </summary>
        /// <example><code>Console.WriteLine(GAME_ENGINE.GetScreenWidth());</code></example>
        public int GetScreenWidth()
        {
            return m_Width;
        }

        /// <summary>
        /// Returns the height of the window in pixels.
        /// </summary>
        /// <example><code>Console.WriteLine(GAME_ENGINE.GetScreenHeight());</code></example>
        public int GetScreenHeight()
        {
            return m_Height;
        }

        /// <summary>
        /// Returns true if VSync is on.
        /// </summary>
        public bool GetVSync()
        {
            if (m_VSync == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Sets the title of your game in the top bar.
        /// </summary>
        /// <param name="title">The title of your game.</param>
        /// <example><code>
        /// GAME_ENGINE.SetTitle("My Awesome Game");
        /// </code></example>
        public void SetTitle(string title)
        {
            m_Title = title;
        }

        /// <summary>
        /// Sets the icon of your game in the top bar.
        /// </summary>
        /// <param name="iconPath">The path to your icon file, relative to the Assets folder.</param>
        /// <example><code>
        /// //if the icon in your Assets folder is named "myIcon.ico":
        /// GAME_ENGINE.SetIcon("myIcon.ico");
        /// </code></example>
        public void SetIcon(string iconPath)
        {
            #if DEBUG
                m_IconPath = ("../../Assets/" + iconPath);
            #else
                m_IconPath = "./Assets/" + iconPath;
            #endif
        }


        /// <summary>
        /// Sets the width of the game window in pixels.
        /// </summary>
        /// <param name="width">The width in pixels you want your window to be.</param>
        /// <example><code>GAME_ENGINE.SetScreenWidth(1920);</code></example>
        public void SetScreenWidth(int width)
        {
            if (width <= 0)
            {
                LogWarning("The screenwidth can not be smaller than 1 pixel. (SetWidth)");
                return;
            }

            m_Width = width;
        }

        /// <summary>
        /// Sets the height of the game window in pixels.
        /// </summary>
        /// <param name="height">The height you want the window to be.</param>
        /// <example><code>GAME_ENGINE.SetScreenHeight(1080);</code></example>
        public void SetScreenHeight(int height)
        {
            if (height <= 0)
            {
                LogWarning("The screenheight can not be smaller than 1 pixel. (SetHeight)");
                return;
            }

            m_Height = height;
        }

        /// <summary>
        /// Sets the background color with r (0-255), g (0-255) and b (0-255) values.
        /// </summary>
        /// <param name="r">Red (0-255)</param>
        /// <param name="g">Green (0-255)</param>
        /// <param name="b">Blue (0-255)</param>
        /// <Example><code>
        /// //Example for black: 
        /// GAME_ENGINE.SetBackgroundColor(0,0,0);
        /// //Example for blue: 
        /// GAME_ENGINE.SetBackgroundColor(0,0,255);
        /// </code></Example>
        public void SetBackgroundColor(int r, int g, int b)
        {
            m_ClearColor = new SharpDX.Color(r, g, b);
        }

        /// <summary>
        /// Sets the background color with a Color struct. 
        /// </summary>
        /// <param name="color">The color you want the background to be.</param>
        /// <example><code>
        /// Color myColor = new Color(0,255,0);
        /// GAME_ENGINE.SetBackgroundColor(myColor)
        /// </code></example>
        public void SetBackgroundColor(Color color)
        {
            SetBackgroundColor(color.R, color.G, color.B);
        }

        /// <summary>
        /// Turns Vsync on (true) or off (false).
        /// </summary>
        /// <param name="state"></param>
        /// <example><code>
        /// //Turns Vsync on:
        /// GAME_ENGINE.SetVsync(true);
        /// </code></example>
        public void SetVSync(bool state)
        {
            if (state == true)
                m_VSync = 1;
            else
                m_VSync = 0;
        }

        //Transformation methods
        public void SetRotation(float angle)
        {
            m_Angle = DegToRad(angle);
        }

        public void Rotate(float angle)
        {
            m_Angle += DegToRad(angle);
        }

        private float DegToRad(float degAngle)
        {
            return (float)(degAngle * (Math.PI / 180.0f)); //1 degree = PI / 180.0f
        }

        public void ResetRotation()
        {
            m_Angle = 0.0f;
        }

        public void SetScale(float x, float y)
        {
            m_Scale = new Vector2f(x, y);
        }

        public void ResetScale()
        {
            m_Scale = new Vector2f(1.0f, 1.0f);
        }

        private void SetTransformMatrix(Vector2f position, float angle, Vector2f scale, Vector2f transformCenter)
        {
            //Adjust the transform matrix
            SharpDX.Matrix3x2 matScale = SharpDX.Matrix3x2.Scaling(scale.X, scale.Y, new SharpDX.Vector2(0.0f, 0.0f)); //For now we only scale from the top left
            SharpDX.Matrix3x2 matRotate = SharpDX.Matrix3x2.Rotation(angle, new SharpDX.Vector2(transformCenter.X * m_Scale.X, transformCenter.Y * m_Scale.Y)); //For now we only rotate around the center
            SharpDX.Matrix3x2 matTranslate = SharpDX.Matrix3x2.Translation(position.X, position.Y);

            m_RenderTarget.Transform = matScale * matRotate * matTranslate;
        }

        private void ResetTransformMatrix()
        {
            m_RenderTarget.Transform = SharpDX.Matrix3x2.Identity;
        }


        //-------------------
        // Drawing methods
        //-------------------

        //Color methods
        /// <summary>
        /// Sets the color of the next Paint instruction.
        /// </summary>
        /// <param name="r">Red (0-255)</param>
        /// <param name="g">Green (0-255)</param>
        /// <param name="b">Blue (0-255)</param>
        /// <example><code>
        /// GAME_ENGINE.SetColor(255,0,0);
        /// //The next thing you draw will be in red.
        /// </code></example>
        public void SetColor(int r, int g, int b)
        {
            SetColor(r, g, b, 255);
        }

        /// <summary>
        /// Sets the color of the next paint instruction with Alpha (transparency).
        /// </summary>
        /// <param name="r">Red (0-255)</param>
        /// <param name="g">Green (0-255)</param>
        /// <param name="b">Blue (0-255)</param>
        /// <param name="a">Alpha (0-255)</param>
        /// <example><code>
        /// GAME_ENGINE.SetColor(255,0,0,128);
        /// //The next thing you draw will be in red and half transparent.
        /// </code></example>
        public void SetColor(int r, int g, int b, int a)
        {
            if (!PaintCheck())
                return;

            SharpDX.Color color = new SharpDX.Color(r, g, b, a);

            m_CurrentBrush.Dispose();
            m_CurrentBrush = new SolidColorBrush(m_RenderTarget, color);
        }

        /// <summary>
        /// Sets the color with a Color struct. 
        /// </summary>
        /// <param name="color">The color struct you want to use.</param>
        /// <example><code>
        /// Color myColor = new Color(0,255,0,128);
        /// GAME_ENGINE.SetColor(myColor);
        /// </code></example>
        public void SetColor(Color color)
        {
            SetColor(color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// Returns the color that has currently been set.
        /// </summary>
        /// <example><code>
        /// Console.WriteLine(GAME_ENGINE.GetColor())
        /// </code></example>
        public Color GetColor()
        {
            RawColor4 sharpDXColor = m_CurrentBrush.Color;
            Color color = new Color((int)(sharpDXColor.R * 255), (int)(sharpDXColor.G * 255), (int)(sharpDXColor.B * 255), (int)(sharpDXColor.A * 255));

            return color;
        }

        //Line
        /// <summary>
        /// Draws a line from startpointX-Y to endPointX-Y.
        /// </summary>
        /// <param name="startPointX">The x value in pixels you want to start drawing the line.</param>
        /// <param name="startPointY">The y value in pixels you want to start drawing the line.</param>
        /// <param name="endPointX">The x value in pixels you want the line to end.</param>
        /// <param name="endPointY">The y value in pixels you want the line to end.</param>
        /// <example><code>
        /// //A position on a screen is defined by an x value and a y value.
        /// //This will draw a line from starting point (0,0) to end point (100,100);
        /// GAME_ENGINE.DrawLine(0,0,100,100);
        /// </code></example>
        public void DrawLine(float startPointX, float startPointY, float endPointX, float endPointY)
        {
            //Not using default parameters on purpose.
            DrawLine(startPointX, startPointY, endPointX, endPointY, 1);
        }

        /// <summary>
        /// Draws a line from startPoint to endPoint.
        /// </summary>
        /// <param name="startPoint">A vector2f in pixels where to start the line.</param>
        /// <param name="endPoint">A vector2f in pixels where the line ends.</param>
        /// <example><code>
        /// Vector2f myStartPoint = new Vector2f(0,0);
        /// Vector2f myEndPoint = new Vector2f(300,300);
        /// GAME_ENGINE.DrawLine(myStartPoint, myEndPoint);
        /// </code></example>
        public void DrawLine(Vector2f startPoint, Vector2f endPoint)
        {
            DrawLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
        }

        /// <summary>
        /// Draws a line from startPointX-Y to endPointX-Y, with a specified width.
        /// </summary>
        /// <param name="startPointX">The x value in pixels you want to start drawing the line.</param>
        /// <param name="startPointY">The y value in pixels you want to start drawing the line.</param>
        /// <param name="endPointX">The x value in pixels you want the line to end.</param>
        /// <param name="endPointY">The y value in pixels you want the line to end.</param>
        /// <param name="strokeWidth">The width of the line.</param>
        /// <example><code>
        /// //A position on a screen is defined by an x value and a y value.
        /// //This will draw a line from starting point (0,0) to end point (100,100);
        /// GAME_ENGINE.DrawLine(0,0,100,100,5);
        /// </code></example>
        public void DrawLine(float startPointX, float startPointY, float endPointX, float endPointY, float strokeWidth)
        {
            if (!PaintCheck())
                return;

            //Adjust the transform matrix
            float width = endPointX - startPointX;
            float height = endPointY - startPointY;
            SetTransformMatrix(new Vector2f(startPointX, startPointY), m_Angle, m_Scale, new Vector2f(width * 0.5f, height * 0.5f));

            RawVector2 p1 = new RawVector2(0.0f, 0.0f);
            RawVector2 p2 = new RawVector2(width, height);

            m_RenderTarget.DrawLine(p1, p2, m_CurrentBrush, strokeWidth);

            ResetTransformMatrix();
        }

        /// <summary>
        /// Draws a line from startPoint to endPoint, with a specified width.
        /// </summary>
        /// <param name="startPoint">A vector2f in pixels where to start the line.</param>
        /// <param name="endPoint">A vector2f in pixels where the line ends.</param>
        /// <param name="strokeWidth">The width of the line.</param>
        /// <example><code>
        /// Vector2f myStartPoint = new Vector2f(0,0);
        /// Vector2f myEndPoint = new Vector2f(300,300);
        /// float myLineWidth = 5;
        /// GAME_ENGINE.DrawLine(myStartPoint, myEndPoint, myLineWidth);
        /// </code></example>
        public void DrawLine(Vector2f startPoint, Vector2f endPoint, float strokeWidth)
        {
            DrawLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y, strokeWidth);
        }

        //Rectangle
        /// <summary>
        /// Draws a rectangle from starting point x and y (topleft), with a specified width and height.
        /// </summary>
        /// <param name="x">The x starting position in pixels.</param>
        /// <param name="y">The y starting position in pixels.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <example><code>
        /// Draws a rectangle that starts on x:10 y:10 and has a width of 200 pixels and a height of 50: 
        /// GAME_ENGINE.DrawRectangle(10,10,200,50);
        /// </code></example>
        public void DrawRectangle(float x, float y, float width, float height)
        {
            //Not using default parameters on purpose.
            DrawRectangle(x, y, width, height, 1);
        }

        /// <summary>
        /// Draws a rectangle with a Rectanglef struct.
        /// </summary>
        /// <param name="rect">The rect to be used.</param>
        /// <example><code>
        /// Rectanglef myRect = new Rectanglef(10,10,200,50);
        /// GAME_ENGINE.DrawRectangle(myRect);
        /// </code></example>
        public void DrawRectangle(Rectanglef rect)
        {
            DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Draws a rectangle from starting point x and y (topleft), with a specified width, height and stroke width.
        /// </summary>
        /// <param name="x">The x starting position in pixels.</param>
        /// <param name="y">The y starting position in pixels.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="strokeWidth">The width of the stroke.</param>
        /// <example><code>
        /// Draws a rectangle that starts on x:10 y:10, has a width of 200 pixels, a height of 50 and a strokeWidth of 5.: 
        /// GAME_ENGINE.DrawRectangle(10,10,200,50, 5);
        /// </code></example>
        public void DrawRectangle(float x, float y, float width, float height, float strokeWidth)
        {
            if (!PaintCheck())
                return;

            //Adjust the transform matrix
            SetTransformMatrix(new Vector2f(x, y), m_Angle, m_Scale, new Vector2f(width * 0.5f, height * 0.5f));

            RawRectangleF rect = new RawRectangleF(0.0f, 0.0f, width, height);
            m_RenderTarget.DrawRectangle(rect, m_CurrentBrush, strokeWidth);

            //Reset the transform matrix
            ResetTransformMatrix();
        }

        /// <summary>
        /// Draws a rectangle with a Rectanglef struct and a specified stroke width.
        /// </summary>
        /// <param name="rect">The rect to be used.</param>
        /// <param name="strokeWidth">The width of the stroke.</param>
        /// <example><code>
        /// Rectanglef myRect = new Rectanglef(10,10,200,50);
        /// float myLineWidth = 5;
        /// GAME_ENGINE.DrawRectangle(myRect, myLineWidth);
        /// </code></example>
        public void DrawRectangle(Rectanglef rect, float strokeWidth)
        {
            DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height, strokeWidth);
        }

        /// <summary>
        /// Draws a <b>filled</b> rectangle from starting point x and y (topleft), with a specified width and height.
        /// </summary>
        /// <param name="x">The x starting position in pixels.</param>
        /// <param name="y">The y starting position in pixels.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <example><code>
        /// Draws a <b>filled</b> rectangle that starts on x:10 y:10 and has a width of 200 pixels and a height of 50: 
        /// GAME_ENGINE.DrawRectangle(10,10,200,50);
        /// </code></example>
        public void FillRectangle(float x, float y, float width, float height)
        {
            if (!PaintCheck())
                return;

            //Adjust the transform matrix
            SetTransformMatrix(new Vector2f(x, y), m_Angle, m_Scale, new Vector2f(width * 0.5f, height * 0.5f));

            //Actually render
            RawRectangleF rect = new RawRectangleF(0.0f, 0.0f, width, height);
            m_RenderTarget.FillRectangle(rect, m_CurrentBrush);

            //Reset the transform matrix
            ResetTransformMatrix();
        }

        /// <summary>
        /// Draws a <b>filled</b> rectangle with a specified Rectanglef.
        /// </summary>
        /// <param name="Rectanglef">The Rectanglef to be used.</param>
        /// <example><code>
        /// RectangleF myRect = new RectangleF(10,10,200,50);
        /// GAME_ENGINE.DrawRectangle(myRect);
        /// </code></example>
        public void FillRectangle(Rectanglef rect)
        {
            FillRectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        //Rounded rectangle
        /// <summary>
        /// Draws a rectangle with rounded corners.
        /// </summary>
        /// <param name="x">X starting point in pixels.</param>
        /// <param name="y">y starting point in pixels.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="radiusX">The x radius of the rounded corner.</param>
        /// <param name="radiusY">The y radius of the rounded corner.</param>
        /// <example><code>
        /// GAME_ENGINE.DrawRoundedRectangle(10,10,200,50,5,5);
        /// </code></example>
        public void DrawRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY)
        {
            DrawRoundedRectangle(x, y, width, height, radiusX, radiusY, 1);
        }

        /// <summary>
        /// Draws a rectangle with rounded corners.
        /// </summary>
        /// <param name="rect">The Rectanglef to be used.</param>
        /// <param name="radius">A Vector2f radius of the rounded corners.</param>
        /// <example><code>
        /// Rectanglef myRect = new Rectanglef(10,10,200,50);
        /// Vector2f myRadius = new Vector2f(5,5);
        /// GAME_ENGINE.DrawRoundedRectangle(myRect, myRadius);
        /// </code></example>
        public void DrawRoundedRectangle(Rectanglef rect, Vector2f radius)
        {
            DrawRoundedRectangle(rect.X, rect.Y, rect.Width, rect.Height, radius.X, radius.Y);
        }

        /// <summary>
        /// Draws a rectangle with rounded corners and a specified stroke width.
        /// </summary>
        /// <param name="x">X starting point in pixels.</param>
        /// <param name="y">y starting point in pixels.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="radiusX">The x radius of the rounded corner.</param>
        /// <param name="radiusY">The y radius of the rounded corner.</param>
        /// <param name="strokeWidth">The width of the stroke.</param>
        /// <example><code>
        /// GAME_ENGINE.DrawRoundedRectangle(10,10,200,50,5,5,3);
        /// </code></example>
        public void DrawRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY, float strokeWidth)
        {
            if (!PaintCheck())
                return;

            //Adjust the transform matrix
            SetTransformMatrix(new Vector2f(x, y), m_Angle, m_Scale, new Vector2f(width * 0.5f, height * 0.5f));

            RawRectangleF rect = new RawRectangleF(0.0f, 0.0f, width, height);

            RoundedRectangle roundedRect = new RoundedRectangle();
            roundedRect.Rect = rect;
            roundedRect.RadiusX = radiusX;
            roundedRect.RadiusY = radiusY;

            m_RenderTarget.DrawRoundedRectangle(roundedRect, m_CurrentBrush, strokeWidth);

            //Reset the transform matrix
            ResetTransformMatrix();
        }

        /// <summary>
        /// Draws a rectangle with rounded corners, and a specified stroke width.
        /// </summary>
        /// <param name="rect">The Rectanglef to be used.</param>
        /// <param name="radius">A Vector2f radius of the rounded corners.</param>
        /// <param name="strokeWidth">The width of the stroke.</param>
        /// <example><code>
        /// Rectanglef myRect = new Rectanglef(10,10,200,50);
        /// Vector2f myRadius = new Vector2f(5,5);
        /// float myStrokeWidth = 3f;
        /// GAME_ENGINE.DrawRoundedRectangle(myRect, myRadius, myStrokeWidth);
        /// </code></example>
        public void DrawRoundedRectangle(Rectanglef rect, Vector2f radius, int strokeWidth)
        {
            DrawRoundedRectangle(rect.X, rect.Y, rect.Width, rect.Height, radius.X, radius.Y, strokeWidth);
        }

        /// <summary>
        /// Draws a <b>filled</b> rectangle with rounded corners.
        /// </summary>
        /// <param name="x">X starting point in pixels.</param>
        /// <param name="y">y starting point in pixels.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="radiusX">The x radius of the rounded corner.</param>
        /// <param name="radiusY">The y radius of the rounded corner.</param>
        /// <example><code>
        /// GAME_ENGINE.FillRoundedRectangle(10,10,200,50,5,5);
        /// </code></example>
        public void FillRoundedRectangle(float x, float y, float width, float height, float radiusX, float radiusY)
        {
            if (!PaintCheck())
                return;

            //Adjust the transform matrix
            SetTransformMatrix(new Vector2f(x, y), m_Angle, m_Scale, new Vector2f(width * 0.5f, height * 0.5f));

            RawRectangleF rect = new RawRectangleF(0.0f, 0.0f, width, height);

            RoundedRectangle roundedRect = new RoundedRectangle();
            roundedRect.Rect = rect;
            roundedRect.RadiusX = radiusX;
            roundedRect.RadiusY = radiusY;

            m_RenderTarget.FillRoundedRectangle(ref roundedRect, m_CurrentBrush);

            //Reset the transform matrix
            ResetTransformMatrix();
        }

        /// <summary>
        /// Draws a <b>filled</b> rectangle with rounded corners.
        /// </summary>
        /// <param name="rect">The Rectanglef to be used.</param>
        /// <param name="radius">A Vector2f radius of the rounded corners.</param>
        /// <example><code>
        /// Rectanglef myRect = new Rectanglef(10,10,200,50);
        /// Vector2f myRadius = new Vector2f(5,5);
        /// GAME_ENGINE.FillRoundedRectangle(myRect, myRadius);
        /// </code></example>
        public void FillRoundedRectangle(Rectanglef rect, Vector2f radius)
        {
            FillRoundedRectangle(rect.X, rect.Y, rect.Width, rect.Height, radius.X, radius.Y);
        }

        //Ellipse
        /// <summary>
        /// Draws an ellipse.
        /// </summary>
        /// <param name="x">The x position of the ellipse.</param>
        /// <param name="y">The y position of the ellipse.</param>
        /// <param name="width">The width of the ellipse.</param>
        /// <param name="height">The height of the ellipse.</param>
        /// <example><code>
        /// GAME_ENGINE.DrawEllipse(100,100,50,50);
        /// </code></example>
        public void DrawEllipse(float x, float y, float width, float height)
        {
            //Not using default parameters on purpose.
            DrawEllipse(x, y, width, height, 1);
        }

        /// <summary>
        /// Draws an ellipse.
        /// </summary>
        /// <param name="rect">The Rectanglef to be used.</param>
        /// <example><code>
        /// //Draws an ellipse with startingpoint x:100 y:100 and the height and width at 50.
        /// Rectanglef myRect = new Rectanglef(100,100,50,50);
        /// GAME_ENGINE.DrawEllipse(myRect);
        /// </code></example>
        public void DrawEllipse(Rectanglef rect)
        {
            DrawEllipse(rect.X, rect.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Draws an ellipse with a specified stroke width.
        /// </summary>
        /// <param name="x">The x position of the ellipse.</param>
        /// <param name="y">The y position of the ellipse.</param>
        /// <param name="width">The width of the ellipse.</param>
        /// <param name="height">The height of the ellipse.</param>
        /// <param name="strokeWidth">The height of the ellipse.</param>
        /// <example><code>
        /// GAME_ENGINE.DrawEllipse(100,100,50,50, 5);
        /// </code></example>
        public void DrawEllipse(float x, float y, float width, float height, float strokeWidth)
        {
            if (!PaintCheck())
                return;

            //Adjust the transform matrix
            SetTransformMatrix(new Vector2f(x, y), m_Angle, m_Scale, new Vector2f(0.0f, 0.0f));

            Ellipse ellipse = new Ellipse(new RawVector2(0.0f, 0.0f), width, height);
            m_RenderTarget.DrawEllipse(ellipse, m_CurrentBrush, strokeWidth);

            //Reset the transform matrix
            ResetTransformMatrix();
        }

        /// <summary>
        /// Draws an ellipse with a specified stroke width.
        /// </summary>
        /// <param name="rect">The Rectanglef to be used.</param>
        /// <param name="strokeWidth">The width of the stroke.</param>
        /// <example><code>
        /// //Draws an ellipse with startingpoint x:100 y:100 and the height and width at 50.
        /// Rectanglef myRect = new Rectanglef(100,100,50,50);
        /// GAME_ENGINE.DrawEllipse(myRect, 5);
        /// </code></example>
        public void DrawEllipse(Rectanglef rect, float strokeWidth)
        {
            DrawEllipse(rect.X, rect.Y, rect.Width, rect.Height, strokeWidth);
        }

        /// <summary>
        /// Draws a <b>filled</b> ellipse.
        /// </summary>
        /// <param name="x">The x position of the ellipse.</param>
        /// <param name="y">The y position of the ellipse.</param>
        /// <param name="width">The width of the ellipse.</param>
        /// <param name="height">The height of the ellipse.</param>
        /// <example><code>
        /// GAME_ENGINE.FillEllipse(100,100,50,50);
        /// </code></example>
        public void FillEllipse(float x, float y, float width, float height)
        {
            if (!PaintCheck())
                return;


            //Adjust the transform matrix
            SetTransformMatrix(new Vector2f(x, y), m_Angle, m_Scale, new Vector2f(0.0f, 0.0f));

            Ellipse ellipse = new Ellipse(new RawVector2(0.0f, 0.0f), width, height);
            m_RenderTarget.FillEllipse(ellipse, m_CurrentBrush);

            //Reset the transform matrix
            ResetTransformMatrix();
        }

        /// <summary>
        /// Draws a <b>filled</b> ellipse.
        /// </summary>
        /// <param name="rect">The Rectanglef to be used.</param>
        /// <example><code>
        /// //Draws an ellipse with startingpoint x:100 y:100 and the height and width at 50.
        /// Rectanglef myRect = new Rectanglef(100,100,50,50);
        /// GAME_ENGINE.FillEllipse(myRect);
        /// </code></example>
        public void FillEllipse(Rectanglef rect)
        {
            FillEllipse(rect.X, rect.Y, rect.Width, rect.Height);
        }

        //Bitmaps
        /// <summary>
        /// Draws a bitmap.
        /// </summary>
        /// <param name="bitmap">The bitmap instance to be drawn.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <example><code>
        /// Bitmap myBitmap = new Bitmap("myPicture.jpg");
        /// GAME_ENGINE.DrawBitmap(myBitmap, 100,100);
        /// </code></example>
        public void DrawBitmap(Bitmap bitmap, float x, float y)
        {
            DrawBitmap(bitmap, x, y, 0, 0, 0, 0);
        }

        /// <summary>
        /// Draws a bitmap.
        /// </summary>
        /// <param name="bitmap">The bitmap instance to be drawn.</param>
        ///<param name="position">A vector2f for the position.</param>
        /// <example><code>
        /// Bitmap myBitmap = new Bitmap("myPicture.jpg");
        /// GAME_ENGINE.DrawBitmap(myBitmap, 100,100);
        /// </code></example>
        public void DrawBitmap(Bitmap bitmap, Vector2f position)
        {
            DrawBitmap(bitmap, position.X, position.Y);
        }

        /// <summary>
        /// Draws only a certain part of a bitmap.
        /// </summary>
        /// <param name="bitmap">The instance of the bitmap.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="sourceX">The <b>x</b> starting position within the supplied bitmap.</param>
        /// <param name="sourceY">The <b>y</b> starting position within the supplied bitmap.</param>
        /// <param name="sourceWidth">The width of the cut out picture.</param>
        /// <param name="sourceHeight">The height of the cut out picture.</param>
        /// <example><code>
        /// //Draws a bitmap at position x:0 y:0 with an offset of x:100 y:100 and a width and height of 200.
        /// Bitmap myBitmap = new Bitmap("myPicture.jpg");
        /// GAME_ENGINE.DrawBitmap(myBitmap, 0, 0, 100,100,200,200);
        /// </code></example>
        public void DrawBitmap(Bitmap bitmap, float x, float y, float sourceX, float sourceY, float sourceWidth, float sourceHeight)
        {
            if (!PaintCheck())
                return;

            SharpDX.Direct2D1.Bitmap D2DBitmap = bitmap.D2DBitmap;

            if (sourceWidth == 0)  sourceWidth = D2DBitmap.Size.Width;
            if (sourceHeight == 0) sourceHeight = D2DBitmap.Size.Height;

            //Adjust the transform matrix
            SetTransformMatrix(new Vector2f(x, y), m_Angle, m_Scale, new Vector2f(sourceWidth * 0.5f, sourceHeight * 0.5f));

            RawRectangleF sourceRect = new RawRectangleF(sourceX, sourceY, (sourceX + sourceWidth), (sourceY + sourceHeight));
            m_RenderTarget.DrawBitmap(D2DBitmap, m_CurrentBrush.Color.A, SharpDX.Direct2D1.BitmapInterpolationMode.NearestNeighbor, sourceRect);

            //Reset the transform matrix
            ResetTransformMatrix();
        }

        /// <summary>
        /// Draws only a certain part of a bitmap.
        /// </summary>
        /// <param name="bitmap">The instance of the bitmap.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="sourceRect">The Rectanglef of the cut out image.</param>
        /// <example><code>
        /// //Draws a bitmap at position x:0 y:0 with an offset of x:100 y:100 and a width and height of 200.
        /// Bitmap myBitmap = new Bitmap("myPicture.jpg");
        /// Rectanglef myRect = new Rectanglef(100,100,200,200);
        /// GAME_ENGINE.DrawBitmap(myBitmap, 0, 0, myRect);
        /// </code></example>
        public void DrawBitmap(Bitmap bitmap, int x, int y, Rectanglef sourceRect)
        {
            DrawBitmap(bitmap, x, y, sourceRect.X, sourceRect.Y, sourceRect.Width, sourceRect.Height);
        }

        /// <summary>
        /// Draws only a certain part of a bitmap.
        /// </summary>
        /// <param name="bitmap">The instance of the bitmap.</param>
        /// <param name="position">The Vector2f for the position.</param>
        /// <param name="sourceRect">The Rectanglef of the cut out image.</param>
        /// <example><code>
        /// //Draws a bitmap at position x:0 y:0 with an offset of x:100 y:100 and a width and height of 200.
        /// Bitmap myBitmap = new Bitmap("myPicture.jpg");
        /// Rectanglef myRect = new Rectanglef(100,100,200,200);
        /// Vector2f myPosition = new Vector2f(0,0);
        /// GAME_ENGINE.DrawBitmap(myBitmap, myPosition, myRect);
        /// </code></example>
        public void DrawBitmap(Bitmap bitmap, Vector2 position, Rectanglef sourceRect)
        {
            DrawBitmap(bitmap, position.X, position.Y, sourceRect.X, sourceRect.Y, sourceRect.Width, sourceRect.Height);
        }

        //Text
        /// <summary>
        /// Draws a string (text) at the specified location.
        /// </summary>
        /// <param name="text">The text to display.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width of the textbox.</param>
        /// <param name="height">The height of the textbox.</param>
        /// <example><code>
        /// GAME_ENGINE.DrawString("Hello world!", 0, 0, 100, 50);
        /// </code></example>
        public void DrawString(string text, float x, float y, float width, float height)
        {
            DrawString(null, text, x, y, width, height);
        }


        /// <summary>
        /// Draws a string (text) at the specified location.
        /// </summary>
        /// <param name="text">The text to display.</param>
        /// <param name="rect">The Rectanglef with positional data.</param>
        /// <example><code>
        /// Rectanglef myRect = new Rect(0,0,100,50);
        /// GAME_ENGINE.DrawString("Hello world!", myRect);
        /// </code></example>
        public void DrawString(string text, Rectanglef rect)
        {
            DrawString(text, rect.X, rect.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Draws a string (text) at the specified location with a specified font.
        /// </summary>
        /// <param name="font">The font to be used.</param>
        /// <param name="text">The text to display.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width of the textbox.</param>
        /// <param name="height">The height of the textbox.</param>
        /// <example><code>
        /// //Font needs to be supplied and initialized with a fontsize.
        /// Font myFont = new Font("Arial", 24);
        /// GAME_ENGINE.DrawString(myFont, "Hello world!", 0, 0, 100, 50);
        /// </code></example>
        public void DrawString(Font font, string text, float x, float y, float width, float height)
        {
            if (!PaintCheck())
                return;

            if (font == null)
                font = m_DefaultFont;

            //Adjust the transform matrix
            SetTransformMatrix(new Vector2f(x, y), m_Angle, m_Scale, new Vector2f(width * 0.5f, height * 0.5f));

            RawRectangleF rect = new RawRectangleF(0.0f, 0.0f, width, height);
            m_RenderTarget.DrawText(text, font.TextFormat, rect, m_CurrentBrush);

            //Reset the transform matrix
            ResetTransformMatrix();
        }

        /// <summary>
        /// Draws a string (text) at the specified location with a specified font.
        /// </summary>
        /// <param name="font">The font to be used.</param>
        /// <param name="text">The text to display.</param>
        /// <param name="rect">The Rectangle2f with the positional data.</param>
        /// <example><code>
        /// //Font needs to be supplied and initialized with a fontsize.
        /// Font myFont = new Font("Arial", 24);
        /// Rectanglef myRect = new Rect(0,0,100,50);
        /// GAME_ENGINE.DrawString(myFont, "Hello world!", myRect);
        /// </code></example>
        public void DrawString(Font font, string text, Rectanglef rect)
        {
            DrawString(font, text, rect.X, rect.Y, rect.Width, rect.Height);
        }

        //Input methods
        /// <summary>
        /// Returns true if the supplied key is being held down during this frame.
        /// </summary>
        /// <param name="key">The key you want to check.</param>
        /// <example><code>
        /// if (GAME_ENGINE.GetKey(Key.Space)){
        ///     Console.WriteLine(Space is being pressed);
        /// }
        /// </code></example>
        public bool GetKey(Key key)
        {
            return m_InputManager.GetKey(key);
        }

        /// <summary>
        /// Returns true if the supplied key has been pressed down in this frame.
        /// </summary>
        /// <param name="key">The key you want to check.</param>
        /// <example><code>
        /// if (GAME_ENGINE.GetKeyDown(Key.Space)){
        ///     Console.WriteLine(Space is being pressed for the first time.);
        /// }
        /// </code></example>
        public bool GetKeyDown(Key key)
        {
            return m_InputManager.GetKeyDown(key);
        }

        /// <summary>
        /// Returns true if the supplied key has been released during this frame.
        /// </summary>
        /// <param name="key">The key you want to check.</param>
        /// if (GAME_ENGINE.GetKeyUp(Key.Space)){
        ///     Console.WriteLine(User stopped pressing space.);
        /// }
        /// </code></example>
        public bool GetKeyUp(Key key)
        {
            return m_InputManager.GetKeyUp(key);
        }

        /// <summary>
        /// Returns true if a supplied mousebutton is being held down during this frame.
        /// </summary>
        /// <param name="buttonID">The mouse button to check (left:0 right:1 middle:2)</param>
        /// <example><code>
        /// if (GAME_ENGINE.GetMouseButton(0)){
        ///     Console.WriteLine(Left Mouse button is being pressed.);
        /// }
        /// if (GAME_ENGINE.GetMouseButton(1)){
        ///     Console.WriteLine(Right Mouse button is being pressed.);
        /// }
        /// </code></example>
        public bool GetMouseButton(int buttonID)
        {
            return m_InputManager.GetMouseButton(buttonID);
        }

        /// <summary>
        /// Returns true if the specified mouse button has been pressed during the current frame.
        /// </summary>
        /// <param name="buttonID">The mouse button to check (left:0 right:1 middle:2)</param>
        /// <example><code>
        /// if (GAME_ENGINE.GetMouseButtonDown(0)){
        ///     Console.WriteLine(Left Mouse button is pressed down right now.);
        /// }
        /// if (GAME_ENGINE.GetMouseButtonDown(1)){
        ///     Console.WriteLine(Right Mouse button is pressed down right now.);
        /// }
        /// </code></example>
        public bool GetMouseButtonDown(int buttonID)
        {
            return m_InputManager.GetMouseButtonDown(buttonID);
        }

        /// <summary>
        /// Returns true if the specified mouse button has been let go during the current frame.
        /// </summary>
        /// <param name="buttonID">The mouse button to check (left:0 right:1 middle:2)</param>
        /// <example><code>
        /// if (GAME_ENGINE.GetMouseButtonUp(0)){
        ///     Console.WriteLine(Left Mouse button is no longer pressed);
        /// }
        /// if (GAME_ENGINE.GetMouseButtonDown(2)){
        ///     Console.WriteLine(Middle Mouse button is no longer pressed.);
        /// }
        /// </code></example>
        public bool GetMouseButtonUp(int buttonID)
        {
            return m_InputManager.GetMouseButtonUp(buttonID);
        }

        /// <summary>
        /// Returns the current position of the mouse cursor in pixels.
        /// </summary>
        /// <example><code>
        /// Console.WriteLine(GAME_ENGINE.GetMousePosition());
        /// </code></example>
        public Vector2 GetMousePosition()
        {
            return m_InputManager.GetMousePosition();
        }

        //Audio methods
        /// <summary>
        /// Plays a specified Audio instance.
        /// </summary>
        /// <param name="audio">The sound to play.</param>
        /// <example><code>
        /// Audio myAudio = new Audio("mySong.wav");
        /// GAME_ENGINE.PlayAudio(myAudio);
        /// </code></example>
        public void PlayAudio(Audio audio)
        {
            m_AudioManager.PlayAudio(audio);
        }

        /// <summary>
        /// Stops a specified Audio instance.
        /// </summary>
        /// <param name="audio">The sound to play.</param>
        /// <example><code>
        /// Audio myAudio = new Audio("mySong.wav");
        /// GAME_ENGINE.PlayAudio(myAudio);
        /// ...
        /// GAME_ENGINE.StopAudio(myAudio);
        /// </code></example>
        public void StopAudio(Audio audio)
        {
            m_AudioManager.StopAudio(audio);
        }

        /// <summary>
        /// Sets the audio volume.
        /// </summary>
        /// <param name="volume">The specified volume (0 - 1)</param>
        /// <example><code>
        /// //Sets the volume to 50%:
        /// GAME_ENGINE.SetVolume(0.5f);
        /// </code></example>
        public void SetVolume(float volume)
        {
            if (volume < 0.0f)
            {
                LogError("The volume cannot be lower than 0.0");
                return;
            }

            if (volume > 1.0f)
            {
                LogError("The volume cannot be higher than 1.0");
                return;
            }

            m_AudioManager.SetVolume(volume);
        }

        //Logging
        /// <summary>
        /// Logs a debug message to the console in white.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <example><code>
        /// GAME_ENGINE.Log("Hello World!");
        /// </code></example>
        public void Log(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("DEBUG: " + message);
        }

        /// <summary>
        /// Logs a warning message to the console in yellow.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        /// <example><code>
        /// GAME_ENGINE.LogWarning("Careful now!");
        /// </code></example>
        public void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("WARNING: " + message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Logs an error message to the console in red.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        /// <example><code>
        /// GAME_ENGINE.LogError("Uh oh!!");
        /// </code></example>
        public void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR: " + message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        //Camera methods TODO

        private class InputManager
        {
            //Input snapshot of this frame
            private List<Keys> m_KeysPressed;
            private List<Keys> m_KeysDown;
            private List<Keys> m_KeysUp;

            private List<MouseButtons> m_MouseButtonsPressed;
            private List<MouseButtons> m_MouseButtonsDown;
            private List<MouseButtons> m_MouseButtonsUp;

            //Used to accuratly fill the arrays above
            private List<Keys> m_KeysDownLastFrame;
            private List<Keys> m_KeysUpLastFrame;
            private List<MouseButtons> m_MouseButtonsDownLastFrame;
            private List<MouseButtons> m_MouseButtonsUpLastFrame;

            private Vector2 m_MousePosition;

            public InputManager(RenderForm renderForm)
            {
                //Initialize the arrays
                m_KeysPressed = new List<Keys>();
                m_KeysDown = new List<Keys>();
                m_KeysUp = new List<Keys>();

                m_MouseButtonsPressed = new List<MouseButtons>();
                m_MouseButtonsDown = new List<MouseButtons>();
                m_MouseButtonsUp = new List<MouseButtons>();

                m_KeysDownLastFrame = new List<Keys>();
                m_KeysUpLastFrame = new List<Keys>();
                m_MouseButtonsDownLastFrame = new List<MouseButtons>();
                m_MouseButtonsUpLastFrame = new List<MouseButtons>();

                m_MousePosition = new Vector2(0, 0);

                StartListening(renderForm);
            }

            private void StartListening(RenderForm renderForm)
            {
                //Because this doesn't go in sync with our own Update loop we manage "Down & Up" ourselves.

                //Keys
                renderForm.KeyDown += (o, e) =>
                {
                    m_KeysDownLastFrame.Add(e.KeyCode);
                };

                renderForm.KeyUp += (o, e) =>
                {
                    m_KeysUpLastFrame.Add(e.KeyCode);
                };

                //Mouse
                renderForm.MouseDown += (o, e) =>
                {
                    m_MouseButtonsDownLastFrame.Add(e.Button);
                };

                renderForm.MouseUp += (o, e) =>
                {
                    m_MouseButtonsUpLastFrame.Add(e.Button);
                };

                renderForm.MouseMove += (o, e) =>
                {
                    m_MousePosition.X = e.X;
                    m_MousePosition.Y = e.Y;
                };
            }

            public void Update()
            {
                HandleKeyboardInput();
                HandleMouseInput();
            }

            private void HandleKeyboardInput()
            {
                m_KeysDown.Clear();
                m_KeysUp.Clear();

                foreach (Keys key in m_KeysDownLastFrame)
                {
                    if (m_KeysPressed.Contains(key) == false)
                    {
                        m_KeysDown.Add(key);
                        m_KeysPressed.Add(key);
                    }
                }

                foreach (Keys key in m_KeysUpLastFrame)
                {
                    if (m_KeysPressed.Contains(key) == true)
                    {
                        m_KeysUp.Add(key);
                        m_KeysPressed.Remove(key);
                    }
                }

                m_KeysDownLastFrame.Clear();
                m_KeysUpLastFrame.Clear();
            }

            private void HandleMouseInput()
            {
                m_MouseButtonsDown.Clear();
                m_MouseButtonsUp.Clear();

                foreach (MouseButtons button in m_MouseButtonsDownLastFrame)
                {
                    if (m_MouseButtonsPressed.Contains(button) == false)
                    {
                        m_MouseButtonsDown.Add(button);
                        m_MouseButtonsPressed.Add(button);
                    }
                }

                foreach (MouseButtons button in m_MouseButtonsUpLastFrame)
                {
                    if (m_MouseButtonsPressed.Contains(button) == true)
                    {
                        m_MouseButtonsUp.Add(button);
                        m_MouseButtonsPressed.Remove(button);
                    }
                }

                m_MouseButtonsDownLastFrame.Clear();
                m_MouseButtonsUpLastFrame.Clear();
            }

            //Functions mimic the way it works in Unity.
            public bool GetKey(Key key)
            {
                return m_KeysPressed.Contains((Keys)key);
            }

            public bool GetKeyDown(Key key)
            {
                return m_KeysDown.Contains((Keys)key);
            }

            public bool GetKeyUp(Key key)
            {
                return m_KeysUp.Contains((Keys)key);
            }

            public bool GetMouseButton(int buttonID)
            {
                MouseButtons button = TranslateMouseButton(buttonID);
                return m_MouseButtonsPressed.Contains(button);
            }

            public bool GetMouseButtonDown(int buttonID)
            {
                MouseButtons button = TranslateMouseButton(buttonID);
                return m_MouseButtonsDown.Contains(button);
            }

            public bool GetMouseButtonUp(int buttonID)
            {
                MouseButtons button = TranslateMouseButton(buttonID);
                return m_MouseButtonsUp.Contains(button);
            }

            public Vector2 GetMousePosition()
            {
                return m_MousePosition;
            }

            private MouseButtons TranslateMouseButton(int buttonID)
            {
                switch (buttonID)
                {
                    case 0: return MouseButtons.Left;
                    case 1: return MouseButtons.Right;
                    case 2: return MouseButtons.Middle;

                    default: return MouseButtons.None;
                }
            }
        }

        private class AudioManager
        {
            private WaveOut m_OutputDevice;
            private MixingSampleProvider m_Mixer;

            public AudioManager(int sampleRate, int channelCount)
            {
                m_OutputDevice = new WaveOut();

                m_Mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
                m_Mixer.ReadFully = true;
                m_Mixer.RemoveAllMixerInputs();

                m_OutputDevice.Init(m_Mixer);
                m_OutputDevice.Play();
            }

            public void Dispose()
            {
                m_OutputDevice.Stop();
                m_OutputDevice.Dispose();
            }

            public void PlayAudio(Audio audio)
            {
                m_Mixer.AddMixerInput(ConvertToRightChannelCount(audio));
            }

            public void StopAudio(Audio audio)
            {
                m_Mixer.RemoveMixerInput(ConvertToRightChannelCount(audio));
            }

            public void SetVolume(float volume)
            {
                m_OutputDevice.Volume = volume;
            }

            private ISampleProvider ConvertToRightChannelCount(ISampleProvider input)
            {
                if (input.WaveFormat.Channels == m_Mixer.WaveFormat.Channels)
                {
                    return input;
                }

                if (input.WaveFormat.Channels == 1 && m_Mixer.WaveFormat.Channels == 2)
                {
                    return new MonoToStereoSampleProvider(input);
                }

                GameEngine.GetInstance().LogError("No such channel conversion currently exists.");
                return input;
            }
        }
    }

    #region DataTypes

    /// <summary>
    /// The default gameobject. 
    /// </summary>
    public class GameObject
    {
        private GameEngine m_GameEngine;
        /// <summary>
        /// Instance of the current GameEngine.
        /// </summary>
        protected GameEngine GAME_ENGINE
        {
            get { return m_GameEngine; }
        }

        private bool m_IsActive = true;

        public GameObject()
        {
            m_GameEngine = GameEngine.GetInstance();
            m_GameEngine.SubscribeGameObject(this);
        }

        public virtual void Dispose()
        {
            m_GameEngine.UnsubscribeGameObject(this);
        }

        /// <summary>
        /// Initializes the game.
        /// </summary>
        public virtual void GameInitialize() { }

        /// <summary>
        /// Gets called when the game starts.
        /// </summary>
        public virtual void GameStart() { }

        /// <summary>
        /// Gets called when the game ends.
        /// </summary>
        public virtual void GameEnd()
        {
            Dispose();
        }

        /// <summary>
        /// This runs every frame.
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// Use the Paint function for all the drawing function.
        /// </summary>
        public virtual void Paint() { }

        //Mutators
        /// <summary>
        /// Turns a GameObject instance on or off.
        /// </summary>
        public void SetActive(bool isActive)
        {
            if (m_IsActive == isActive)
                return;

            //Totally unsubscribe this object during inactivity in order to save CPU cycles.
            if (isActive == true) { m_GameEngine.SubscribeGameObject(this); }
            else                  { m_GameEngine.UnsubscribeGameObject(this); }

            m_IsActive = isActive;
        }

        //Accessors
        /// <summary>
        /// Returns true if the current GameObject is active.
        /// </summary>
        public bool IsActive()
        {
            return m_IsActive;
        }
    }

    /// <summary>
    /// Used for displaying images.
    /// </summary>
    public class Bitmap : IDisposable
    {
        private SharpDX.Direct2D1.Bitmap m_D2DBitmap;
        public SharpDX.Direct2D1.Bitmap D2DBitmap
        {
            get { return m_D2DBitmap; }
        }

        /// <summary>
        /// Create a new bitmap instance with a supplied filepath.
        /// </summary>
        /// <param name="filePath">The location of the image you want to use. Relative to the Assets folder.</param>
        /// <example><code>
        /// //For a picture called myPicture.jpg that is in my Assets folder:
        /// Bitmap myBitmap = new Bitmap("myPicture.jpg");
        /// </code></example>
        public Bitmap(string filePath)
        {
            #if DEBUG
                LoadBitmap("../../Assets/" + filePath);
            #else
                LoadBitmap("./Assets/" + filePath);
            #endif
        }

        /// <summary>
        /// Clears the Bitmap from memory.
        /// </summary>
        public void Dispose()
        {
            //This is the destructor
            if (m_D2DBitmap != null)
            {
                m_D2DBitmap.Dispose();
                m_D2DBitmap = null;
            }
        }

        private void LoadBitmap(string filePath)
        {
            //Read the image
            ImagingFactory imagingFactory = new ImagingFactory();
            NativeFileStream fileStream = new NativeFileStream(filePath, NativeFileMode.Open, NativeFileAccess.Read);

            //Decode and get the frame (decodes all sorts of image formats for us)
            BitmapDecoder bitmapDecoder = new BitmapDecoder(imagingFactory, fileStream, DecodeOptions.CacheOnDemand);
            BitmapFrameDecode frame = bitmapDecoder.GetFrame(0);

            //Convert the colors to the Direct2D standard
            FormatConverter converter = new FormatConverter(imagingFactory);
            converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppPRGBA);

            RenderTarget renderTarget = GameEngine.GetInstance().RenderTarget;
            m_D2DBitmap = SharpDX.Direct2D1.Bitmap1.FromWicBitmap(renderTarget, converter);
        }

        //private void SetTransparancyColor(Color color)
        //{
            //To be made, for now use PNG
            //new SharpDX.Direct2D1.Effects.ColorManagement()
        //}

            /// <summary>
            /// Returns the width of the Bitmap.
            /// </summary>
        public float GetWidth()
        {
            return m_D2DBitmap.Size.Width;
        }

        /// <summary>
        /// Returns the height of the Bitmap.
        /// </summary>
        /// <returns></returns>
        public float GetHeight()
        {
            return m_D2DBitmap.Size.Height;
        }
    }
    
    /// <summary>
    /// Used for displaying fonts.
    /// </summary>
    public class Font : IDisposable
    {
        public enum Alignment
        {
            Left,
            Right,
            Center
        }

        private SharpDX.DirectWrite.TextFormat m_TextFormat;
        public SharpDX.DirectWrite.TextFormat TextFormat
        {
            get { return m_TextFormat; }
        }

        /// <summary>
        /// Creates a Font instance to be used to display text.
        /// </summary>
        /// <param name="fontName">The name of the font.</param>
        /// <param name="size">The fontsize.</param>
        public Font(string fontName, float size)
        {
            CreateFont(fontName, size);
        }

        /// <summary>
        /// Releases the font from memory.
        /// </summary>
        public void Dispose()
        {
            //This is the destructor
            if (m_TextFormat != null)
            {
                m_TextFormat.Dispose();
                m_TextFormat = null;
            }
        }

        private void CreateFont(string fontName, float size)
        {
            SharpDX.DirectWrite.Factory fontFactory = new SharpDX.DirectWrite.Factory();
            m_TextFormat = new SharpDX.DirectWrite.TextFormat(fontFactory, fontName, size);
            fontFactory.Dispose();
        }
        
        /// <summary>
        /// Sets the horizontal alignment of the text.
        /// </summary>
        /// <param name="alignment">Alignment.Center, Alignment.Left or Alignment.Right</param>
        /// <example><code>
        /// Font myFont = new Font("Arial", 24);
        /// myFont.SetHorizontalAlignment(Alignment.Center);
        /// </code></example>
        public void SetHorizontalAlignment(Alignment alignment)
        {
            m_TextFormat.TextAlignment = (SharpDX.DirectWrite.TextAlignment)alignment;
        }

        /// <summary>
        /// Sets the vertical alignment of the text.
        /// </summary>
        /// <param name="alignment">Alignment.Center, Alignment.Left or Alignment.Right</param>
        /// <example><code>
        /// Font myFont = new Font("Arial", 24);
        /// myFont.SetVerticalAlignment(Alignment.Center);
        /// </code></example>
        public void SetVerticalAlignment(Alignment alignment)
        {
            m_TextFormat.ParagraphAlignment = (SharpDX.DirectWrite.ParagraphAlignment)alignment;
        }
    }

    /// <summary>
    /// Used for displaying buttons.
    /// </summary>
    public class Button : GameObject
    {
        public delegate void ButtonCallbackNoArgs();
        public delegate void ButtonCallback(params object[] args);

        //State
        private Font m_DefaultFont;
        private Font m_Font;
        private Bitmap m_Bitmap; //Instead of colors we can also use a bitmap as the background

        private bool m_IsHovering;
        private bool m_IsClicked;

        //Required
        private string m_Text = "Button";
        private Rectanglef m_Rectangle = new Rectanglef(0, 0, 100, 25);

        //Callbacks
        private ButtonCallbackNoArgs m_OnClickCallbackNoArgs; //Only used to introduce students. Otherwise always use m_OnClickCallback
        private ButtonCallback m_OnClickCallback;
        private object[] m_OnClickArgs;

        private ButtonCallback m_OnHoverBeginCallback;
        private object[] m_OnHoverBeginArgs;

        private ButtonCallback m_OnHoverEndCallback;
        private object[] m_OnHoverEndArgs;

        //Extra settings
        private bool m_ShowForeground = true;
        private bool m_ShowBackground = true;
        private bool m_ShowBorder = true;

        private Color m_ForegroundColor = Color.Black;
        private Color m_BackgroundColor = Color.White;
        private Color m_BorderColor = Color.Black;

        private Color m_HoverForegroundColor = Color.Black;
        private Color m_HoverBackgroundColor = new Color(245, 245, 245);
        private Color m_HoverBorderColor = Color.Black;

        private Color m_ClickForegroundColor = Color.Black;
        private Color m_ClickBackgroundColor = new Color(200, 200, 200);
        private Color m_ClickBorderColor = Color.Black;

        private Vector2f m_BorderCornerRadius = new Vector2f(0.0f, 0.0f);
        private int m_BorderWidth = 1;

        //Functions
        /// <summary>
        /// Creates a default Button. 
        /// </summary>
        /// <example><code>
        /// Button myButton = new Button();
        /// </code></example>
        public Button() : base()
        {
            Initialize(null, "Button", new Rectanglef(0, 0, 100, 25));
        }

        /// <summary>
        /// Creates a Button.
        /// </summary>
        /// <param name="text">The text on the button.</param>
        /// <param name="x">The <b>x</b> position in pixels.</param>
        /// <param name="y">The <b>y</b> position in pixels.</param>
        /// <param name="width">The width of the button in pixels.</param>
        /// <param name="height">The height of the button in pixels.</param>
        /// <example><code>
        /// //Creates a Button that says Click me!:
        /// Button myButton = new Button("Click me!", 0, 0, 100,50);
        /// </code></example>
        public Button(string text, int x, int y, int width, int height) : base()
        {
            Initialize(null, text, new Rectanglef(x, y, width, height));
        }

        /// <summary>
        /// Creates a Button.
        /// </summary>
        /// <param name="text">The text on the button.</param>
        /// <param name="rectangle">The RectangleF of the button.</param>
        /// <example><code>
        /// //Creates a Button that says Click me!:
        /// RectangleF myRect = 0, 0, 100, 50;
        /// Button myButton = new Button("Click me!", myRect);
        /// </code></example>
        public Button(string text, Rectanglef rectangle) : base()
        {
            Initialize(null, text, rectangle);
        }

        /// <summary>
        /// Creates a button with a callback (no arguments).   
        /// </summary>
        /// <param name="callback">The function to trigger when the button is clicked.</param>
        /// <example><code>
        /// //Create the function:
        /// /// void OnClick(){
        ///     GAME_ENGINE.Log("Clicked me!");
        /// }
        /// 
        /// //For example at gamestart create a button:
        /// public override void GameStart(){
        ///     Button myButton = new Button(new Button.ButtonCallbackNoArgs(OnClick));
        /// }
        /// </code></example>
        public Button(ButtonCallbackNoArgs callback) : base()
        {
            Initialize(callback, "Button", new Rectanglef(0, 0, 100, 25));
        }

        /// <summary>
        /// Creates a button with a callback (no arguments).   
        /// </summary>
        /// <param name="callback">The function to trigger when the button is clicked.</param>
        /// <param name="text">The text on the button.</param>
        /// <param name="x">The <b>x</b> position in pixels.</param>
        /// <param name="y">The <b>y</b> position in pixels.</param>
        /// <param name="width">The width of the button in pixels.</param>
        /// <param name="height">The height of the button in pixels.</param>
        /// <example><code>
        /// //Create the function:
        /// /// void OnClick(){
        ///     GAME_ENGINE.Log("Clicked me!");
        /// }
        /// 
        /// //For example at gamestart create a button:
        /// public override void GameStart(){
        ///     Button myButton = new Button(new Button.ButtonCallbackNoArgs(OnClick), "Click me!", 0, 0, 200, 50);
        /// }
        /// </code></example>
        public Button(ButtonCallbackNoArgs callback, string text, int x, int y, int width, int height) : base()
        {
            Initialize(callback, text, new Rectanglef(x, y, width, height));
        }

        /// <summary>
        /// Creates a button with a callback (no arguments).   
        /// </summary>
        /// <param name="callback">The function to trigger when the button is clicked.</param>
        /// <param name="text">The text on the button.</param>
        /// <param name="rectangle">The Rectanglef to be used.</param>
        /// <example><code>
        /// //Create the function:
        /// /// void OnClick(){
        ///     GAME_ENGINE.Log("Clicked me!");
        /// }
        /// 
        /// //For example at gamestart create a button:
        /// public override void GameStart(){
        ///     Rectanglef myRect = new Rectanglef(0,0,200,50);
        ///     Button myButton = new Button(new Button.ButtonCallbackNoArgs(OnClick), "Click me!", myRect);
        /// }
        /// </code></example>
        public Button(ButtonCallbackNoArgs callback, string text, Rectanglef rectangle) : base()
        {
            Initialize(callback, text, rectangle);
        }


        private void Initialize(ButtonCallbackNoArgs callback, string text, Rectanglef rectangle)
        {
            //Allowing this would needlessly lengthen the update time.
            if (rectangle.Width < 1)
            {
                GAME_ENGINE.LogWarning("A button was given a WIDTH of 0 or less. It has been inverted.");
                rectangle.Width *= -1;
            }

            if (rectangle.Height < 1)
            {
                GAME_ENGINE.LogWarning("A button was given a HEIGHT of 0 or less. It has been inverted.");
                rectangle.Height *= -1;
            }

            m_Text = text;
            m_Rectangle = rectangle;
            m_OnClickCallbackNoArgs = callback;

            m_DefaultFont = new Font("Arial", 12.0f);
            m_DefaultFont.SetHorizontalAlignment(Font.Alignment.Center);
            m_DefaultFont.SetVerticalAlignment(Font.Alignment.Center);

            m_Font = m_DefaultFont;
        }

        /// <summary>
        /// Removes the button from memory.
        /// </summary>
        public override void Dispose()
        {
            //Only the default font was created by us.
            if (m_DefaultFont != null)
            {
                m_DefaultFont.Dispose();
                m_DefaultFont = null;
            }

            base.Dispose();
        }

        public override void GameEnd()
        {
            Dispose();
            base.GameEnd();
        }

        public override void Update()
        {
            if (!base.IsActive())
                return;

            //Check if the mouse position is colliding with our button
            Vector2 mousePosition = GAME_ENGINE.GetMousePosition();

            m_IsClicked = false;

            bool wasHovering = m_IsHovering;
            m_IsHovering = (!(mousePosition.X < m_Rectangle.X ||
                              mousePosition.X > (m_Rectangle.X + m_Rectangle.Width) ||
                              mousePosition.Y < m_Rectangle.Y ||
                              mousePosition.Y > (m_Rectangle.Y + m_Rectangle.Height)));

            if (m_IsHovering)
            {
                m_IsClicked = GAME_ENGINE.GetMouseButton(0);

                //We clicked
                if (GAME_ENGINE.GetMouseButtonUp(0))
                {
                    if (m_OnClickCallbackNoArgs != null)
                        m_OnClickCallbackNoArgs();

                    if (m_OnClickCallback != null)
                        m_OnClickCallback(m_OnClickArgs);
                }

                //We started hovering
                if (wasHovering == false)
                {
                    if (m_OnHoverBeginCallback != null)
                        m_OnHoverBeginCallback(m_OnHoverBeginArgs);
                }
            }
            else
            {
                //We stopped hovering
                if (wasHovering == true)
                {
                    if (m_OnHoverEndCallback != null)
                        m_OnHoverEndCallback(m_OnHoverEndArgs);
                }
            }
        }

        public override void Paint()
        {
            if (!base.IsActive())
                return;

            GAME_ENGINE.ResetRotation();

            Color fgColor = m_ForegroundColor;
            Color bgColor = m_BackgroundColor;
            Color borderColor = m_BorderColor;

            if (m_IsHovering)
            {
                fgColor = m_HoverForegroundColor;
                bgColor = m_HoverBackgroundColor;
                borderColor = m_HoverBorderColor;
            }

            if (m_IsClicked)
            {
                fgColor = m_ClickForegroundColor;
                bgColor = m_ClickBackgroundColor;
                borderColor = m_ClickBorderColor;
            }

            //Background
            if (m_Bitmap != null)
            {
                DrawBitmapButton();
            }
            else
            {
                if (m_BorderCornerRadius.X == 0 && m_BorderCornerRadius.Y == 0)
                {
                    if (m_ShowBackground) { DrawRectangleButton(bgColor); }
                    if (m_ShowBorder)     { DrawRectangleButtonBorder(borderColor); }
                }
                else
                {
                    if (m_ShowBackground) { DrawRoundedRectangleButton(bgColor); }
                    if (m_ShowBorder)     { DrawRoundedRectangleButtonBorder(borderColor); }
                }
            }

            //Text
            if (m_ShowForeground)
            {
                GAME_ENGINE.SetColor(fgColor);
                GAME_ENGINE.DrawString(m_Font, m_Text, m_Rectangle);
            }
        }


        private void DrawRectangleButton(Color backgroundColor)
        {
            GAME_ENGINE.SetColor(backgroundColor);
            GAME_ENGINE.FillRectangle(m_Rectangle);
        }

        private void DrawRectangleButtonBorder(Color borderColor)
        {
            GAME_ENGINE.SetColor(borderColor);
            GAME_ENGINE.DrawRectangle(m_Rectangle, m_BorderWidth);
        }

        private void DrawRoundedRectangleButton(Color backgroundColor)
        {
            GAME_ENGINE.SetColor(backgroundColor);
            GAME_ENGINE.FillRoundedRectangle(m_Rectangle, m_BorderCornerRadius);
        }

        private void DrawRoundedRectangleButtonBorder(Color borderColor)
        {
            GAME_ENGINE.SetColor(borderColor);
            GAME_ENGINE.DrawRoundedRectangle(m_Rectangle, m_BorderCornerRadius, m_BorderWidth);
        }

        private void DrawBitmapButton()
        {
            float yOffset = 0;
            if (m_IsHovering) yOffset += m_Rectangle.Height;
            if (m_IsClicked)  yOffset += m_Rectangle.Height;

            GAME_ENGINE.DrawBitmap(m_Bitmap, m_Rectangle.X, m_Rectangle.Y, 0, yOffset, m_Rectangle.Width, m_Rectangle.Height);
        }


        //Mutators
        /// <summary>
        /// Sets the function the button should trigger when clicked.
        /// </summary>
        /// <param name="callback">The function to run.</param>
        /// <example><code>
        /// void OnClick(){
        ///     GAME_ENGINE.Log("Clicked me!");
        /// }
        /// 
        /// //When creating the button:
        /// Button myButton = new Button();
        /// myButton.SetClickCallbackNoArgs(ClickMe);
        /// </code></example>
        public void SetClickCallbackNoArgs(ButtonCallbackNoArgs callback)
        {
            m_OnClickCallbackNoArgs = callback;
        }

        /// <summary>
        /// Sets the function the button should trigger when clicked and send some data to be used.
        /// </summary>
        /// <param name="callback">The function to run.</param>
        /// <param name="args">An array of objects to be used.</param>
        /// <example><code>
        /// private void ClickMe(Object[] number)
        /// {
        ///     //This will Log "Hi":
        ///     GAME_ENGINE.Log((string)number[0]);
        /// }
        /// 
        /// //When creating the button:
        /// Button myButton = new Button();
        /// myButton.SetClickCallback(ClickMe, new object[] { "Hi" });
        /// </code></example>
        public void SetClickCallback(ButtonCallback callback, params object[] args)
        {
            m_OnClickCallback = callback;
            m_OnClickArgs = args;
        }

        /// <summary>
        /// Sets the function the button should trigger when hovered over and send some data to be used.
        /// </summary>
        /// <param name="callback">The function to run.</param>
        /// <param name="args">An array of objects to be used.</param>
        /// <example><code>
        /// private void OnHover(Object[] number)
        /// {
        ///     //This will Log "Hi":
        ///     GAME_ENGINE.Log((string)number[0]);
        /// }
        /// 
        /// //When creating the button:
        /// Button myButton = new Button();
        /// myButton.SetBeginHoverCallback(OnHover, new object[] { "Hi" });
        /// </code></example>
        public void SetBeginHoverCallback(ButtonCallback callback, params object[] args)
        {
            m_OnHoverBeginCallback = callback;
            m_OnHoverBeginArgs = args;
        }

        /// <summary>
        /// Sets the function the button should trigger when no longer hovered over and send some data to be used.
        /// </summary>
        /// <param name="callback">The function to run.</param>
        /// <param name="args">An array of objects to be used.</param>
        /// <example><code>
        /// private void OnEndHover(Object[] number)
        /// {
        ///     //This will Log "Hi":
        ///     GAME_ENGINE.Log((string)number[0]);
        /// }
        /// 
        /// //When creating the button:
        /// Button myButton = new Button();
        /// myButton.SetEndHoverCallback(OnEndHover, new object[] { "Hi" });
        /// </code></example>
        public void SetEndHoverCallback(ButtonCallback callback, params object[] args)
        {
            m_OnHoverEndCallback = callback;
            m_OnHoverEndArgs = args;
        }

        /// <summary>
        /// Sets the text displayed on the button.
        /// </summary>
        /// <param name="text">The displayed text.</param>
        /// <example><code>
        /// //This will set the buttons text to "Hello World"
        /// myButton.SetText("Hello World");
        /// </code></example>
        public void SetText(string text)
        {
            m_Text = text;
        }

        /// <summary>
        /// Sets the position of the button.
        /// </summary>
        /// <param name="x">The X position in pixels.</param>
        /// <param name="y">The Y position in pixels.</param>
        /// <example><code>
        /// //Moves the button to x:100 y:100.
        /// myButton.SetPosition(100,100);
        /// </code></example>
        public void SetPosition(int x, int y)
        {
            m_Rectangle = new Rectanglef(x, y, m_Rectangle.Width, m_Rectangle.Height);
        }

        /// <summary>
        /// Sets the size of the button in pixels.
        /// </summary>
        /// <param name="width">The width of the button in pixels.</param>
        /// <param name="height">The height of the button in pixels.</param>
        /// <example><code>
        /// //Sets myButton to 200 width and 50 pixels high.
        /// myButton.SetSize(200,50);
        /// </code></example>
        public void SetSize(int width, int height)
        {
            m_Rectangle = new Rectanglef(m_Rectangle.X, m_Rectangle.Y, width, height);
        }

        /// <summary>
        /// Sets the button to use the supplied Rectanglef.
        /// </summary>
        /// <param name="rectangle">The Rectanglef to be used.</param>
        /// <example><code>
        /// //Sets myButton to have a position of x:100 y:100, width of 200 and height of 50
        /// Rectanglef myButtonRect = new Rectanglef(100,100,200,50);
        /// myButton.SetRectangle(myButtonRect);
        /// </code></example>
        public void SetRectangle(Rectanglef rectangle)
        {
            m_Rectangle = rectangle;
        }

        /// <summary>
        /// Toggle the foreground of the button.
        /// </summary>
        /// <param name="state">True or false</param>
        /// <example><code>
        /// //Hides the foreground of myButton:
        /// myButton.ShowForeground(false);
        /// </code></example>
        public void ShowForeground(bool state)
        {
            m_ShowForeground = state;
        }

        /// <summary>
        /// Toggle the background of the button.
        /// </summary>
        /// <param name="state">True or false</param>
        /// <example><code>
        /// //Hides the background of myButton:
        /// myButton.ShowBackground(false);
        /// </code></example>
        public void ShowBackground(bool state)
        {
            m_ShowBackground = state;
        }

        /// <summary>
        /// Toggle the border of the button.
        /// </summary>
        /// <param name="state">True or false</param>
        /// <example><code>
        /// //Hides the border of myButton:
        /// myButton.ShowBorder(false);
        /// </code></example>
        public void ShowBorder(bool state)
        {
            m_ShowBorder = state;
        }

        /// <summary>
        /// Sets the foregroundcolor of the button.
        /// </summary>
        /// <param name="color">The supplied Color</param>
        /// <example><code>
        /// //Sets the foreground color of myButton to red.
        /// myColor = new Color(255,0,0);
        /// myButton.SetForegroundColor(myColor);
        /// </code></example>
        public void SetForegroundColor(Color color)
        {
            m_ForegroundColor = color;
        }


        /// <summary>
        /// Sets the background color of the button.
        /// </summary>
        /// <param name="color">The supplied Color</param>
        /// <example><code>
        /// //Sets the background color of myButton to red.
        /// myColor = new Color(255,0,0);
        /// myButton.SetBackgroundColor(myColor);
        /// </code></example>
        public void SetBackgroundColor(Color color)
        {
            m_BackgroundColor = color;
        }

        /// <summary>
        /// Sets the border color of the button.
        /// </summary>
        /// <param name="color">The supplied Color</param>
        /// <example><code>
        /// //Sets the border color of myButton to black.
        /// myColor = new Color(0,0,0);
        /// myButton.SetBorderColor(myColor);
        /// </code></example>
        public void SetBorderColor(Color color)
        {
            m_BorderColor = color;
        }

        /// <summary>
        /// Sets the hover foreground color of the button.
        /// </summary>
        /// <param name="color">The supplied Color</param>
        /// <example><code>
        /// //Sets the hover foreground color of myButton to green.
        /// myColor = new Color(0,255,0);
        /// myButton.SetHoverForegroundColor(myColor);
        /// </code></example>
        public void SetHoverForegroundColor(Color color)
        {
            m_HoverForegroundColor = color;
        }

        /// <summary>
        /// Sets the hover background color of the button.
        /// </summary>
        /// <param name="color">The supplied Color</param>
        /// <example><code>
        /// //Sets the hover background color of myButton to green.
        /// myColor = new Color(0,255,0);
        /// myButton.SetHoverBackgroundColor(myColor);
        /// </code></example>
        public void SetHoverBackgroundColor(Color color)
        {
            m_HoverBackgroundColor = color;
        }

        /// <summary>
        /// Sets the hover border color of the button.
        /// </summary>
        /// <param name="color">The supplied Color</param>
        /// <example><code>
        /// //Sets the hover border color of myButton to green.
        /// myColor = new Color(0,255,0);
        /// myButton.SetHoverBorderColor(myColor);
        /// </code></example>
        public void SetHoverBorderColor(Color color)
        {
            m_HoverBorderColor = color;
        }

        /// <summary>
        /// Sets the click foreground color of the button.
        /// </summary>
        /// <param name="color">The supplied Color</param>
        /// <example><code>
        /// //Sets the click foreground color of myButton to green.
        /// myColor = new Color(0,255,0);
        /// myButton.SetClickForegroundColor(myColor);
        /// </code></example>
        public void SetClickForegroundColor(Color color)
        {
            m_ClickForegroundColor = color;
        }

        /// <summary>
        /// Sets the click background color of the button.
        /// </summary>
        /// <param name="color">The supplied Color</param>
        /// <example><code>
        /// //Sets the click background color of myButton to green.
        /// myColor = new Color(0,255,0);
        /// myButton.SetClickBackgroundColor(myColor);
        /// </code></example>
        public void SetClickBackgroundColor(Color color)
        {
            m_ClickBackgroundColor = color;
        }

        /// <summary>
        /// Sets the click border color of the button.
        /// </summary>
        /// <param name="color">The supplied Color</param>
        /// <example><code>
        /// //Sets the click border color of myButton to green.
        /// myColor = new Color(0,255,0);
        /// myButton.SetClickBorderColor(myColor);
        /// </code></example>
        public void SetClickBorderColor(Color color)
        {
            m_ClickBorderColor = color;
        }

        /// <summary>
        /// Sets the bitmap to be used by the bitmap.
        /// </summary>
        /// <param name="bitmap">The Bitmap to be used</param>
        /// <example><code>
        /// //Sets myButton to look like button.png 
        /// Bitmap myBitmap = new Bitmap("button.png");
        /// myButton.SetBitmap(myBitmap);
        /// </code></example>
        public void SetBitmap(Bitmap bitmap)
        {
            m_Bitmap = bitmap;
        }

        /// <summary>
        /// Sets the font of the button.
        /// </summary>
        /// <param name="font">The Font to be used.</param>
        /// <example><code>
        /// //Sets the button's text font to arial with a fontsize of 24.
        /// Font myFont = new Font("arial", 24);
        /// myButton.SetFont(myFont);
        /// </code></example>
        public void SetFont(Font font)
        {
            m_Font = font;
        }

        /// <summary>
        /// Sets the radius of the buttons' corners.
        /// </summary>
        /// <param name="radius">The specified radius as Vector2f.</param>
        /// <example><code>
        /// Vector2f myBorderRadius = new Vector2f(5,5);
        /// myButton.SetBorderCornerRadius(myBorderRadius);
        /// </code></example>
        public void SetBorderCornerRadius(Vector2f radius)
        {
            m_BorderCornerRadius = radius;
        }

        /// <summary>
        /// Sets the width of the buttons' border.
        /// </summary>
        /// <param name="width">The desired width in pixels.</param>
        /// <example><code>
        /// myButton.SetBorderWidth(10);
        /// </code></example>
        public void SetBorderWidth(int width)
        {
            m_BorderWidth = width;
        }
    }

    /// <summary>
    /// Used for audio.
    /// </summary>
    public class Audio : IDisposable, ISampleProvider
    {
        private float[] m_OriginalData;
        private float[] m_Data;
        private long m_CurrentPosition;

        private float m_Volume = 1.0f;
        private bool m_IsLooping = false;

        private WaveFormat m_WaveFormat;
        public WaveFormat WaveFormat
        {
            get { return m_WaveFormat; }
        }

        public Audio(string filePath)
        {
            #if DEBUG
                LoadAudio("../../Assets/" + filePath);
            #else
                LoadAudio("./Assets/" + filePath);
            #endif
        }

        private void LoadAudio(string filePath)
        {
            using (AudioFileReader audioFileReader = new AudioFileReader(filePath))
            {
                //Make sure all audio uses the same samplerate
                WaveToSampleProvider convStream = new WaveToSampleProvider(new MediaFoundationResampler(new SampleToWaveProvider(audioFileReader),
                                                                                                        WaveFormat.CreateIeeeFloatWaveFormat(44100, 2)) { ResamplerQuality = 60 });

                
                m_WaveFormat = convStream.WaveFormat;

                List<float> wholeFile = new List<float>((int)(audioFileReader.Length / 4));
                float[] readBuffer = new float[convStream.WaveFormat.SampleRate * convStream.WaveFormat.Channels];

                int samplesRead;
                while ((samplesRead = convStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                {
                    wholeFile.AddRange(readBuffer.Take(samplesRead));
                }

                m_OriginalData = wholeFile.ToArray();
                m_Data = m_OriginalData;
            }
        }

        /// <summary>
        /// Removes the audio from memory
        /// </summary>
        /// <example><code>myAudio.Dispose();</code></example>
        public void Dispose()
        {
            m_WaveFormat = null;
        }

        /// <summary>
        /// Sets the volume of the audio.
        /// </summary>
        /// <param name="volume">Volume of the audioclip (0-1)</param>
        /// <example><code>
        /// //Sets volume of myAudio to 50%.
        /// myAudio.SetVolume(0.5f);
        /// </code></example>
        public void SetVolume(float volume)
        {
            //We'll just do this manually otherwise finding a way out of all these conversion classes (see LoadAudio) will be a nightmare.
            //As long as we don't use this to fadeout we'll be fine.
            for (int i = 0; i < m_OriginalData.Length; ++i)
            {
                m_Data[i] = m_OriginalData[i] * volume;
            }
        }

        /// <summary>
        /// Sets looping of audio.
        /// </summary>
        /// <param name="looping">True or false</param>
        /// <example><code>
        /// //If I want my audio to loop:
        /// myAudio.SetLooping(true);
        /// </code></example>
        public void SetLooping(bool looping)
        {
            m_IsLooping = looping;
        }

        /// <summary>
        /// Returns the current volume of the audio as a float.
        /// </summary>
        /// <example><code>Console.WriteLine(myAudio.GetVolume());</code></example>
        public float GetVolume()
        {
            return m_Volume;
        }

        /// <summary>
        /// Returns true if this audio is looping.
        /// </summary>
        /// <example><code>
        /// Console.WriteLine(myAudio.IsLooping());
        /// </code></example>
        public bool IsLooping()
        {
            return m_IsLooping;
        }

        //ISampleProvider
        public int Read(float[] buffer, int offset, int count)
        {
            long availableSamples = m_Data.Length - m_CurrentPosition;

            //Looping
            if (availableSamples <= 0)
            {
                m_CurrentPosition = 0;
                availableSamples = 0;

                if (m_IsLooping == true)
                {
                    availableSamples = m_Data.Length - m_CurrentPosition;
                }
            }


            long samplesToCopy = Math.Min(availableSamples, count);

            Array.Copy(m_Data, m_CurrentPosition, buffer, offset, samplesToCopy);

            m_CurrentPosition += samplesToCopy;

            return (int)samplesToCopy;
        }
    }

    #endregion
}
