using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;
using System.Drawing;
using System.IO;


namespace Reproducto_Musica
{
    internal class PlaylistManager
    {
        private List<string> playlist = new List<string>();
        public int CurrentIndex { get; private set; } = -1;

        public void AddSong(string filePath)
        {
            playlist.Add(filePath);
            if (CurrentIndex == -1)
                CurrentIndex = 0;
        }
        public Image GetCoverImage(string filePath)
        {
            try
            {
                var file = TagLib.File.Create(filePath);
                if (file.Tag.Pictures != null && file.Tag.Pictures.Length > 0)
                {
                    var bin = (byte[])(file.Tag.Pictures[0].Data.Data);
                    using (var ms = new MemoryStream(bin))
                    {
                        return Image.FromStream(ms);
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public void SetCurrentIndex(int index)
        {
            if (index >= 0 && index < playlist.Count)
                CurrentIndex = index;
        }

        public string GetCurrentSong()
        {
            if (CurrentIndex >= 0 && CurrentIndex < playlist.Count)
                return playlist[CurrentIndex];
            return null;
        }

        public string NextSong()
        {
            if (playlist.Count == 0) return null;
            CurrentIndex = (CurrentIndex + 1) % playlist.Count;
            return playlist[CurrentIndex];
        }

        public string PreviousSong()
        {
            if (playlist.Count == 0) return null;
            CurrentIndex = (CurrentIndex - 1 + playlist.Count) % playlist.Count;
            return playlist[CurrentIndex];
        }

        public List<string> GetPlaylist() => playlist;

    }
}
