using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Entities.IntegrationGames
{
    public class PaceOfPlay
    {
        public PaceOfPlay()
        {
            PaceOfPlayLanguages = new Collection<PaceOfPlayLanguage>();
        }
        public int Id { get; set; }
        
        public virtual ICollection<PaceOfPlayLanguage> PaceOfPlayLanguages { get; set; } 
    }
}