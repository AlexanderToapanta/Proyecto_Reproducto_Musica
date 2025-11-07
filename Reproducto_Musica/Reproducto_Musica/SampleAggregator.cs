using System;
using NAudio.Wave;

namespace Reproducto_Musica
{
    internal class SampleAggregator : ISampleProvider
    {
        private readonly ISampleProvider source;

        // new signature: provide float[] samples directly
        public Action<float[]> SamplesAvailable;

        // optional FFT processor is handled inside VisualizerControl now

        public SampleAggregator(ISampleProvider source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public WaveFormat WaveFormat => source.WaveFormat;

        public int Read(float[] buffer, int offset, int count)
        {
            int read = source.Read(buffer, offset, count);
            if (read > 0)
            {
                // copy into a new array of length 'read' to provide contiguous samples
                float[] copy = new float[read];
                Array.Copy(buffer, offset, copy, 0, read);
                SamplesAvailable?.Invoke(copy);
            }
            return read;
        }
    }
}
