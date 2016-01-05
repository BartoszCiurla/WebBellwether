using System.ComponentModel.DataAnnotations.Schema;
using WebBellwether.Repositories.Entities.Translations;

namespace WebBellwether.Repositories.Entities.Joke
{
    [Table("JokeDetail")]
    public class JokeDetailDao
    {
        public int Id { get; set; }
        public virtual LanguageDao Language { get; set; }
        public virtual JokeDao Joke { get; set; }
        public string JokeContent { get; set; }
        public virtual JokeCategoryDetailDao JokeCategoryDetail { get; set; }
    }
}
