using ArcadeLauncher.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ArcadeLauncher.Services
{
    public class GameService
    {
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private IntPtr handle;
        public IWebHostEnvironment env;
        public GamesData gamesData;
        private Process? currentProcess;
        private Action? CloseCallback;


        public GameService(IWebHostEnvironment environment, IServiceProvider provider)
        {
            env = environment;
            this.gamesData = provider.CreateScope().ServiceProvider.GetRequiredService<GamesData>();

        }

        public string[] GetAllFolders()
        {
            string folderPath = Path.Combine(env.WebRootPath, "Executes");
            string[] foldersFound = Directory.GetDirectories(folderPath);
            return foldersFound;
        }
        public GameInfo[] GetAllManifests()
        {
            List<GameInfo> infos = new List<GameInfo>();

            foreach (var game in gamesData.GameInfo)
            {
                string manifestpath = Path.Combine(game.GamePath, "manifest.json");

                if (!File.Exists(manifestpath))
                    continue;

                game.Manifest = JsonConvert.DeserializeObject<GameManifest>(File.ReadAllText(manifestpath));

                infos.Add(game);
            }

            return infos.ToArray();
        }

        public bool OpenExe(GameInfo gameFolder, Action CloseCallback)
        {
            if (currentProcess is not null && !currentProcess.HasExited)
                return false;

            Process[] processList = Process.GetProcessesByName(gameFolder.Manifest.NameExe);
            if (processList.Length == 0)
            {
                this.CloseCallback = CloseCallback;
                currentProcess = Process.Start(Path.Combine(gameFolder.GamePath, gameFolder.Manifest.NameExe + ".exe"));
                currentProcess.Exited += (x,e) => CloseGame();
                handle = currentProcess.MainWindowHandle;
                //SetForegroundWindow(handle);
                return true;
            }
            return false;


        }

        public string ShowImage(GameInfo gameFolder)
        {
            var iconpath = Path.Combine(gameFolder.GamePath, "icon.png");
            if (!File.Exists(iconpath))
            {
                return string.Empty;
            }
            byte[] imageArray = System.IO.File.ReadAllBytes(iconpath);
            string base64Image = Convert.ToBase64String(imageArray);
            return base64Image;
        }

        internal void CloseGame()
        {

            currentProcess?.Kill();
            currentProcess = null;
            CloseCallback?.Invoke();
            CloseCallback = null;
        }
    }
}
