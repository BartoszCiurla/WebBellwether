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
            IntegrationGameDetails = new Collection<IntegrationGameDetail>();
        }
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual ICollection<IntegrationGameDetail> IntegrationGameDetails { get; set; } 
    }
}