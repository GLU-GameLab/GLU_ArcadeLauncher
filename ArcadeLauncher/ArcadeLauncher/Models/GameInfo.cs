using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArcadeLauncher.Models
{
    public class GameInfo
    {
        [Key]
        public string GameName { get; set; }
        public bool isTopPicked { get; set; }
        public string GamePath { get; set; }
        public int PlayTime { get; set; }
        public DateTime LastPlayed {  get; set; }

        [NotMapped]
        public GameManifest Manifest { get; set; }
    }
}
