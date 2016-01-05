using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBellwether.Repositories.Entities.IntegrationGame
{
    [Table("IntegrationGame")]
    public class IntegrationGameDao
    {
        public IntegrationGameDao()
        {
            IntegrationGameDetails = new Collection<IntegrationGameDetailDao>();
        }
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual ICollection<IntegrationGameDetailDao> IntegrationGameDetails { get; set; } 
    }
}