using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBellwether.API.Entities.IntegrationGames
{
    public class PreparationFunLanguage
    {
        public int Id { get; set; }
        public string PreparationFunName { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
        public virtual PreparationFun PreparationFun { get; set; }
    }
}