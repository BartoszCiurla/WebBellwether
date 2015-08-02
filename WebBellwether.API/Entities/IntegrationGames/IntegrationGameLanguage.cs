using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Entities.IntegrationGames
{
    public class IntegrationGameLanguage
    {
        public int Id { get; set; }
        public string GameName { get; set; }
        public string GameDetails { get; set; }

        public int GameCategoryId { get; set; }
        public string GameCategory { get; set; }
        //    Integration,
        //Concentration,
        //Creativity,
        //GoneToGroups,
        //LetUsLearnTo,
        //TeamWork,
        //DischargeEnergy,
        //Competition,
        //Dexterity

        public int PaceOfPlayId { get; set; }
        public string PaceOfPlay { get; set; }
        //Dynamic,
        //Static,
        //Living

        public int NumberOfPlayerId { get; set; }
        public string NumberOfPlayer { get; set; }
        // Optional,
        //MoreThan20

        public int PreparationFunId { get; set; }
        public string PreparationFun { get; set; }
        //Lack,Minimum,Required
        public int LanguageId { get; set; }
        public string Language { get; set; }
        public virtual IntegrationGame IntegrationGame { get; set; }




    }
}