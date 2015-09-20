using System.ComponentModel.DataAnnotations;

namespace WebBellwether.API.Models.IntegrationGame
{
    public class GameFeatureDetailModel
    {
        [Required]
        public int Id { get; set; }//gamefeaturedetaillanguageid
        [Required]
        public int GameFeatureDetailId { get; set; }//detail id 
        [Required]
        public int GameFeatureId { get; set; }
        [Required]
        public string GameFeatureDetailName { get; set; }
        public string GameFeatureDetailTemplateName { get; set; }
        [Required]
        public string Language { get; set; }
        [Required]
        public int LanguageId { get; set; }
        public bool IsSelected { get; set; } //i use this in edit game ,and new game 
    }
}