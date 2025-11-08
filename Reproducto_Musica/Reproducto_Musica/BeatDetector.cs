using System;

namespace Reproducto_Musica
{
    public class BeatEventArgs : EventArgs
    {
        public bool IsStrongBeat { get; set; }
        public float BeatStrength { get; set; }
        public double TimeStamp { get; set; }
    }

    internal class BeatDetector
    {
        private readonly object lockObj = new object();
        private float[] energyHistory;
        private int energyHistoryIndex = 0;
        private const int HISTORY_SIZE = 43; // ~1 segundo a 44.1kHz
        private float lastBeatTime = 0;
        private const float MIN_BEAT_INTERVAL = 0.3f; // Mínimo 300ms entre beats
        
        // Configuración de detección
        private const float BEAT_THRESHOLD = 1.3f; // Multiplicador para detectar beat
        private const float STRONG_BEAT_THRESHOLD = 1.8f; // Beat fuerte
        
        // BPM tracking
        private float[] beatIntervals;
        private int beatIntervalIndex = 0;
        private const int BPM_HISTORY_SIZE = 8;
        private int currentBPM = 0;

        // Eventos públicos
        public event EventHandler<BeatEventArgs> OnBeatDetected;
        public event EventHandler<int> BpmChanged;

        public int CurrentBPM => currentBPM;
        public bool IsBeat { get; private set; }
        public float BeatStrength { get; private set; }

        public BeatDetector()
        {
            energyHistory = new float[HISTORY_SIZE];
            beatIntervals = new float[BPM_HISTORY_SIZE];
            IsBeat = false;
            BeatStrength = 0f;
        }

        public void ProcessSamples(float[] samples)
        {
            if (samples == null || samples.Length == 0) return;

            lock (lockObj)
            {
                // Calcular energía instantánea
                float instantEnergy = CalculateEnergy(samples);
                
                // Agregar a historial
                energyHistory[energyHistoryIndex] = instantEnergy;
                energyHistoryIndex = (energyHistoryIndex + 1) % HISTORY_SIZE;
                
                // Calcular energía promedio local
                float localAverage = CalculateLocalAverage();
                
                // Detectar beat
                DetectBeat(instantEnergy, localAverage);
            }
        }

        private float CalculateEnergy(float[] samples)
        {
            float sum = 0f;
            for (int i = 0; i < samples.Length; i++)
            {
                sum += samples[i] * samples[i];
            }
            return sum / samples.Length;
        }

        private float CalculateLocalAverage()
        {
            float sum = 0f;
            int count = 0;
            
            for (int i = 0; i < HISTORY_SIZE; i++)
            {
                if (energyHistory[i] > 0)
                {
                    sum += energyHistory[i];
                    count++;
                }
            }
            
            return count > 0 ? sum / count : 0f;
        }

        private void DetectBeat(float instantEnergy, float localAverage)
        {
            float currentTime = Environment.TickCount / 1000f;
            
            // Reset beat state
            IsBeat = false;
            BeatStrength = 0f;
            
            if (localAverage > 0 && instantEnergy > localAverage * BEAT_THRESHOLD)
            {
                // Verificar intervalo mínimo entre beats
                if (currentTime - lastBeatTime > MIN_BEAT_INTERVAL)
                {
                    IsBeat = true;
                    BeatStrength = instantEnergy / localAverage;
                    
                    // Calcular intervalo de beat para BPM
                    float beatInterval = currentTime - lastBeatTime;
                    UpdateBPM(beatInterval);
                    
                    // Determinar si es beat fuerte
                    bool isStrongBeat = BeatStrength > STRONG_BEAT_THRESHOLD;
                    
                    // Disparar evento
                    OnBeatDetected?.Invoke(this, new BeatEventArgs
                    {
                        IsStrongBeat = isStrongBeat,
                        BeatStrength = BeatStrength,
                        TimeStamp = currentTime
                    });
                    
                    lastBeatTime = currentTime;
                }
            }
        }

        private void UpdateBPM(float beatInterval)
        {
            // Agregar intervalo al historial
            beatIntervals[beatIntervalIndex] = beatInterval;
            beatIntervalIndex = (beatIntervalIndex + 1) % BPM_HISTORY_SIZE;
            
            // Calcular BPM promedio
            float averageInterval = 0f;
            int validIntervals = 0;
            
            for (int i = 0; i < BPM_HISTORY_SIZE; i++)
            {
                if (beatIntervals[i] > 0)
                {
                    averageInterval += beatIntervals[i];
                    validIntervals++;
                }
            }
            
            if (validIntervals > 0)
            {
                averageInterval /= validIntervals;
                int newBPM = (int)(60f / averageInterval);
                
                // Filtrar BPM válidos (30-200 BPM)
                if (newBPM >= 30 && newBPM <= 200 && Math.Abs(newBPM - currentBPM) > 5)
                {
                    currentBPM = newBPM;
                    BpmChanged?.Invoke(this, currentBPM);
                }
            }
        }

        public void Reset()
        {
            lock (lockObj)
            {
                Array.Clear(energyHistory, 0, energyHistory.Length);
                Array.Clear(beatIntervals, 0, beatIntervals.Length);
                energyHistoryIndex = 0;
                beatIntervalIndex = 0;
                currentBPM = 0;
                lastBeatTime = 0;
                IsBeat = false;
                BeatStrength = 0f;
            }
        }
    }
}