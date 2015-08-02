using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBellwether.API.Entities.Translations;

namespace WebBellwether.API.Models
{
    public class IntegrationGameModel
    {
        public int Id { get; set; }
        public string GameName { get; set; }
        public string GameDetails { get; set; }
        public int GameCategoryId { get; set; }
        public string GameCategory { get; set; }
        public int PaceOfPlayId { get; set; }
        public string PaceOfPlay { get; set; }
        public int NumberOfPlayerId { get; set; }
        public string NumberOfPlayer { get; set; }
        public int PreparationFunId { get; set; }
        public string PreparationFun { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
    }
}