using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBellwether.Repositories.Entities.IntegrationGame
{
    [Table("GameFeatureDetail")]
    public class GameFeatureDetailDao
    {
        public GameFeatureDetailDao()
        {
            GameFeatureDetailLanguages = new Collection<GameFeatureDetailLanguageDao>();
        }
        public int Id { get; set; }
        public virtual ICollection<GameFeatureDetailLanguageDao> GameFeatureDetailLanguages { get; set; }
        public virtual GameFeatureDao GameFeature { get; set; }
    }
}