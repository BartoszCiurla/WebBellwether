using System.ComponentModel.DataAnnotations.Schema;

namespace WebBellwether.Repositories.Entities.IntegrationGame
{
    [Table("IntegrationGameFeature")]
    public class IntegrationGameFeatureDao
    {
        public int Id { get; set; }
        public virtual IntegrationGameDetailDao IntegrationGameDetail { get; set; }
        public virtual GameFeatureLanguageDao GameFeatureLanguage { get; set; }
        public virtual GameFeatureDetailLanguageDao GameFeatureDetailLanguage { get; set; }
    }
}