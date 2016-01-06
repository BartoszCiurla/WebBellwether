using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBellwether.Models.Models.IntegrationGame
{
    public class GameFeatureModel
    {
        public GameFeatureModel()
        {
            GameFeatureDetailModels = new List<GameFeatureDetailModel>();
        }
        [Required]
        public int Id { get; set; }//robi jako gamefeatureid czyli ten glowny 
        public string GameFeatureTemplateName { get; set; } // jako template zawsze angielski na twardo to zostanie przypisane 
        [Required]
        public string GameFeatureName { get; set; }
        [Required]
        public int LanguageId { get; set; }
        public List<GameFeatureDetailModel> GameFeatureDetailModels { get; set; } 
    }
}