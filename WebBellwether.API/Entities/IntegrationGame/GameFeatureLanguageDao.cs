using System.ComponentModel.DataAnnotations.Schema;
using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Entities.IntegrationGame
{
    [Table("GameFeatureLanguage")]
    public class GameFeatureLanguageDao
    {
        public int Id { get; set; }
        public string GameFeatureName { get; set; }
        public LanguageDao Language { get; set; }
        public virtual GameFeatureDao GameFeature { get; set; }
    }
}