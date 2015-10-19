using System.Collections.Generic;
using WebBellwether.API.Entities.Translations;
namespace WebBellwether.API.Entities.Joke
{
    public class JokeCategoryDetail
    {
        public int Id { get; set; }
        public virtual Language Language { get; set; }
        public virtual JokeCategory JokeCategory { get; set; }
        public string JokeCategoryName { get; set; }
    }
}
