using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reproducto_Musica
{
    public partial class MainForm : Form
    {

        private Timer tmr_Reproductor;
        private AudioPlayer audioPlayer;
        private PlaylistManager playlistManager;
        private bool isPlaying = false;
        private bool isUserScrolling = false;
        public MainForm()
        {
            audioPlayer = new AudioPlayer();
            playlistManager = new PlaylistManager();
            InitializeComponent();
            
            

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            tm_Tiempo_Musica.Interval = 500;
            tm_Tiempo_Musica.Tick += tm_Tiempo_Musica_Tick;
            tm_Tiempo_Musica.Start();
            hscrb_Progreso.LargeChange = 1;
            hscrb_Progreso.Minimum = 0;
        }
        private void tm_Tiempo_Musica_Tick(object sender, EventArgs e)
        {
            if (isPlaying && !isUserScrolling)
            {
                double current = audioPlayer.GetCurrentTime();
                double total = audioPlayer.GetTotalTime();

                if (total > 0)
                {
                    hscrb_Progreso.Maximum = (int)total;
                    hscrb_Progreso.Value = (int)Math.Min(current, total);
                }

                TimeSpan currentTime = TimeSpan.FromSeconds(current);
                TimeSpan totalTime = TimeSpan.FromSeconds(total);
                lbl_Tiempo.Text = $"{currentTime:mm\\:ss} / {totalTime:mm\\:ss}";
            }
        }
        private void hscrb_Progreso_Scroll(object sender, ScrollEventArgs e)
        {
            isUserScrolling = true;
            lbl_Tiempo.Text = $"{TimeSpan.FromSeconds(hscrb_Progreso.Value):mm\\:ss}";
            double newPosition = hscrb_Progreso.Value;
            audioPlayer.SetPosition(newPosition);
            isUserScrolling = false;
        }
        private void btn_CargarMusica_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Archivos de audio|*.mp3;*.wav";
                ofd.Multiselect = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (var file in ofd.FileNames)
                    {
                        playlistManager.AddSong(file);
                        var item = new ListViewItem(System.IO.Path.GetFileName(file));
                        item.Tag = file;
                        lstw_Canciones.Items.Add(item);
                    }

                    if (lstw_Canciones.Items.Count > 0)
                    {

                        lstw_Canciones.Items[0].Selected = true;
                        lstw_Canciones.Items[0].Focused = true;
                        playlistManager.SetCurrentIndex(0);
                    }
                }
            }
        }

        private void btn_Anterior_Click(object sender, EventArgs e)
        {

        }

        private void btn_Parar_Click(object sender, EventArgs e)
        {

        }

        private void btn_Play_Click(object sender, EventArgs e)
        {
            if (lstw_Canciones.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selecciona una canción.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string currentSong = playlistManager.GetCurrentSong();

            int selectedIndex = -1;
            if (lstw_Canciones.SelectedIndices.Count > 0)
                selectedIndex = lstw_Canciones.SelectedIndices[0];
            if (currentSong == null && selectedIndex >= 0)
            {
                currentSong = playlistManager.GetPlaylist()[selectedIndex];
                playlistManager.SetCurrentIndex(selectedIndex);
            }
            if (currentSong == null)
            {
                MessageBox.Show("No hay ninguna canción seleccionada.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!isPlaying)
            {
                audioPlayer.Play(currentSong);
                lbl_Nom_Cancion.Text = System.IO.Path.GetFileName(currentSong);
                btn_Play.Text = "⏸️ Pausar";
                isPlaying = true;
            }
            else
            {
                audioPlayer.Pause();
                btn_Play.Text = "▶️ Reanudar";
                isPlaying = false;
            }
        }

        private void btn_Siguiente_Click(object sender, EventArgs e)
        {

        }

        private void lbl_Tiempo_Click(object sender, EventArgs e)
        {

        }

        private void lbl_Nom_Cancion_Click(object sender, EventArgs e)
        {

        }

        private void lstw_Canciones_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

     

       
    }
}
