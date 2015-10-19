using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Entities.Joke
{
    public class JokeDetail
    {
        public int Id { get; set; }
        public virtual Language Language { get; set; }
        public virtual Joke Joke { get; set; }
        public string JokeContent { get; set; }
        public virtual JokeCategoryDetail JokeCategoryDetail { get; set; }
    }
}
