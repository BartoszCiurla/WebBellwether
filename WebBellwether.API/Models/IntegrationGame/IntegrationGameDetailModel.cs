using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBellwether.API.Models.IntegrationGame
{
    public class IntegrationGameDetailModel
    {
        public int Id { get; set; }// id for featuredetail
        public int GameFeatureId { get; set; }
        public string GameFeatureName { get; set; }
        public string GameFeatureDetailName { get; set; }

    }
}