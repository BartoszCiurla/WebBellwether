using System.ComponentModel.DataAnnotations;
using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Models.IntegrationGame
{
    public class NewIntegrationGameModel
    {
        public int Id { get; set; }
        [Required]
        public string GameName { get; set; }
        [Required]
        public string GameDetails { get; set; }
        [Required]
        public Language Language { get; set; }
        public int[] Features { get; set; }
    }

}