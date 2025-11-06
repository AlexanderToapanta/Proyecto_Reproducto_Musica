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
        private AudioPlayer audioPlayer;
        private PlaylistManager playlistManager;
        private bool isPlaying = false;
        public MainForm()
        {
            audioPlayer = new AudioPlayer();
            playlistManager = new PlaylistManager();
            InitializeComponent();
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

        private void prb_Progreso_Click(object sender, EventArgs e)
        {

        }
    }
}
