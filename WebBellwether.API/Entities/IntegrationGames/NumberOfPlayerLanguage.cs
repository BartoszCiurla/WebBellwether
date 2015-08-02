using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace WebBellwether.API.Entities.IntegrationGames
{
    public class NumberOfPlayerLanguage
    {
        public int Id { get; set; }
        public string NumberOfPlayerName { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
        public virtual NumberOfPlayer NumberOfPlayer { get; set; }

    }
}