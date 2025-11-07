using System;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Reproducto_Musica
{
    internal class AudioPlayer
    {
        private IWavePlayer waveOut;
        private AudioFileReader audioFileReader;
<<<<<<< Updated upstream
        private SampleAggregator sampleAggregator;
        private VolumeSampleProvider volumeProvider;
=======
        private EventHandler<StoppedEventArgs> playbackStoppedHandler;
>>>>>>> Stashed changes

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
            // Si está en pausa, reanuda
            if (waveOut != null && waveOut.PlaybackState == PlaybackState.Paused)
            {
                waveOut.Play();
                return;
            }

            // Antes de cargar una nueva pista, liberar recursos anteriores
            Unload();

<<<<<<< Updated upstream
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
=======
            try
            {
                audioFileReader = new AudioFileReader(filePath);
                waveOut = new WaveOutEvent();
                waveOut.Init(audioFileReader);

                playbackStoppedHandler = (s, e) => OnPlaybackStopped?.Invoke(this, EventArgs.Empty);
                waveOut.PlaybackStopped += playbackStoppedHandler;

                waveOut.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al reproducir: " + ex.Message);
                Unload();
                throw;
            }
>>>>>>> Stashed changes
        }

        // Comportamiento tipo Windows Media Player: detiene y pone posición a 0,
        // pero mantiene la pista cargada (no libera objetos).
        public void Stop()
        {
            try
            {
                if (waveOut != null)
                    waveOut.Stop();

                if (audioFileReader != null)
                    audioFileReader.CurrentTime = TimeSpan.Zero;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al detener el audio: " + ex.Message);
            }
        }

        // Detiene y libera recursos (usar antes de cargar otra pista)
        public void Unload()
        {
            try
            {
                if (waveOut != null)
                {
                    if (playbackStoppedHandler != null)
                    {
                        waveOut.PlaybackStopped -= playbackStoppedHandler;
                        playbackStoppedHandler = null;
                    }

                    waveOut.Stop();
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
                Console.WriteLine("Error al descargar recursos de audio: " + ex.Message);
            }
        }

        public void Pause()
        {
            if (waveOut != null)
                waveOut.Pause();
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
