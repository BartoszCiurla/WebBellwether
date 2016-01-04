using System.ComponentModel.DataAnnotations.Schema;
using WebBellwether.API.Entities.Translations;
namespace WebBellwether.API.Entities.Joke
{
    [Table("JokeCategoryDetail")]
    public class JokeCategoryDetailDao
    {
        public int Id { get; set; }
        public virtual LanguageDao Language { get; set; }
        public virtual JokeCategoryDao JokeCategory { get; set; }
        public string JokeCategoryName { get; set; }
    }
}
