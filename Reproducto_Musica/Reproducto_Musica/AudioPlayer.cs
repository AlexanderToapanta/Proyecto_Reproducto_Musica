using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Reproducto_Musica
{
    internal class AudioPlayer
    {
        private IWavePlayer waveOut;
        private AudioFileReader audioFileReader;

        public event EventHandler OnPlaybackStopped;

        public void Play(string filePath)
        {
            if (waveOut != null && waveOut.PlaybackState == PlaybackState.Paused)
            {
                waveOut.Play();
                return;
            }

            Stop();

            audioFileReader = new AudioFileReader(filePath);
            waveOut = new WaveOutEvent();
            waveOut.Init(audioFileReader);
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
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }

            if (audioFileReader != null)
            {
                audioFileReader.Dispose();
                audioFileReader = null;
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
