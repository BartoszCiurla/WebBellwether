using System.ComponentModel.DataAnnotations;

namespace WebBellwether.Models.ViewModels.IntegrationGame
{
    public class NewIntegrationGameViewModel
    {
        public int Id { get; set; }
        public int IntegrationGameId { get; set; }//this is id for translation not game fss 
        [Required]
        public string GameName { get; set; }
        [Required]
        public string GameDetails { get; set; }
        [Required]
        public int Language { get; set; }
        public int[] Features { get; set; }
    }

}