using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Reproducto_Musica
{
    internal class AudioPlayer
    {
        private IWavePlayer waveOut;
        private AudioFileReader audioFileReader;
        private SampleAggregator sampleAggregator;
        private VolumeSampleProvider volumeProvider;

        public event EventHandler OnPlaybackStopped;
        public event Action<float[]> SamplesAvailable;

        private float volume = 1.0f;

        public float Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                if (volumeProvider != null)
                {
                    volumeProvider.Volume = volume;
                }
            }
        }
        public void Play(string filePath)
        {
            if (waveOut != null && waveOut.PlaybackState == PlaybackState.Paused)
            {
                waveOut.Play();
                return;
            }

            Stop();

            audioFileReader = new AudioFileReader(filePath);

            var sampleProvider = audioFileReader.ToSampleProvider();
            sampleAggregator = new SampleAggregator(sampleProvider);
            sampleAggregator.SamplesAvailable += (buffer) =>
            {
                SamplesAvailable?.Invoke(buffer);
            };

            volumeProvider = new VolumeSampleProvider(sampleAggregator);

            var waveProvider = new SampleToWaveProvider16(volumeProvider);

            waveOut = new WaveOutEvent();
            // Initialize with the provider that wraps the SampleAggregator so it will be read
            waveOut.Init(waveProvider);
            waveOut.PlaybackStopped += (s, e) => OnPlaybackStopped?.Invoke(this, EventArgs.Empty);
            waveOut.Play();
        }

        public void Pause()
        {
            if (waveOut != null)
                waveOut.Pause();
        }

        public void Stop()
        {
            try
            {
                if (waveOut != null)
                {

                    waveOut.Stop();


                    waveOut.PlaybackStopped -= (s, e) => OnPlaybackStopped?.Invoke(this, EventArgs.Empty);

                    waveOut.Dispose();
                    waveOut = null;
                }

                if (audioFileReader != null)
                {
                    audioFileReader.Dispose();
                    audioFileReader = null;
                }
                
                if (sampleAggregator != null)
                {
                    sampleAggregator.SamplesAvailable = null;
                    sampleAggregator = null;
                }

                volumeProvider = null;
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error al detener el audio: " + ex.Message);
            }
        }

        public double GetCurrentTime()
        {
            return audioFileReader != null ? audioFileReader.CurrentTime.TotalSeconds : 0;
        }
        public double GetTotalTime()
        {
            return audioFileReader != null ? audioFileReader.TotalTime.TotalSeconds : 0;
        }
        public void SetPosition(double seconds)
        {
            if (audioFileReader != null)
                audioFileReader.CurrentTime = TimeSpan.FromSeconds(seconds);
        }

    }

}
