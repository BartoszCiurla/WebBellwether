using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WebBellwether.API.Entities.Joke
{
    public class JokeCategory
    {
        public JokeCategory()
        {
            JokeCategoryDetail = new Collection<JokeCategoryDetail>();
        }
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<JokeCategoryDetail> JokeCategoryDetail { get; set; }

    }
}
