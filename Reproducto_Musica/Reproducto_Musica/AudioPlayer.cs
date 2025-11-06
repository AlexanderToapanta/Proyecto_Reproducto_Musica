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
    }
}
