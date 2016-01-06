using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using WebBellwether.Models.Models.IntegrationGame;
using WebBellwether.Models.Models.Translation;
using WebBellwether.Models.Results;
using WebBellwether.Repositories.Entities.IntegrationGame;
using WebBellwether.Repositories.Entities.Translations;
using WebBellwether.Services.Factories;
using WebBellwether.Services.Utility;

namespace WebBellwether.Services.Services.IntegrationGameService
{
    public interface IManagementIntegrationGamesService
    {
        ResultStateContainer InsertSingleLanguageGame(NewIntegrationGameModel game);
        IntegrationGameDetailDao BuildIntegrationGameDetail(NewIntegrationGameModel game);
        LanguageDao GetLanguage(int id);
        List<GameFeatureModel> GetGameFeatures(int language);
        void BuildFeaturesTemplate(int language, List<GameFeatureModel> gameFeatures);
        List<IntegrationGameFeatureDao> GetGameFeatures(int[] features, int language);
        ResultStateContainer InsertSeveralLanguageGame(NewIntegrationGameModel game);
        ResultMessage CheckNewGameLanguage(NewIntegrationGameModel game);
        ResultStateContainer InsertIntegrationGame(NewIntegrationGameModel game);
        ResultStateContainer PutIntegrationGame(IntegrationGameModel integrationGame);
        ResultStateContainer DeleteIntegratiomGame(IntegrationGameModel integrationGame);
        IntegrationGameModel GetGameTranslation(int gameId, int languageId);
    }
    public class ManagementIntegrationGamesService : IManagementIntegrationGamesService
    {
        public IntegrationGameModel GetGameTranslation(int gameId, int languageId)
        {
            var entity =
                RepositoryFactory.Context.IntegrationGameDetails.FirstOrDefault(
                    x => x.IntegrationGame.Id == gameId && x.Language.Id == languageId);
            if (entity != null)
                return new IntegrationGameModel { Id = gameId, GameName = entity.IntegrationGameName, GameDescription = entity.IntegrationGameDescription, Language = new Language { Id = entity.Language.Id, IsPublic = entity.Language.IsPublic, LanguageName = entity.Language.LanguageName, LanguageShortName = entity.Language.LanguageShortName }, IntegrationGameId = entity.Id };
            return null;
        }

