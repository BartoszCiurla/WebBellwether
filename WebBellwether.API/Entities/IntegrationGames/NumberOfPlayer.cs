using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Entities.IntegrationGames
{
    public class NumberOfPlayer
    {
        public NumberOfPlayer()
        {
            NumberOfPlayerLanguages = new Collection<NumberOfPlayerLanguage>();
        }
        public int Id { get; set; }
        public virtual ICollection<NumberOfPlayerLanguage> NumberOfPlayerLanguages { get; set; }
        
    }
}