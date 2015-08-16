﻿using System.ComponentModel.DataAnnotations;

namespace WebBellwether.API.Models.IntegrationGame
{
    public class GameFeatureModel
    {
        [Required]
        public int Id { get; set; }//robi jako gamefeatureid czyli ten glowny 
        public string GameFeatureTemplateName { get; set; } // jako template zawsze angielski na twardo to zostanie przypisane 
        [Required]
        public string GameFeatureName { get; set; }
        [Required]
        public int LanguageId { get; set; }
    }
}