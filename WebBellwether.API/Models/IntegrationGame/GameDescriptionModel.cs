using System.Collections.Generic;

namespace WebBellwether.API.Models.IntegrationGame
{
    public class GameDescriptionModel
    {
        public GameDescriptionModel()
        {
            GameCategories = new List<GameCategoryModel>();
            NumberOfPlayers = new List<NumberOfPlayerModel>();
            PaceOfPlays = new List<PaceOfPlayModel>();
            PreparationFuns = new List<PreparationFunModel>();
        }
        public List<GameCategoryModel> GameCategories { get; set; }
        public List<NumberOfPlayerModel> NumberOfPlayers { get; set; }
        public List<PaceOfPlayModel> PaceOfPlays { get; set; }
        public List<PreparationFunModel> PreparationFuns { get; set; }
    }

    public class GameCategoryModel
    {
        public int Id { get; set; }
        public string GameCategoryName { get; set; }
        public string GameCategoryTemplateName { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
    }

    public class NumberOfPlayerModel
    {
        public int Id { get; set; }
        public string NumberOfPlayerName { get; set; }
        public string NumberOfPlayerTemplateName { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
    }

    public class PaceOfPlayModel
    {
        public int Id { get; set; }
        public string PaceOfPlayName { get; set; }
        public string PaceOfPlayTemplateName { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
    }

    public class PreparationFunModel
    {
        public int Id { get; set; }
        public string PreparationFunName { get; set; }
        public string PreparationFunTemplateName { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
    }
}