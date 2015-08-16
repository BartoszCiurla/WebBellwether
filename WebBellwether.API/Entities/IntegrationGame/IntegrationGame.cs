using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WebBellwether.API.Entities.IntegrationGame
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