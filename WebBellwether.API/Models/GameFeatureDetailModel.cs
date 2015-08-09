using System;
using System.Collections.Generic;

namespace WebBellwether.API.Models
{
    public class GameFeatureDetailModel
    {
        public int Id { get; set; }//gamefeature id 
        public int GameFeatureDetailId { get; set; }//detail id 
        public string GameFeatureDetailName { get; set; }
        public string GameFeatureDetailTemplateName { get; set; }
        public string Language { get; set; }
        public int LanguageId { get; set; }
    }
}