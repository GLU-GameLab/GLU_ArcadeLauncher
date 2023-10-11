using ArcadeLauncher.Models;
using Newtonsoft.Json;
using System.Diagnostics;
namespace ArcadeLauncher.Services
{
    public class GameService
    {
        public IWebHostEnvironment env;
        public GameService(IWebHostEnvironment environment)
        {
            env = environment;
        }

        public string[] GetAllFolders()
        {
            string folderPath = Path.Combine(env.WebRootPath, "Executes");
            string[] foldersFound = Directory.GetDirectories(folderPath);
            return foldersFound;
        }
        public Game[] GetAllManifests()
        {
            var folders = GetAllFolders();
            List<Game> manifests = new List<Game>();

            for (int i = 0; i < folders.Length; i++)
            {
                string manifestpath = Path.Combine(folders[i], "Manifest.json");
                if (File.Exists(manifestpath))
                {
                    Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(manifestpath));
                    game.CompleteFolder = folders[i];
                    manifests.Add(game);
                }
            }
            return manifests.ToArray();
        }

        public void OpenExe(Game executable)
        {
            Process[] processList = Process.GetProcessesByName(executable.NameExe);
            if (processList.Length == 0)
                Process.Start(Path.Combine(executable.CompleteFolder, executable.NameExe + ".exe"));
        }

        public string ShowImage(Game gameFolder)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(Path.Combine(gameFolder.CompleteFolder, "icon.png"));
            string base64Image = Convert.ToBase64String(imageArray);
            return base64Image;
        }
    }
}
