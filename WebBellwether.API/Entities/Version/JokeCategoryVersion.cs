using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Entities.Version
{
    public class JokeCategoryVersion
    {
        public int Id { get; set; }
        public virtual Language Language { get; set; }
        public double Version { get; set; }
        public int NumberOfJokeCategory { get; set; }
    }
}
