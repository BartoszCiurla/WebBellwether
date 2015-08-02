using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Entities.IntegrationGames
{
    public class GameCategoryLanguage
    {
        public int Id { get; set; }
        public string GameCategoryName { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
        public virtual GameCategory GameCategory { get; set; }
    }
}