        public ResultStateContainer InsertSingleLanguageGame(NewIntegrationGameModel game)
        {
            try
            {
                IntegrationGameDao entity = new IntegrationGameDao
                {
                    CreationDate = DateTime.UtcNow,
                    IntegrationGameDetails = new List<IntegrationGameDetailDao>
                {
                   BuildIntegrationGameDetail(game)
                }
                };
                RepositoryFactory.Context.IntegrationGames.Add(entity);
                RepositoryFactory.Context.SaveChanges();
                IntegrationGameDetailDao integrationGameDetail =
                    RepositoryFactory.Context.IntegrationGameDetails.FirstOrDefault(
                        x => x.IntegrationGameName.Contains(game.GameName));
                IntegrationGameModel returnEntity = new IntegrationGameModel
                {
                    Id = integrationGameDetail.IntegrationGame.Id, // this is global id 
                    IntegrationGameId = integrationGameDetail.Id, // id for translation
                    Language =  ModelMapper.Map<Language,LanguageDao>(integrationGameDetail.Language),
                    GameName = integrationGameDetail.IntegrationGameName,
                    GameDescription = integrationGameDetail.IntegrationGameDescription,
                };
                return new ResultStateContainer { ResultState = ResultState.Success, ResultMessage = ResultMessage.GameAdded, ResultValue = returnEntity };
            }
            catch (Exception)
            {
                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.Error };
            }

        }
        public IntegrationGameDetailDao BuildIntegrationGameDetail(NewIntegrationGameModel game)
        {
            List<IntegrationGameFeatureDao> integrationGameFeatures = GetGameFeatures(game.Features, game.Language);
            if (integrationGameFeatures == null)
                return null;
            return new IntegrationGameDetailDao
            {
                Language = GetLanguage(game.Language),
                IntegrationGameName = game.GameName,
                IntegrationGameDescription = game.GameDetails,
                IntegrationGameFeatures = integrationGameFeatures
            };
        }
        public List<IntegrationGameFeatureDao> GetGameFeatures(int[] features, int language)
        {
            var result = new List<IntegrationGameFeatureDao>();
            features.ToList().ForEach(x =>
            {
                var entity =
                    RepositoryFactory.Context.GameFeatureDetailLanguages.FirstOrDefault(
                        z => z.GameFeatureDetail.Id == x && z.Language.Id == language);
                if (entity != null)
                    result.Add(new IntegrationGameFeatureDao { GameFeatureDetailLanguage = entity });
            });
            result.ForEach(x =>
            {
                var entity = RepositoryFactory.Context.GameFeatureLanguages.FirstOrDefault(z =>
                    x.GameFeatureDetailLanguage.Language.Id == z.Language.Id &&
                    x.GameFeatureDetailLanguage.GameFeatureDetail.GameFeature.Id == z.GameFeature.Id);
                x.GameFeatureLanguage = entity;
            });
            return result;
        }
        public ResultStateContainer DeleteIntegratiomGame(IntegrationGameModel integrationGame)
        {
            try
            {
                if (integrationGame.TemporarySeveralTranslationDelete)
                {
                    var gameFeatureEntity =
                        RepositoryFactory.Context.IntegrationGameFeatures.Where(
                            x => x.IntegrationGameDetail.IntegrationGame.Id == integrationGame.Id);
                    gameFeatureEntity.ToList().ForEach(x =>
                    {
                        RepositoryFactory.Context.IntegrationGameFeatures.Remove(x);
                    });
                    var gameEntity =
                        RepositoryFactory.Context.IntegrationGameDetails.Where(
                            x => x.IntegrationGame.Id == integrationGame.Id);
                    gameEntity.ToList().ForEach(x =>
                    {
                        RepositoryFactory.Context.IntegrationGameDetails.Remove(x);
                    });
                    var mainGameEntity =
                        RepositoryFactory.Context.IntegrationGames.FirstOrDefault(x => x.Id == integrationGame.Id);
                    RepositoryFactory.Context.IntegrationGames.Remove(mainGameEntity);
                    RepositoryFactory.Context.SaveChanges();
                    return new ResultStateContainer { ResultState = ResultState.Success, ResultMessage = ResultMessage.GameRemoved };
                }
                else
                {
                    var gameEntity =
                        RepositoryFactory.Context.IntegrationGameDetails.FirstOrDefault(
                            x => x.Id == integrationGame.IntegrationGameId);
                    if (gameEntity == null)
                        return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.GameRemoved };
                    var gameFeatureEntity =
                        RepositoryFactory.Context.IntegrationGameFeatures.Where(
                            x => x.IntegrationGameDetail.Id == gameEntity.Id);
                    if (!gameFeatureEntity.Any())
                        return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.GameFeatureTranslationNotExists };
                    int gameTranslationCount = 0;
                    integrationGame.GameTranslations?.ForEach(x =>
                    {
                        if (x.HasTranslation)
                            gameTranslationCount++;
                    });
                    //delete game feature
                    gameFeatureEntity.ToList().ForEach(x =>
                    {
                        RepositoryFactory.Context.IntegrationGameFeatures.Remove(x);
                    });
                    //delete game detail 
                    RepositoryFactory.Context.IntegrationGameDetails.Remove(gameEntity);
                    if (gameTranslationCount == 1)//have only one translation , can delete main id for game . Safe is safe ...
                    {
                        var mainGameEntity =
                            RepositoryFactory.Context.IntegrationGames.FirstOrDefault(x => x.Id == integrationGame.Id);
                        if (mainGameEntity != null)
                            //delete main game
                            RepositoryFactory.Context.IntegrationGames.Remove(mainGameEntity);
                    }
                    RepositoryFactory.Context.SaveChanges();
                    return new ResultStateContainer { ResultState = ResultState.Success, ResultMessage = ResultMessage.GameRemoved };
                }
            }
            catch (Exception)
            {
                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.Error };
            }

        }

        public ResultStateContainer PutIntegrationGame(IntegrationGameModel integrationGame)
        {
            //btw this code don't have user notification 
            //the code below is highly complex not touch
            try
            {
                var entity =
                    RepositoryFactory.Context.IntegrationGameDetails.FirstOrDefault(
                        x => x.Id == integrationGame.IntegrationGameId);
                if (entity != null)
                {
                    entity.IntegrationGameName = integrationGame.GameName;
                    entity.IntegrationGameDescription = integrationGame.GameDescription;
                    RepositoryFactory.Context.SaveChanges();
                    //if (integrationGame.GameTranslations.Count() == 0)
                    //    return ResultState.GameTranslationAdded;//uwaga tutaj bede obslugiwal edycje innej translacji i tylko tyle 
                    integrationGame.GameTranslations.ForEach(x =>
                    {
                        if (x.HasTranslation)
                        {
                            entity =
                                RepositoryFactory.Context.IntegrationGameDetails.FirstOrDefault(
                                    z => z.IntegrationGame.Id == integrationGame.Id && z.Language.Id == x.Language.Id);
                            integrationGame.IntegrationGameDetailModels.ForEach(g =>
                            {
                                var integrationGameFeatureDao = entity?.IntegrationGameFeatures.FirstOrDefault(
                                    z => z.GameFeatureLanguage.GameFeature.Id == g.GameFeatureId);
                                if (integrationGameFeatureDao != null)
                                    integrationGameFeatureDao
                                        .GameFeatureDetailLanguage =
                                        RepositoryFactory.Context.GameFeatureDetailLanguages.FirstOrDefault(
                                            h =>
                                                h.GameFeatureDetail.Id == g.GameFeatureDetailId &&
                                                h.Language.Id == x.Language.Id);
                            });
                            RepositoryFactory.Context.SaveChanges();
                            entity = null;
                        }
                    });
                    return new ResultStateContainer { ResultState = ResultState.Success, ResultMessage = ResultMessage.GameEdited };
                }
                return new ResultStateContainer { ResultState = ResultState.Success, ResultMessage = ResultMessage.GameNotExists };
            }
            catch (Exception)
            {
                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.Error };
            }

        }


        public LanguageDao GetLanguage(int id)
        {
            return RepositoryFactory.Context.Languages.FirstOrDefault(x => x.Id == id);
        }
        public List<GameFeatureModel> GetGameFeatures(int language)
        {
            //id for header , gamefeaturename for details
            var gameFeatures =
            RepositoryFactory.Context.GameFeatureLanguages.Where(x => x.Language.Id == language)
                .Select(
                    x =>
                        new GameFeatureModel
                        {
                            Id = x.GameFeature.Id,
                            GameFeatureName = x.GameFeatureName,
                            LanguageId = language
                        }).ToList();
            //works for language <> en
            BuildFeaturesTemplate(language, gameFeatures);
            return gameFeatures;
        }
        public void BuildFeaturesTemplate(int language, List<GameFeatureModel> gameFeatures)
        {
            //this is not good i use statis language name ... 
            var checkIsExists = RepositoryFactory.Context.Languages.FirstOrDefault(x => x.LanguageName == "English");
            if (checkIsExists != null)
            {
                int enId = checkIsExists.Id;
                if (enId != language) // then i build template features     
                    RepositoryFactory.Context.GameFeatureLanguages.Where(x => x.Language.Id == enId).ToList().ForEach(x => gameFeatures.ForEach(z =>
                    {
                        if (z.Id == x.GameFeature.Id)
                            z.GameFeatureTemplateName = x.GameFeatureName;
                    }));
            }
        }
        public ResultStateContainer InsertSeveralLanguageGame(NewIntegrationGameModel game)
        {
            try
            {
                //if game have id i must check exists game for game language
                if (game.IntegrationGameId != 0) // when i have this id i know this is edit 
                {
                    var entityToEdit =
                        RepositoryFactory.Context.IntegrationGameDetails.FirstOrDefault(
                            x => x.Id == game.IntegrationGameId);
                    if (entityToEdit == null)
                        return new ResultStateContainer { ResultMessage = ResultMessage.GameTranslationNotExists, ResultState = ResultState.Failure };

                    entityToEdit.IntegrationGameName = game.GameName;
                    entityToEdit.IntegrationGameDescription = game.GameDetails;
                    RepositoryFactory.Context.SaveChanges();
                    return new ResultStateContainer { ResultState = ResultState.Success, ResultMessage = ResultMessage.GameTranslationEdited };
                }
                if (CheckNewGameLanguage(game) != ResultMessage.GameCanBeAdded)
                    return new ResultStateContainer(ResultState.Failure, ResultMessage.GameHaveTranslationForThisLanguage, game.Id);
                var entity = RepositoryFactory.Context.IntegrationGames.FirstOrDefault(x => x.Id == game.Id);
                var gameDetails = BuildIntegrationGameDetail(game);
                if (!gameDetails.IntegrationGameFeatures.Any())
                    return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.GameFeatureTranslationNotExists };
                entity?.IntegrationGameDetails.Add(gameDetails);
                RepositoryFactory.Context.SaveChanges();
                return new ResultStateContainer(ResultState.Success, ResultMessage.SeveralLanguageGameAdded, game.Id);
            }
            catch (Exception)
            {
                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.Error };
            }
        }
        public ResultMessage CheckNewGameLanguage(NewIntegrationGameModel game)
        {
            if (RepositoryFactory.Context.IntegrationGameDetails.FirstOrDefault(x => x.IntegrationGame.Id == game.Id && x.Language.Id == game.Language) == null)
                return ResultMessage.GameCanBeAdded;
            return ResultMessage.GameHaveTranslationForThisLanguage;
        }
        public ResultStateContainer InsertIntegrationGame(NewIntegrationGameModel game)
        {
            //probably i set here multiple language insert game 
            if (RepositoryFactory.Context.IntegrationGameDetails.FirstOrDefault(x => x.IntegrationGameName.Equals(game.GameName)) != null)
                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.GameExists };
            if (game.Id == 0)
                return InsertSingleLanguageGame(game);
            return InsertSeveralLanguageGame(game);
        }
    }
}