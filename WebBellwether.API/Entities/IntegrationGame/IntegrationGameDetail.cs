using System.Collections.Generic;
using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Entities.IntegrationGame
{
    public class IntegrationGameDetail
    {
        public int Id { get; set; }
        public virtual Language Language { get; set; }
        public virtual Entities.IntegrationGame.IntegrationGame IntegrationGame { get; set; }
        public string IntegrationGameName { get; set; }
        public string IntegrationGameDescription { get; set; }
        public virtual ICollection<IntegrationGameFeature> IntegrationGameFeatures { get; set; }
    }
}