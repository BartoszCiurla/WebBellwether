using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Entities.IntegrationGames
{
    public class GameCategory
    {
        public GameCategory()
        {
            GameCategoryLanguages = new Collection<GameCategoryLanguage>();
        }
        public int Id { get; set; }
        public virtual ICollection<GameCategoryLanguage> GameCategoryLanguages { get; set; }
        
    }
}