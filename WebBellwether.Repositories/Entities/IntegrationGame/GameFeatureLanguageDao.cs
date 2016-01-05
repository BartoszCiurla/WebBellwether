using System.ComponentModel.DataAnnotations.Schema;
using WebBellwether.Repositories.Entities.Translations;

namespace WebBellwether.Repositories.Entities.IntegrationGame
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