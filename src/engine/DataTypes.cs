using System;
using System.Collections.Generic;
using System.Linq;

using SharpDX.Direct2D1;
using SharpDX.WIC;
using SharpDX.IO;

using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Project;

public enum Key
{
    Back = 8,
    Tab = 9,
    Clear = 12,
    Return = 13,
    Enter = 13,
    ShiftKey = 16,
    ControlKey = 17,
    Menu = 18,
    Pause = 19,
    CapsLock = 20,
    Escape = 27,
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
}

public class GameObject
{
    public GameEngine Engine => GameEngine.Instance;
    public bool isActive = true;

    public GameObject() => Engine.subscribingGameObjects.Add(this);

    public virtual void Destroy() => Engine.unsubscribingGameObjects.Add(this);
    public virtual void GameEnd() => Destroy();
    public virtual void GameInitialize(){}
    public virtual void GameStart(){}
    public virtual void Update(){}
    public virtual void Paint(){}

    public void SetActive(bool isActive)
    {
        if (this.isActive == isActive) return;
        if (isActive == true) Engine.subscribingGameObjects.Add(this);
        else Engine.unsubscribingGameObjects.Add(this);
        this.isActive = isActive;
    }
}

public class Bitmap
{
    public SharpDX.Direct2D1.Bitmap dxbitmap;
    public float Width => dxbitmap.Size.Width;
    public float Height => dxbitmap.Size.Height;

    public Bitmap(string filePath) => LoadFromFile("res/" + filePath);

    private void LoadFromFile(string filePath)
    {
        ImagingFactory imagingFactory = new();
        NativeFileStream fileStream = new(filePath, NativeFileMode.Open, NativeFileAccess.Read);
        BitmapDecoder bitmapDecoder = new(imagingFactory, fileStream, DecodeOptions.CacheOnDemand);
        BitmapFrameDecode frame = bitmapDecoder.GetFrame(0);
        FormatConverter converter = new(imagingFactory);
        converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppPRGBA);
        RenderTarget renderTarget = GameEngine.Instance.renderTarget;
        dxbitmap = Bitmap1.FromWicBitmap(renderTarget, converter);
    }
}

public class Font : IDisposable
{
    public enum Alignment
    {
        Left,
        Right,
        Center
    }

    public SharpDX.DirectWrite.TextFormat format;

    public Font(string name, float size) => CreateFont(name, size);

    public void Dispose()
    {
        if (format != null)
        {
            format.Dispose();
            format = null;
        }
    }

    private void CreateFont(string fontName, float size)
    {
        SharpDX.DirectWrite.Factory fontFactory = new();
        format = new SharpDX.DirectWrite.TextFormat(fontFactory, fontName, size);
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

    public AudioClip(string filePath) => LoadAudio("res/" + filePath);

    private void LoadAudio(string filePath)
    {
        using AudioFileReader audioFileReader = new(filePath);
        var provider = new SampleToWaveProvider(audioFileReader);
        var resampler = new MediaFoundationResampler(provider, WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
        resampler.ResamplerQuality = 60;
        waveFormat = new WaveToSampleProvider(resampler).WaveFormat;
        List<float> wholeFile = new((int)(audioFileReader.Length / 4));
        
        float[] readBuffer = new float[new WaveToSampleProvider(new MediaFoundationResampler(new SampleToWaveProvider(audioFileReader), WaveFormat.CreateIeeeFloatWaveFormat(44100, 2))
        {
            ResamplerQuality = 60
        }).WaveFormat.SampleRate * new WaveToSampleProvider(new MediaFoundationResampler(new SampleToWaveProvider(audioFileReader), WaveFormat.CreateIeeeFloatWaveFormat(44100, 2))
        {
            ResamplerQuality = 60
        }).WaveFormat.Channels];

        int samplesRead;
        while ((samplesRead = new WaveToSampleProvider(new MediaFoundationResampler(new SampleToWaveProvider(audioFileReader), WaveFormat.CreateIeeeFloatWaveFormat(44100, 2))
        { ResamplerQuality = 60 }).Read(readBuffer, 0, readBuffer.Length)) > 0)
        {
            wholeFile.AddRange(readBuffer.Take(samplesRead));
        }

        originalData = wholeFile.ToArray();
        data = originalData;
    }

    public void SetVolume(float volume)
    {
        for (int i = 0; i < originalData.Length; ++i) data[i] = originalData[i] * volume;
    }

    public void SetLooping(bool looping) => this.looping = looping;
    public float GetVolume() => volume;
    public bool IsLooping() => looping;

    public int Read(float[] buffer, int offset, int count)
    {
        long availableSamples = data.Length - currentPosition;

        if (availableSamples <= 0)
        {
            currentPosition = 0;
            availableSamples = 0;

            if (looping == true)
            {
                availableSamples = data.Length - currentPosition;
            }
        }

        long samplesToCopy = Math.Min(availableSamples, count);
        Array.Copy(data, currentPosition, buffer, offset, samplesToCopy);
        currentPosition += samplesToCopy;
        return (int)samplesToCopy;
    }
}