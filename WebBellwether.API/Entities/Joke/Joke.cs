using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WebBellwether.API.Entities.Joke
{
    public class Joke
    {
        public Joke()
        {
            JokeDetails = new Collection<JokeDetail>();
        }
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual ICollection<JokeDetail> JokeDetails { get; set; }
    }
}