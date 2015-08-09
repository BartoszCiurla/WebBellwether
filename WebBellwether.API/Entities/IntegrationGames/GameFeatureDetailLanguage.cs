using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBellwether.API.Entities.IntegrationGames
{
    public class GameFeatureDetailLanguage
    {
        public int Id { get; set; }
        public virtual GameFeatureDetail GameFeatureDetail { get; set; }
        public string GameFeatureDetailName { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
    }
}