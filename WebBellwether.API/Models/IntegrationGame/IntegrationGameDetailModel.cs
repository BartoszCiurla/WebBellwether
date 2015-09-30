using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBellwether.API.Models.IntegrationGame
{
    public class IntegrationGameDetailModel
    {
        public int Id { get; set; }// id for featuredetail
        public int GameFeatureId { get; set; } // game feature id 
        public int GameFeatureLanguageId { get; set; }
        public int GameFeatureDetailId { get; set; } // game feature detail id 
        public string GameFeatureName { get; set; }
        public string GameFeatureDetailName { get; set; }

    }
}