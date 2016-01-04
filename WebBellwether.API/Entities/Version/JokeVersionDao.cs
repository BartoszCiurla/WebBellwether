using System.ComponentModel.DataAnnotations.Schema;
using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Entities.Version
{
    [Table("JokeVersion")]
    public class JokeVersionDao
    {
        public int Id { get; set; }
        public virtual LanguageDao Language { get; set; }
        public double Version { get; set; }
        public int NumberOfJokes { get; set; }
    }
}