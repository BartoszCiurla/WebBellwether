using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace WebBellwether.API.Entities.IntegrationGames
{
    public class IntegrationGame
    {
        public IntegrationGame()
        {
            IntegrationGameLanguages = new Collection<IntegrationGameLanguage>();
        }
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual ICollection<IntegrationGameLanguage> IntegrationGameLanguages { get; set; } 
    }
}