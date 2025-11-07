using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;

namespace Reproducto_Musica
{
    public enum VisualMode
    {
        Barras,
        Onda,
        Espectro
    }

    public class VisualizerControl : Control
    {
        private readonly object bufferLock = new object();
        private float[] sampleBuffer = new float[0];

        // FFT
        private FftProcessor fftProcessor;
        private float[] spectrum = new float[0];
        private readonly object spectrumLock = new object();

        public VisualMode Mode { get; set; } = VisualMode.Barras;
        public Color BarColor { get; set; } = Color.LimeGreen;
        public Color PeakColor { get; set; } = Color.Yellow;

        public VisualizerControl()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);

            // default FFT
            fftProcessor = new FftProcessor(1024);
            fftProcessor.MagnitudesAvailable += OnFftMagnitudes;
        }

        public void AddSamples(float[] samples)
        {
            lock (bufferLock)
            {
                // append and keep recent
                int keep = Math.Min(sampleBuffer.Length, Width * 4);
                int newSize = Math.Min(keep + samples.Length, Width * 8);
                float[] newBuf = new float[newSize];
                if (keep > 0)
                    Array.Copy(sampleBuffer, sampleBuffer.Length - keep, newBuf, 0, keep);
                int toCopy = Math.Min(samples.Length, newSize - keep);
                Array.Copy(samples, 0, newBuf, keep, toCopy);
                sampleBuffer = newBuf;
            }

            // feed FFT
            fftProcessor.Add(samples);

            if (IsHandleCreated)
                BeginInvoke((Action)(() => Invalidate()));
        }

        private void OnFftMagnitudes(float[] mags)
        {
            lock (spectrumLock)
            {
                if (spectrum == null || spectrum.Length != mags.Length)
                    spectrum = new float[mags.Length];
                for (int i = 0; i < mags.Length; i++)
                {
                    float v = (float)(Math.Log10(1 + mags[i]) / 2.0);
                    spectrum[i] = spectrum[i] * 0.6f + v * 0.4f;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.Clear(BackColor);

            float[] bufferCopy;
            lock (bufferLock)
            {
                bufferCopy = new float[sampleBuffer.Length];
                Array.Copy(sampleBuffer, bufferCopy, bufferCopy.Length);
            }

            if (bufferCopy.Length == 0) return;

            int w = Width;
            int h = Height;

            if (Mode == VisualMode.Barras)
            {
                int barWidth = Math.Max(4, w / 60);
                int spacing = Math.Max(2, barWidth / 2);
                int bars = Math.Max(1, w / (barWidth + spacing));
                int samplesPerBar = Math.Max(1, bufferCopy.Length / bars);

                using (Brush barBrush = new SolidBrush(BarColor))
                using (Brush peakBrush = new SolidBrush(PeakColor))
                {
                    for (int i = 0; i < bars; i++)
                    {
                        int start = i * samplesPerBar;
                        int len = Math.Min(samplesPerBar, bufferCopy.Length - start);
                        if (len <= 0) break;
                        float max = 0f;
                        for (int j = 0; j < len; j++) { float a = Math.Abs(bufferCopy[start + j]); if (a > max) max = a; }
                        int barHeight = (int)(Math.Min(1f, max) * (h - 6));
                        int x = i * (barWidth + spacing);
                        int y = (h - barHeight) / 2;
                        g.FillRectangle(barBrush, new Rectangle(x, y, barWidth, barHeight));
                        int capHeight = Math.Max(2, Math.Min(10, (int)(barHeight * 0.08)));
                        g.FillRectangle(peakBrush, new Rectangle(x, y - capHeight - 1, barWidth, capHeight));
                    }
                }
            }
            else if (Mode == VisualMode.Onda)
            {
                using (Pen pen = new Pen(Color.Cyan))
                {
                    int mid = h / 2;
                    int points = w;
                    float samplesPerPoint = Math.Max(1f, (float)bufferCopy.Length / points);
                    Point[] pts = new Point[points];
                    for (int x = 0; x < points; x++)
                    {
                        int start = (int)(x * samplesPerPoint);
                        int len = Math.Min((int)samplesPerPoint, bufferCopy.Length - start);
                        float sum = 0f;
                        for (int j = 0; j < len; j++) sum += bufferCopy[start + j];
                        float avg = sum / Math.Max(1, len);
                        int y = mid - (int)(avg * mid);
                        pts[x] = new Point(x, y);
                    }
                    g.DrawLines(pen, pts);
                }
            }
            else if (Mode == VisualMode.Espectro)
            {
                float[] specCopy;
                lock (spectrumLock)
                {
                    if (spectrum == null || spectrum.Length == 0) return;
                    specCopy = new float[spectrum.Length];
                    Array.Copy(spectrum, specCopy, specCopy.Length);
                }

                int bars = Math.Max(32, Math.Min(specCopy.Length, w / 4));
                int barWidth = Math.Max(2, w / bars);
                int spacing = 1;

                using (Brush barBrush = new SolidBrush(Color.OrangeRed))
                {
                    for (int i = 0; i < bars; i++)
                    {
                        int idx = (int)((i / (float)bars) * specCopy.Length);
                        float v = specCopy[idx];
                        int barHeight = (int)(Math.Min(1f, v) * (h - 4));
                        int x = i * (barWidth + spacing);
                        int y = h - barHeight;
                        g.FillRectangle(barBrush, new Rectangle(x, y, barWidth, barHeight));
                    }
                }
            }
        }
    }
}
