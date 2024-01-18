namespace ArcadeLauncher.Models
{
    public class GameManifest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Authors { get; set; }
        public string NameExe { get; set; }
        public string BackgroundColor { get; set; }

        public int PlayersNeeded { get; set; }
        public int ManifestVersion { get; set; } = 1;
    }
}
