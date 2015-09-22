﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBellwether.API.Context;
using WebBellwether.API.Entities.IntegrationGame;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Models;
using WebBellwether.API.Models.IntegrationGame;
using WebBellwether.API.Results;

namespace WebBellwether.API.Repositories
{
    public class Test
    {
        private readonly EfDbContext _ctx;
        public Test(EfDbContext ctx)
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

        public List<IntegrationGameModel> GetIntegrationGamesWithAvailableLanguages(int language)
        {
            List<Language> languages = _ctx.Languages.ToList();
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
                    IntegrationGameDetailModels = FillGameDetailModel(x.Id), //i take id from integrationgamedetails
                    GameTranslations = FillAvailableTranslation(x.IntegrationGame.Id, languages)
                });
            });
            return games;
        }

        private List<AvailableLanguage> FillAvailableTranslation(int gameId, List<Language> allLanguages)
        {
            var translation = new List<AvailableLanguage>();
            _ctx.IntegrationGameDetails.Where(x => x.IntegrationGame.Id == gameId).ToList().ForEach(z => translation.Add(new AvailableLanguage { Language = z.Language, HasTranslation = true }));
            allLanguages.ForEach(x =>
            {
                if (translation.FirstOrDefault(y => y.Language.Id == x.Id) == null)
                    translation.Add(new AvailableLanguage { Language = x, HasTranslation = false });
            });
            return translation;
        }

        private List<IntegrationGameDetailModel> FillGameDetailModel(int integrationGameDetailId) //i must take data for detail because datail can have many languages 
        {
            List<IntegrationGameDetailModel> result = new List<IntegrationGameDetailModel>();
            _ctx.IntegrationGameFeatures.Where(x => x.IntegrationGameDetail.Id == integrationGameDetailId)
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

        private ResultState CheckNewGameLanguage(NewIntegrationGameModel game)
        {
            if (
                _ctx.IntegrationGameDetails.FirstOrDefault(
                    x => x.IntegrationGame.Id == game.Id && x.Language.Id == game.Language.Id) == null)
                return ResultState.GameCanBeAdded;
            return ResultState.GameHaveTranslationForThisLanguage;
        }

        private ResultStateContainer InsertSingleLanguageGame(NewIntegrationGameModel game)
        {
            IntegrationGame entity = new IntegrationGame
            {
                CreationDate = DateTime.UtcNow,
                IntegrationGameDetails = new List<IntegrationGameDetail>
                {
                   BuildIntegrationGameDetail(game)
                }
            };
            _ctx.IntegrationGames.Add(entity);
            _ctx.SaveChanges();
            var integrationGameDetail = _ctx.IntegrationGameDetails.SingleOrDefault(x => x.IntegrationGameName.Contains(game.GameName));
            int integrationGameId = 0;
            if (integrationGameDetail != null)
            {
                integrationGameId =
                    integrationGameDetail
                        .IntegrationGame.Id;
            }
            return new ResultStateContainer { ResultState = ResultState.GameAdded, Value = integrationGameId };
        }

        private IntegrationGameDetail BuildIntegrationGameDetail(NewIntegrationGameModel game)
        {
            return new IntegrationGameDetail
            {
                Language = GetLanguage(game.Language.Id),
                IntegrationGameName = game.GameName,
                IntegrationGameDescription = game.GameDetails,
                IntegrationGameFeatures = GetGameFeatures(game.Features, game.Language.Id)
            };
        }

        private ResultStateContainer InsertSeveralLanguageGame(NewIntegrationGameModel game)
        {
            //if game have id i must check exists game for game language
            if (CheckNewGameLanguage(game) != ResultState.GameCanBeAdded)
                return new ResultStateContainer { ResultState = ResultState.GameHaveTranslationForThisLanguage, Value = game.Id };
            var entity = _ctx.IntegrationGames.SingleOrDefault(x => x.Id == game.Id);
            entity?.IntegrationGameDetails.Add(BuildIntegrationGameDetail(game));
            _ctx.SaveChanges();
            return new ResultStateContainer { ResultState = ResultState.SeveralLanguageGameAdded, Value = game.Id };
        }

        public ResultStateContainer InsertIntegrationGame(NewIntegrationGameModel game)
        {
            //probably i set here multiple language insert game 
            if (_ctx.IntegrationGameDetails.FirstOrDefault(x => x.IntegrationGameName.Contains(game.GameName)) != null)
                return new ResultStateContainer { ResultState = ResultState.ThisGameExistsInDb, Value = game.Id };
            if (game.Id == 0)
                return InsertSingleLanguageGame(game);
            return InsertSeveralLanguageGame(game);
        }

        //this function must go to static class , because this is f...
        private Language GetLanguage(int id)
        {
            return _ctx.Languages.FirstOrDefault(x => x.Id == id);
        }

        private List<IntegrationGameFeature> GetGameFeatures(int[] features, int language)
        {
            //its very important features == gameFeatureDetail.id not gamefeaturedetaillanguages.id i must use language to take good record 
            //first part , take feature detail
            var result = new List<IntegrationGameFeature>();
            features.ToList().ForEach(x =>
            {
                var entity = _ctx.GameFeatureDetailLanguages.FirstOrDefault(z => z.GameFeatureDetail.Id == x && z.Language.Id == language);
                result.Add(new IntegrationGameFeature { GameFeatureDetailLanguage = entity });
            });
            //second part ,take feature
            result.ForEach(x =>
            {
                var entity =
                    _ctx.GameFeatureLanguages.FirstOrDefault(
                        z =>
                            z.Language.Id == x.GameFeatureDetailLanguage.Language.Id &&
                            z.GameFeature.Id == x.GameFeatureDetailLanguage.GameFeatureDetail.GameFeature.Id);
                x.GameFeatureLanguage = entity;
            });
            return result;
        }


        public ResultState PutGameFeature(GameFeatureModel gameFeatureModel)
        {
            var entity =
                _ctx.GameFeatureLanguages.FirstOrDefault(
                    x => x.GameFeature.Id == gameFeatureModel.Id && x.Language.Id == gameFeatureModel.LanguageId);
            if (entity == null)
                return ResultState.GameFeatureNotExists;
            entity.GameFeatureName = gameFeatureModel.GameFeatureName;
            _ctx.SaveChanges();
            return ResultState.GameFeatureEdited;
        }

        public ResultState PutGameFeatureDetail(GameFeatureDetailModel gameFeatureDetailModel)
        {
            var entity =
                _ctx.GameFeatureDetailLanguages.FirstOrDefault(
                    x => x.Id == gameFeatureDetailModel.Id && x.Language.Id == gameFeatureDetailModel.LanguageId);
            if (entity == null)
                return ResultState.GameFeatureDetailNotExists;
            entity.GameFeatureDetailName = gameFeatureDetailModel.GameFeatureDetailName;
            _ctx.SaveChanges();
            return ResultState.GameFeatureDetailEdited;
        }



        public List<GameFeatureModel> GetGameFeatures(int language)
        {
            //id for header , gamefeaturename for details
            var gameFeatures = new List<GameFeatureModel>();
            _ctx.GameFeatureLanguages.Where(x => x.Language.Id == language).ToList().ForEach(x => gameFeatures.Add(new GameFeatureModel { Id = x.GameFeature.Id, GameFeatureName = x.GameFeatureName, LanguageId = language }));

            //works for language <> en
            BuildFeaturesTemplate(language, gameFeatures);


            return gameFeatures;
        }

        public List<GameFeatureDetailModel> GetGameFeatureDetails(int language)
        {
            var gameFeatureDetails = new List<GameFeatureDetailModel>();
            _ctx.GameFeatureDetailLanguages.Where(x => x.Language.Id == language).ToList().ForEach(x =>
            {
                //warning id is gamefeaturedetaillanguages id 
                gameFeatureDetails.Add(new GameFeatureDetailModel { Id = x.Id, GameFeatureDetailId = x.GameFeatureDetail.Id, GameFeatureId = x.GameFeatureDetail.GameFeature.Id, GameFeatureDetailName = x.GameFeatureDetailName, Language = x.Language.LanguageName, LanguageId = x.Language.Id });
            });

            //works for language <> en
            BuildFeaturesDetailsTemplate(language, gameFeatureDetails);

            return gameFeatureDetails;
        }


        private void BuildFeaturesTemplate(int language, List<GameFeatureModel> gameFeatures)
        {
            //this is not good i use statis language name ... 
            var checkIsExists = _ctx.Languages.FirstOrDefault(x => x.LanguageName == "English");
            if (checkIsExists != null)
            {
                int enId = checkIsExists.Id;
                if (enId != language) // then i build template features     
                    _ctx.GameFeatureLanguages.Where(x => x.Language.Id == enId).ToList().ForEach(x => gameFeatures.ForEach(z =>
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
                    _ctx.GameFeatureDetailLanguages.Where(x => x.Language.Id == enId)
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