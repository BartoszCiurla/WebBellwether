using System.ComponentModel.DataAnnotations.Schema;
using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Entities.IntegrationGame
{
    [Table("GameFeatureDetailLanguage")]
    public class GameFeatureDetailLanguageDao
    {
        public int Id { get; set; }
        public virtual GameFeatureDetailDao GameFeatureDetail { get; set; }
        public string GameFeatureDetailName { get; set; }
        public virtual LanguageDao Language { get; set; }
    }
}