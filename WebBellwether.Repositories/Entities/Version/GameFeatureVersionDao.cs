using System.ComponentModel.DataAnnotations.Schema;
using WebBellwether.Repositories.Entities.Translations;

namespace WebBellwether.Repositories.Entities.Version
{
    [Table("GameFeatureVersion")]
    public class GameFeatureVersionDao
    {
        public int Id { get; set; }
        public virtual LanguageDao Language { get; set; }
        public double Version { get; set; }
    }
}
