using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    
namespace Reproducto_Musica
{
    internal class PlaylistManager
    {
        private List<string> playlist = new List<string>();
        private int currentIndex = -1;

        public void AddSong(string filePath)
        {
            playlist.Add(filePath);
            if (currentIndex == -1)
                currentIndex = 0;
        }

        public string GetCurrentSong()
        {
            if (currentIndex >= 0 && currentIndex < playlist.Count)
                return playlist[currentIndex];
            return null;
        }

        public string NextSong()
        {
            if (playlist.Count == 0) return null;
            currentIndex = (currentIndex + 1) % playlist.Count;
            return playlist[currentIndex];
        }

        public string PreviousSong()
        {
            if (playlist.Count == 0) return null;
            currentIndex = (currentIndex - 1 + playlist.Count) % playlist.Count;
            return playlist[currentIndex];
        }

        public List<string> GetPlaylist() => playlist;
    
}
}
