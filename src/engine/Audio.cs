using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Project;

public class Audio
{
    private WaveOut outputDevice;
    private MixingSampleProvider mixer;

    public Audio()
    {
        outputDevice = new();
        mixer = new(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
        mixer.ReadFully = true;
        mixer.RemoveAllMixerInputs();
        outputDevice.Init(mixer);
        outputDevice.Play();
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
}