using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBellwether.Models.ViewModels.IntegrationGame
{
    public class GameFeatureViewModel
    {
        public GameFeatureViewModel()
        {
            GameFeatureDetailModels = new List<GameFeatureDetailViewModel>();
        }
        [Required]
        public int Id { get; set; }//robi jako gamefeatureid czyli ten glowny 
        public string GameFeatureTemplateName { get; set; } // jako template zawsze angielski na twardo to zostanie przypisane 
        [Required]
        public string GameFeatureName { get; set; }
        [Required]
        public int LanguageId { get; set; }
        public List<GameFeatureDetailViewModel> GameFeatureDetailModels { get; set; } 
    }
}