using System;
using System.Collections.Generic;
using System.Linq;

using SharpDX.Direct2D1;
using SharpDX.WIC;
using SharpDX.IO;

using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Spork;

public class GameObject
{
    public Engine engine => Engine.Instance;
    public bool isActive = true;

    public GameObject() => engine.subscribingGameObjects.Add(this);

    public virtual void Destroy() => engine.unsubscribingGameObjects.Add(this);
    public virtual void GameEnd() => Destroy();
    public virtual void GameInitialize(){}
    public virtual void GameStart(){}
    public virtual void Update(){}
    public virtual void Paint(){}

    public void SetActive(bool isActive)
    {
        if (this.isActive == isActive) return;
        if (isActive == true) engine.subscribingGameObjects.Add(this);
        else engine.unsubscribingGameObjects.Add(this);
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
        RenderTarget renderTarget = Engine.Instance.renderTarget;
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