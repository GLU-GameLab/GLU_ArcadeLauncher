using System.Collections;

namespace ArcadeLauncher.Models
{
    public class Category
    {
        public string Name { get; set; }
        public IEnumerable<GameInfo> Selector { get; set; }
    }
}
