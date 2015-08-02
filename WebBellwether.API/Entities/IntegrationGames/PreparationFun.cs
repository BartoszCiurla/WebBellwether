using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Entities.IntegrationGames
{
    public class PreparationFun
    {
        public PreparationFun()
        {
            PreparationFunLanguages = new Collection<PreparationFunLanguage>();
        }
        public int Id { get; set; }
        public virtual ICollection<PreparationFunLanguage> PreparationFunLanguages { get; set; }


    }
}