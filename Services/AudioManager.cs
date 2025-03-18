using System.Diagnostics;
using System.IO;
using System.Media;
using System.Windows.Media;

namespace AOL_Reborn.Audio
{
    public static class AudioManager
    {
        // Cache of preloaded SoundPlayer instances
        private static readonly Dictionary<string, SoundPlayer> _players = new Dictionary<string, SoundPlayer>();

        // Call this method during initialization for all your sound files
        public static void PreloadSound(string fileName)
        {
            try
            {
                var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Audio", fileName);
                Debug.WriteLine($"Preloading sound from: {fullPath}");
                var player = new SoundPlayer(fullPath);
                player.Load(); // Loads the sound into memory
                _players[fileName] = player;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error preloading sound '{fileName}': {ex.Message}");
            }
        }

        public static void PlaySound(string fileName)
        {
            try
            {
                var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Audio", fileName);
                Debug.WriteLine($"Playing MP3 from: {fullPath}");
                var player = new MediaPlayer();
                player.Open(new Uri(fullPath, UriKind.Absolute));
                player.Volume = 1.0;
                player.Play();

                // Optionally, you might want to handle MediaEnded event to close the player:
                player.MediaEnded += (s, e) => player.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error playing MP3: {ex.Message}");
            }
        }
    }

}

