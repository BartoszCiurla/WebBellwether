using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using WebBellwether.API.Context;
using WebBellwether.API.Entities.IntegrationGames;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Models;
using WebBellwether.API.Models.IntegrationGame;

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
            var entity = _ctx.IntegrationGameDetails.Where(x => x.Language.Id == language).ToList();
            entity.ForEach(x =>
            {
                games.Add(new IntegrationGameModel
                {
                    Id = x.IntegrationGame.Id,
                    Language = x.Language,
                    GameName = x.IntegrationGameName,
                    GameDescription = x.IntegrationGameDescription,
                    IntegrationGameDetailModels = FillGameDetailModel(x.Id) //i take id from integrationgamedetails
                });
            });
            return games;
        }

        private List<IntegrationGameDetailModel> FillGameDetailModel(int integrationGameDetailId) //i must take data for detail because datail can have many languages 
        {
            List<IntegrationGameDetailModel> result = new List<IntegrationGameDetailModel>();
            _ctx.IntegrationGameFeatures.Where(x=>x.IntegrationGameDetail.Id == integrationGameDetailId)
                .ToList()
                .ForEach(z =>
                {
                    result.Add(new IntegrationGameDetailModel
                    {
                        Id = z.GameFeatureDetailLanguage.Id,
                        GameFeatureId = z.GameFeatureLanguage.Id,
                        GameFeatureName = z.GameFeatureLanguage.GameFeatureName,
                        GameFeatureDetailName = z.GameFeatureDetailLanguage.GameFeatureDetailName
                    });
                });

            return result;
        } 



        public ResultState InsertIntegrationGame(NewIntegrationGameModel game)
        {
            IntegrationGame entity = new IntegrationGame
            {
                CreationDate = DateTime.UtcNow,
                IntegrationGameDetails = new List<IntegrationGameDetail>
                {
                    new IntegrationGameDetail
                    {
                        Language = GetLanguage(game.Language.Id),
                        IntegrationGameName = game.GameName,
                        IntegrationGameDescription = game.GameDetails,
                        IntegrationGameFeatures = GetGameFeaturesTest(game.Features,game.Language.Id)
                    }
                }
            };
            _ctx.IntegrationGames.Add(entity);
            _ctx.SaveChanges();                  
            return ResultState.GameAdded;
        }

        //this function must go to static class , because this is f...
        private Language GetLanguage(int id)
        {
            return _ctx.Languages.FirstOrDefault(x => x.Id == id);
        }

        private List<IntegrationGameFeature> GetGameFeaturesTest(int[] features,int language)
        {
            //its very important features == gameFeatureDetail.id not gamefeaturedetaillanguages.id i must use language to take good record 
            //first part , take feature detail
            var result = new List<IntegrationGameFeature>();
            features.ToList().ForEach(x =>
            {
                var entity = _ctx.GameFeatureDetailLanguages.FirstOrDefault(z => z.GameFeatureDetail.Id == x && z.LanguageId ==language);
                result.Add(new IntegrationGameFeature {GameFeatureDetailLanguage = entity});
            });
            //second part ,take feature
            result.ForEach(x =>
            {
                var entity =
                    _ctx.GameFeatureLanguages.FirstOrDefault(
                        z =>
                            z.LanguageId == x.GameFeatureDetailLanguage.LanguageId &&
                            z.GameFeature.Id == x.GameFeatureDetailLanguage.GameFeatureDetail.GameFeature.Id);               
                x.GameFeatureLanguage = entity;
            });
            return result;
        } 


        public ResultState PutGameFeature(GameFeatureModel gameFeatureModel)
        {
            var entity =
                _ctx.GameFeatureLanguages.FirstOrDefault(
                    x => x.GameFeature.Id == gameFeatureModel.Id && x.LanguageId == gameFeatureModel.LanguageId);
            if (entity != null)
                entity.GameFeatureName = gameFeatureModel.GameFeatureName;
            _ctx.SaveChanges();
            return ResultState.GameFeatureEdited;
        }

        public ResultState PutGameFeatureDetail(GameFeatureDetailModel gameFeatureDetailModel)
        {
            ResultState result = ResultState.GameFeatureDetailEdited;
            //first step i update basic rekord with new name 
            var entity =
                _ctx.GameFeatureDetailLanguages.FirstOrDefault(
                    x => x.Id == gameFeatureDetailModel.Id && x.LanguageId == gameFeatureDetailModel.LanguageId);
            if (entity != null)
                entity.GameFeatureDetailName = gameFeatureDetailModel.GameFeatureDetailName;
            //second step i update all integration games 

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
                //warning id is gamefeaturedetaillanguages id 
                gameFeatureDetails.Add(new GameFeatureDetailModel { Id = x.Id, GameFeatureDetailId = x.GameFeatureDetail.Id, GameFeatureId = x.GameFeatureDetail.GameFeature.Id,GameFeatureDetailName = x.GameFeatureDetailName, Language = x.Language, LanguageId = x.LanguageId });
            });

            //works for language <> en
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
                             if (z.GameFeatureDetailId == x.GameFeatureDetail.Id)
                                 z.GameFeatureDetailTemplateName = x.GameFeatureDetailName;
                         }));
                }

            }        
        }
    }
}