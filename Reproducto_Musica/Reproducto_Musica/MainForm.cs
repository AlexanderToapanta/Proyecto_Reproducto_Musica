using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

        private VisualizerControl visualizer;
        public MainForm()
        {
            audioPlayer = new AudioPlayer();
            playlistManager = new PlaylistManager();
            InitializeComponent();

            visualizer = new VisualizerControl();
            visualizer.Location = new Point(115, 46);
            visualizer.Size = new Size(616, 200);
            visualizer.BackColor = Color.Black;
            visualizer.Dock = DockStyle.Top;
            visualizer.Height = 220;
            this.Controls.Add(visualizer);

            audioPlayer.SamplesAvailable += visualizer.AddSamples;

            // initialize controls
            cmb_VisualMode.SelectedIndex = 0;
            trk_Volumen.Value = 100;
            audioPlayer.Volume = 1f;

            cmb_VisualMode.SelectedIndexChanged += (s, e) =>
            {
                if (cmb_VisualMode.SelectedItem == null) return;
                string m = cmb_VisualMode.SelectedItem.ToString();
                switch (m)
                {
                    case "Barras": visualizer.Mode = VisualMode.Barras; break;
                    case "Onda": visualizer.Mode = VisualMode.Onda; break;
                    default: visualizer.Mode = VisualMode.Barras; break;
                }
            };


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

                    if (lstw_Canciones.LargeImageList == null)
                    {
                        lstw_Canciones.LargeImageList = new ImageList();
                        lstw_Canciones.LargeImageList.ImageSize = new Size(64, 64);
                    }


                    lstw_Canciones.Items.Clear();
                    lstw_Canciones.LargeImageList.Images.Clear();

                    foreach (var file in ofd.FileNames)
                    {
                        playlistManager.AddSong(file);


                        Image cover = playlistManager.GetCoverImage(file);


                        if (cover == null)
                        {
                            string defaultCoverPath = Path.Combine(Application.StartupPath, "Resources", "cover.png");
                            if (File.Exists(defaultCoverPath))
                                cover = Image.FromFile(defaultCoverPath);
                            else
                                cover = new Bitmap(64, 64);
                        }


                        lstw_Canciones.LargeImageList.Images.Add(cover);


                        var item = new ListViewItem(Path.GetFileName(file), lstw_Canciones.LargeImageList.Images.Count - 1);
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
            string prev = playlistManager.PreviousSong();
            if (prev != null)
            {

                lstw_Canciones.Items[playlistManager.CurrentIndex].Selected = true;
                lstw_Canciones.EnsureVisible(playlistManager.CurrentIndex);

                audioPlayer.Play(prev);
                lbl_Nom_Cancion.Text = System.IO.Path.GetFileName(prev);
                btn_Play.Text = "⏸️ Pausar";
                isPlaying = true;
            }
        }

        private void btn_Parar_Click(object sender, EventArgs e)
        {
            audioPlayer.Stop();
            isPlaying = false;
            btn_Play.Text = "▶️";
            lbl_Tiempo.Text = "00:00 / 00:00";
            hscrb_Progreso.Value = 0;
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
            string next = playlistManager.NextSong();
            if (next != null)
            {
                lstw_Canciones.Items[playlistManager.CurrentIndex].Selected = true;
                lstw_Canciones.EnsureVisible(playlistManager.CurrentIndex);

                audioPlayer.Play(next);
                lbl_Nom_Cancion.Text = System.IO.Path.GetFileName(next);
                btn_Play.Text = "⏸️ Pausar";
                isPlaying = true;
            }
        }

        private void lstw_Canciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstw_Canciones.SelectedItems.Count > 0)
            {
                int index = lstw_Canciones.SelectedItems[0].Index;
                playlistManager.SetCurrentIndex(index);

                string selectedSong = playlistManager.GetCurrentSong();
                if (selectedSong != null)
                {
                    audioPlayer.Play(selectedSong);
                    lbl_Nom_Cancion.Text = Path.GetFileName(selectedSong);
                    btn_Play.Text = "⏸️ Pausar";
                    isPlaying = true;
                }
            }
        }

        private void trk_Volumen_Scroll(object sender, EventArgs e)
        {
            float vol = trk_Volumen.Value / 100f;
            audioPlayer.Volume = vol;
        }

        private void cmb_VisualMode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
