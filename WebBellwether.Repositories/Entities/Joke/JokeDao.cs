using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBellwether.Repositories.Entities.Joke
{
    [Table("Joke")]
    public class JokeDao
    {
        public JokeDao()
        {
            JokeDetails = new Collection<JokeDetailDao>();
        }
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual ICollection<JokeDetailDao> JokeDetails { get; set; }
    }
}