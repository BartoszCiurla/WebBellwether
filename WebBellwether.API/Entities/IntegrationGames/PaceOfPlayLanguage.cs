using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBellwether.API.Entities.IntegrationGames
{
    public class PaceOfPlayLanguage
    {
        public int Id { get; set; }
        public string PaceOfPlayName { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
        public virtual PaceOfPlay PaceOfPlay { get; set; }

    }
}