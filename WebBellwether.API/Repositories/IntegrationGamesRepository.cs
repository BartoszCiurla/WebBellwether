using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using WebBellwether.API.Context;
using WebBellwether.API.Entities.IntegrationGames;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Models;
using WebBellwether.API.Repositories.Abstract;

namespace WebBellwether.API.Repositories
{
    public class IntegrationGamesRepository //: IRepository<Game>
    {
        private EfDbContext _ctx;
        public IntegrationGamesRepository(EfDbContext ctx)
        {
            _ctx = ctx;
        }

        public List<IntegrationGameModel> GetIntegrationGames(int language)
        {
            var games = new List<IntegrationGameModel>();
            _ctx.IntegrationGameLanguages.Where(x => x.LanguageId == language).ToList().ForEach(x=>games.Add(new IntegrationGameModel
            {
                //general id for game IMPORTANT not detail id
                Id = x.IntegrationGame.Id,
                GameName = x.GameName,
                GameDetails = x.GameDetails,
                GameCategoryId = x.GameCategoryId,
                GameCategory = x.GameCategory,
                PaceOfPlayId = x.PaceOfPlayId,
                PaceOfPlay = x.PaceOfPlay,
                NumberOfPlayerId = x.NumberOfPlayerId,
                NumberOfPlayer = x.NumberOfPlayer,
                PreparationFunId = x.PreparationFunId,
                PreparationFun = x.PreparationFun,
                LanguageId = x.LanguageId,
                Language = x.Language
            }));
            return games;

        }
        public ResultState InsertIntegrationGame(IntegrationGameModel game)
        {
            var entity = new IntegrationGame {CreationDate = DateTime.UtcNow};
            entity.IntegrationGameLanguages.Add(new IntegrationGameLanguage
            {
                GameName = game.GameName,
                GameDetails = game.GameDetails,
                GameCategory = game.GameCategory,
                GameCategoryId = game.GameCategoryId,
                NumberOfPlayer = game.NumberOfPlayer,
                NumberOfPlayerId = game.NumberOfPlayerId,
                PaceOfPlay = game.PaceOfPlay,
                PaceOfPlayId = game.PaceOfPlayId,
                PreparationFun = game.PreparationFun,
                PreparationFunId = game.PreparationFunId,
                Language = game.Language,
                LanguageId = game.LanguageId
            });
            _ctx.IntegrationGames.Add(entity);
            _ctx.SaveChanges();
            return ResultState.GameAdded;
        }

        public ResultState PutGameFeature(GameFeatureModel gameFeatureModel)
        {
            var entity =
                _ctx.GameFeatureLanguages.FirstOrDefault(
                    x => x.GameFeature.Id == gameFeatureModel.Id && x.LanguageId == gameFeatureModel.LanguageId);
            if (entity != null) entity.GameFeatureName = gameFeatureModel.GameFeatureName;
            _ctx.SaveChanges();
            return ResultState.GameFeatureEdited;
        }

        public List<GameFeatureModel> GetGameFeatures(int language)
        {
            //id for header , gamefeaturename for details
            var gameFeatures = new List<GameFeatureModel>();
            _ctx.GameFeatureLanguages.Where(x=>x.LanguageId == language).ToList().ForEach(x=> gameFeatures.Add(new GameFeatureModel{Id=x.GameFeature.Id,GameFeatureName = x.GameFeatureName,LanguageId = language}));
            //update in template language en 

            var checkIsExists = _ctx.Languages.FirstOrDefault(x => x.LanguageName == "English");
            if (checkIsExists != null)
            {
                int enId = checkIsExists.Id;
                if(enId != language) // then i build template features     
                _ctx.GameFeatureLanguages.Where(x=>x.LanguageId == enId).ToList().ForEach(x => gameFeatures.ForEach(z =>
                {
                    if (z.Id == x.GameFeature.Id)
                        z.GameFeatureTemplateName = x.GameFeatureName;
                }));
            }

            return gameFeatures;
        } 

        public GameDescriptionModel GetGameDescription(int language)
        {
            List<NumberOfPlayerLanguage> numberOfPlayers = _ctx.NumberOfPlayerLanguages.Where(x => x.LanguageId == language).ToList();
            List<GameCategoryLanguage> gameCategories = _ctx.GameCategoryLanguages.Where(x => x.LanguageId == language).ToList();
            List<PaceOfPlayLanguage> paceOfPlays = _ctx.PaceOfPlayLanguages.Where(x => x.LanguageId == language).ToList();
            List<PreparationFunLanguage> preparationFuns = _ctx.PreparationFunLanguages.Where(x => x.LanguageId == language).ToList();

            var gameDescription = new GameDescriptionModel();
            numberOfPlayers.ForEach(
                x => gameDescription.NumberOfPlayers.Add(
                    new NumberOfPlayerModel { Id = x.NumberOfPlayer.Id, Language = x.Language, LanguageId = x.LanguageId, NumberOfPlayerName = x.NumberOfPlayerName }));

            gameCategories.ForEach(
                x => gameDescription.GameCategories.Add(
                    new GameCategoryModel { Id = x.GameCategory.Id, Language = x.Language, LanguageId = x.LanguageId, GameCategoryName = x.GameCategoryName }));

            paceOfPlays.ForEach(
                x => gameDescription.PaceOfPlays.Add(
                    new PaceOfPlayModel { Id = x.PaceOfPlay.Id, Language = x.Language, LanguageId = x.LanguageId, PaceOfPlayName = x.PaceOfPlayName }));

            preparationFuns.ForEach(
                x => gameDescription.PreparationFuns.Add(
                    new PreparationFunModel { Id = x.PreparationFun.Id, Language = x.Language, LanguageId = x.LanguageId, PreparationFunName = x.PreparationFunName }));

            return gameDescription;
        }
    }
}