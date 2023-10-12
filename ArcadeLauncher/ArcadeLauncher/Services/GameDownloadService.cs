using Microsoft.Extensions.Logging;
using System.IO;

namespace ArcadeLauncher.Services
{
    public class GameDownloadService : BackgroundService
    {

        private PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(3));
        private ILogger<GameDownloadService> Log;
        private List<string> SearchedDrives;
        private string folderPath;
        public GameDownloadService(ILogger<GameDownloadService> logger)
        {
            Log = logger;
            SearchedDrives = new();
            folderPath = Path.Combine(Environment.GetFolderPath(
  Environment.SpecialFolder.ApplicationData), "GluArcadeLauncher");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
            {
                await CheckForDrive(stoppingToken);
            }
        }

        private async Task CheckForDrive(CancellationToken stoppingToken)
        {
            foreach (var drive in DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Removable && d.IsReady && !SearchedDrives.Contains(d.VolumeLabel)))
            {

                ScanDrive(drive, stoppingToken);
            }

            foreach (string SearchedDrive in SearchedDrives.ToArray())
            {
                if (!DriveInfo.GetDrives().Select(x => x.VolumeLabel).Contains(SearchedDrive))
                    SearchedDrives.Remove(SearchedDrive);
            }
        }

        private void ScanDrive(DriveInfo drive, CancellationToken stoppingToken)
        {
            Log.LogInformation($"scanning {drive.VolumeLabel} at {drive.Name}");

            SearchedDrives.Add(drive.VolumeLabel);
            foreach (DirectoryInfo directory in drive.RootDirectory.EnumerateDirectories())
            {
                var possibleExistingManifest = Path.Combine(folderPath, directory.Name, "Manifest.json");
                var possibleNewManifest = Path.Combine(directory.ToString(), "Manifest.json");
                if (!File.Exists(possibleNewManifest))
                    return;
                
                Log.LogDebug($"manifest found at {possibleNewManifest}");
                if (!File.Exists(possibleExistingManifest))
                {
                    Log.LogDebug($"manifest  already copied at {possibleExistingManifest}");

                }

                Log.LogInformation($"directory {directory.FullName} found with a manifest, starting to copy to local folder");
                CopyDirectory(directory, folderPath);
            }
            Log.LogInformation($"scan complete {drive.VolumeLabel} at {drive.Name}");

        }


        static void CopyDirectory(DirectoryInfo sourceDir, string destinationDir)
        {
            // Get information about the source directory

            // Check if the source directory exists
            if (!sourceDir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {sourceDir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = sourceDir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in sourceDir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                CopyDirectory(subDir, newDestinationDir);
            }
        }
    }
}
