using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebBellwether.Repositories.Entities.Translations;

namespace WebBellwether.Repositories.Entities.IntegrationGame
{
    [Table("IntegrationGameDetail")]
    public class IntegrationGameDetailDao
    {
        public int Id { get; set; }
        public virtual LanguageDao Language { get; set; }
        public virtual IntegrationGameDao IntegrationGame { get; set; }
        public string IntegrationGameName { get; set; }
        public string IntegrationGameDescription { get; set; }
        public virtual ICollection<IntegrationGameFeatureDao> IntegrationGameFeatures { get; set; }
    }
}