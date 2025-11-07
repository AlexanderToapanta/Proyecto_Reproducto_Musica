using System;
using NAudio.Dsp;

namespace Reproducto_Musica
{
    internal class FftProcessor
    {
        private readonly int fftSize;
        private readonly float[] window;
        private readonly float[] buffer;
        private int bufferPos = 0;
        private readonly int hopSize;
        private readonly object lockObj = new object();

        // magnitudes for subscribers
        public event Action<float[]> MagnitudesAvailable;

        public FftProcessor(int fftSize = 1024)
        {
            if ((fftSize & (fftSize - 1)) != 0) // must be power of two
                throw new ArgumentException("fftSize must be power of two");
            this.fftSize = fftSize;
            window = new float[fftSize];
            buffer = new float[fftSize];
            hopSize = fftSize / 2; // 50% overlap

            // Hanning window
            for (int i = 0; i < fftSize; i++)
            {
                window[i] = (float)(0.5 * (1 - Math.Cos(2 * Math.PI * i / (fftSize - 1))));
            }
        }

        public void Add(float[] samples)
        {
            lock (lockObj)
            {
                int srcOffset = 0;
                while (srcOffset < samples.Length)
                {
                    int toCopy = Math.Min(samples.Length - srcOffset, fftSize - bufferPos);
                    Array.Copy(samples, srcOffset, buffer, bufferPos, toCopy);
                    bufferPos += toCopy;
                    srcOffset += toCopy;

                    if (bufferPos >= fftSize)
                    {
                        ProcessBuffer();

                        // shift buffer for hop/overlap
                        Array.Copy(buffer, hopSize, buffer, 0, fftSize - hopSize);
                        bufferPos = fftSize - hopSize;
                    }
                }
            }
        }

        private void ProcessBuffer()
        {
            // prepare complex buffer
            Complex[] complex = new Complex[fftSize];
            for (int i = 0; i < fftSize; i++)
            {
                complex[i].X = buffer[i] * window[i];
                complex[i].Y = 0f;
            }

            // perform FFT (in-place)
            FastFourierTransform.FFT(true, (int)Math.Log(fftSize, 2), complex);

            int half = fftSize / 2;
            float[] mags = new float[half];
            for (int i = 0; i < half; i++)
            {
                float re = complex[i].X;
                float im = complex[i].Y;
                mags[i] = (float)Math.Sqrt(re * re + im * im);
            }

            MagnitudesAvailable?.Invoke(mags);
        }
    }
}
