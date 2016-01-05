using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBellwether.Repositories.Entities.Joke
{
    [Table("JokeCategory")]
    public class JokeCategoryDao
    {
        public JokeCategoryDao()
        {
            JokeCategoryDetail = new Collection<JokeCategoryDetailDao>();
        }
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<JokeCategoryDetailDao> JokeCategoryDetail { get; set; }

    }
}
