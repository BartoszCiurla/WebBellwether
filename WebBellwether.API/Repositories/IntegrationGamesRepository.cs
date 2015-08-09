using System;
using System.Collections.Generic;
using System.Linq;
using WebBellwether.API.Context;
using WebBellwether.API.Entities.IntegrationGames;
using WebBellwether.API.Models;

namespace WebBellwether.API.Repositories
{
    public class IntegrationGamesRepository //: IRepository<Game>
    {
        private readonly EfDbContext _ctx;
        public IntegrationGamesRepository(EfDbContext ctx)
        {
            _ctx = ctx;
        }

        public List<IntegrationGameModel> GetIntegrationGames(int language)
        {
            var games = new List<IntegrationGameModel>();
            _ctx.IntegrationGameLanguages.Where(x => x.LanguageId == language).ToList().ForEach(x => games.Add(new IntegrationGameModel
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
            var entity = new IntegrationGame { CreationDate = DateTime.UtcNow };
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

        public ResultState PutGameFeatureDetail(GameFeatureDetailModel gameFeatureDetailModel)
        {
            ResultState result = ResultState.GameFeatureDetailEdited;



            return result;
        }



        public List<GameFeatureModel> GetGameFeatures(int language)
        {
            //id for header , gamefeaturename for details
            var gameFeatures = new List<GameFeatureModel>();
            _ctx.GameFeatureLanguages.Where(x => x.LanguageId == language).ToList().ForEach(x => gameFeatures.Add(new GameFeatureModel { Id = x.GameFeature.Id, GameFeatureName = x.GameFeatureName, LanguageId = language }));

            //works for language <> en
            BuildFeaturesTemplate(language, gameFeatures);


            return gameFeatures;
        }

        public List<GameFeatureDetailModel> GetGameFeatureDetails(int language)
        {
            var gameFeatureDetails = new List<GameFeatureDetailModel>();
            _ctx.GameFeatureDetailLanguages.Where(x => x.LanguageId == language).ToList().ForEach(x =>
            {
                gameFeatureDetails.Add(new GameFeatureDetailModel { Id = x.GameFeatureDetail.GameFeature.Id, GameFeatureDetailId = x.Id, GameFeatureDetailName = x.GameFeatureDetailName, Language = x.Language, LanguageId = x.LanguageId });
            });

            BuildFeaturesDetailsTemplate(language, gameFeatureDetails);

            return gameFeatureDetails;
        }


        private void BuildFeaturesTemplate(int language, List<GameFeatureModel> gameFeatures)
        {
            var checkIsExists = _ctx.Languages.FirstOrDefault(x => x.LanguageName == "English");
            if (checkIsExists != null)
            {
                int enId = checkIsExists.Id;
                if (enId != language) // then i build template features     
                    _ctx.GameFeatureLanguages.Where(x => x.LanguageId == enId).ToList().ForEach(x => gameFeatures.ForEach(z =>
                    {
                        if (z.Id == x.GameFeature.Id)
                            z.GameFeatureTemplateName = x.GameFeatureName;
                    }));
            }
        }

        //not be used 
        //public GameDescriptionModel GetGameDescription(int language)
        //{
        //var gameDescription = new GameDescriptionModel();

        //var gameDescription = new GameDescriptionModel();
        //_ctx.GameCategoryLanguages.Where(x => x.LanguageId == language)
        //    .ToList()
        //    .ForEach(z => gameDescription.GameCategories.Add(new GameCategoryModel { Id = z.GameCategory.Id, Language = z.Language, LanguageId = z.LanguageId, GameCategoryName = z.GameCategoryName }));

        //_ctx.NumberOfPlayerLanguages.Where(x => x.LanguageId == language)
        //    .ToList()
        //    .ForEach(z => gameDescription.NumberOfPlayers.Add(new NumberOfPlayerModel { Id = z.NumberOfPlayer.Id, Language = z.Language, LanguageId = z.LanguageId, NumberOfPlayerName = z.NumberOfPlayerName }));

        //_ctx.PaceOfPlayLanguages.Where(x => x.LanguageId == language)
        //    .ToList()
        //    .ForEach(z => gameDescription.PaceOfPlays.Add(new PaceOfPlayModel { Id = z.PaceOfPlay.Id, Language = z.Language, LanguageId = z.LanguageId, PaceOfPlayName = z.PaceOfPlayName }));

        //_ctx.PreparationFunLanguages.Where(x => x.LanguageId == language)
        //    .ToList()
        //    .ForEach(z => gameDescription.PreparationFuns.Add(new PreparationFunModel { Id = z.PreparationFun.Id, Language = z.Language, LanguageId = z.LanguageId, PreparationFunName = z.PreparationFunName }));

        //works for language <> en
        //BuildFeaturesDetailsTemplate(language, gameDescription);

        //return gameDescription;
        //}

        private void BuildFeaturesDetailsTemplate(int language, List<GameFeatureDetailModel> gameFeatureDetailModels)
        {
            var checkIsExists = _ctx.Languages.FirstOrDefault(x => x.LanguageName == "English");
            if (checkIsExists != null)
            {
                int enId = checkIsExists.Id;
                if (enId != language) // then i build template features for edit 
                {
                    _ctx.GameFeatureDetailLanguages.Where(x => x.LanguageId == enId)
                        .ToList()
                        .ForEach(x => gameFeatureDetailModels.ForEach(z =>
                         {
                             if (x.GameFeatureDetail.GameFeature.Id == z.Id)
                                 z.GameFeatureDetailTemplateName = x.GameFeatureDetailName;
                         }));
                }

            }
            //var checkIsExists = _ctx.Languages.FirstOrDefault(x => x.LanguageName == "English");
            //if (checkIsExists != null)
            //{
            //    int enId = checkIsExists.Id;
            //    if (enId != language) // then i build template features for edit 
            //    {
            //        _ctx.GameCategoryLanguages.Where(x => x.LanguageId == enId)
            //            .ToList()
            //            .ForEach(x => gameDescriptionModel.GameCategories.ForEach(z =>
            //            {
            //                if (z.Id == x.GameCategory.Id)
            //                    z.GameCategoryTemplateName = x.GameCategoryName;
            //            }));

            //        _ctx.NumberOfPlayerLanguages.Where(x => x.LanguageId == enId)
            //            .ToList()
            //            .ForEach(x => gameDescriptionModel.NumberOfPlayers.ForEach(z =>
            //            {
            //                if (z.Id == x.NumberOfPlayer.Id)
            //                    z.NumberOfPlayerTemplateName = x.NumberOfPlayerName;
            //            }));

            //        _ctx.PaceOfPlayLanguages.Where(x => x.LanguageId == language)
            //            .ToList()
            //            .ForEach(x => gameDescriptionModel.PaceOfPlays.ForEach(z =>
            //            {
            //                if (z.Id == x.PaceOfPlay.Id)
            //                    z.PaceOfPlayTemplateName = x.PaceOfPlayName;
            //            }));

            //        _ctx.PreparationFunLanguages.Where(x => x.LanguageId == language)
            //            .ToList()
            //            .ForEach(x => gameDescriptionModel.PreparationFuns.ForEach(z =>
            //            {
            //                if (z.Id == x.PreparationFun.Id)
            //                    z.PreparationFunTemplateName = x.PreparationFunName;
            //            }));
            //    }

            //}
        }
    }
}