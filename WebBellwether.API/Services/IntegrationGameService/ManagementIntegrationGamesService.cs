using System;
using System.Collections.Generic;
using System.Linq;
using WebBellwether.API.Entities.IntegrationGame;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Models.IntegrationGame;
using WebBellwether.API.Results;
using WebBellwether.API.Services.IntegrationGameService.Abstract;
using WebBellwether.API.Repositories.Abstract;

namespace WebBellwether.API.Services.IntegrationGameService
{
    public class ManagementIntegrationGamesService : IManagementIntegrationGamesService
    {
        private readonly IAggregateRepositories _repository;
        public ManagementIntegrationGamesService(IAggregateRepositories repository)
        {
            _repository = repository;
        }
        public IntegrationGameModel GetGameTranslation(int gameId, int languageId)
        {

            var entity = _repository.IntegrationGameDetailRepository.GetWithInclude(x => x.IntegrationGame.Id == gameId && x.Language.Id == languageId).FirstOrDefault();
            if (entity == null)
                return null;
            IntegrationGameModel integrationGame = new IntegrationGameModel { Id = gameId, GameName = entity.IntegrationGameName, GameDescription = entity.IntegrationGameDescription, Language = entity.Language, IntegrationGameId = entity.Id };
            return integrationGame;
        }
        public ResultStateContainer InsertSingleLanguageGame(NewIntegrationGameModel game)
        {
            try
            {
                IntegrationGame entity = new IntegrationGame
                {
                    CreationDate = DateTime.UtcNow,
                    IntegrationGameDetails = new List<IntegrationGameDetail>
                {
                   BuildIntegrationGameDetail(game)
                }
                };
                _repository.IntegrationGameRepository.Insert(entity);
                _repository.Save();
                var integrationGameDetail = _repository.IntegrationGameDetailRepository.GetWithInclude(x => x.IntegrationGameName.Contains(game.GameName)).FirstOrDefault();
                IntegrationGameModel returnEntity = new IntegrationGameModel
                {
                    Id = integrationGameDetail.IntegrationGame.Id, // this is global id 
                    IntegrationGameId = integrationGameDetail.Id, // id for translation
                    Language = integrationGameDetail.Language,
                    GameName = integrationGameDetail.IntegrationGameName,
                    GameDescription = integrationGameDetail.IntegrationGameDescription,
                };
                return new ResultStateContainer { ResultState = ResultState.Success,ResultMessage=ResultMessage.GameAdded, ResultValue = returnEntity };
            }
            catch(Exception)
            {
                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.Error };
            }
           
        }
        public IntegrationGameDetail BuildIntegrationGameDetail(NewIntegrationGameModel game)
        {
            List<IntegrationGameFeature> integrationGameFeatures = GetGameFeatures(game.Features, game.Language);
            if (integrationGameFeatures == null)
                return null;
            return new IntegrationGameDetail
            {
                Language = GetLanguage(game.Language),
                IntegrationGameName = game.GameName,
                IntegrationGameDescription = game.GameDetails,
                IntegrationGameFeatures = integrationGameFeatures
            };
        }
        public List<IntegrationGameFeature> GetGameFeatures(int[] features, int language)
        {
            //its very important features == gameFeatureDetail.id not gamefeaturedetaillanguages.id i must use language to take good record 
            //first part , take feature detail
            var result = new List<IntegrationGameFeature>();
            features.ToList().ForEach(x =>
            {
                var entity = _repository.GameFeatureDetailLanguageRepository.Get(z => z.GameFeatureDetail.Id == x && z.Language.Id == language);
                if (entity != null)
                    result.Add(new IntegrationGameFeature { GameFeatureDetailLanguage = entity });
            });
            //second part ,take feature
            result.ForEach(x =>
            {
                var entity =
                    _repository.GameFeatureLanguageRepository.GetWithInclude(
                        z =>
                            x.GameFeatureDetailLanguage.Language.Id == z.Language.Id &&
                            x.GameFeatureDetailLanguage.GameFeatureDetail.GameFeature.Id == z.GameFeature.Id).SingleOrDefault();

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
                    var gameFeatureEntity = _repository.IntegrationGameFeatureRepository.GetWithInclude(x => x.IntegrationGameDetail.IntegrationGame.Id == integrationGame.Id);
                    if (gameFeatureEntity != null)
                        gameFeatureEntity.ToList().ForEach(x =>
                        {
                            //delete game feature
                            _repository.IntegrationGameFeatureRepository.Delete(x);
                        });
                    var gameEntity = _repository.IntegrationGameDetailRepository.GetWithInclude(x => x.IntegrationGame.Id == integrationGame.Id);
                    if (gameEntity != null)
                        gameEntity.ToList().ForEach(x =>
                        {
                            //delete game detail 
                            _repository.IntegrationGameDetailRepository.Delete(x);
                        });
                    var mainGameEntity = _repository.IntegrationGameRepository.GetWithInclude(x => x.Id == integrationGame.Id).FirstOrDefault();
                    if (gameEntity != null)
                        //delete main game
                        _repository.IntegrationGameRepository.Delete(mainGameEntity);
                    _repository.Save();
                    return new ResultStateContainer { ResultState = ResultState.Success , ResultMessage=ResultMessage.GameRemoved };
                }
                else
                {
                    var gameEntity = _repository.IntegrationGameDetailRepository.GetWithInclude(x => x.Id == integrationGame.IntegrationGameId).FirstOrDefault();
                    if (gameEntity == null)
                        return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage = ResultMessage.GameRemoved };
                    var gameFeatureEntity = _repository.IntegrationGameFeatureRepository.GetWithInclude(x => x.IntegrationGameDetail.Id == gameEntity.Id);
                    if (gameFeatureEntity == null)
                        return new ResultStateContainer { ResultState = ResultState.Failure , ResultMessage = ResultMessage.GameFeatureTranslationNotExists };
                    int gameTranslationCount = 0;
                    if (integrationGame.GameTranslations != null) // when i have null here i know this is only translation
                        integrationGame.GameTranslations.ForEach(x =>
                        {
                            if (x.HasTranslation)
                                gameTranslationCount++;
                        });
                    //delete game feature
                    gameFeatureEntity.ToList().ForEach(x =>
                    {
                        _repository.IntegrationGameFeatureRepository.Delete(x);
                    });
                    //delete game detail 
                    _repository.IntegrationGameDetailRepository.Delete(gameEntity);
                    if (gameTranslationCount == 1)//have only one translation , can delete main id for game . Safe is safe ...
                    {
                        var mainGameEntity = _repository.IntegrationGameRepository.GetWithInclude(x => x.Id == integrationGame.Id).FirstOrDefault();
                        if (mainGameEntity != null)
                            //delete main game
                            _repository.IntegrationGameRepository.Delete(mainGameEntity);
                    }
                    _repository.Save();
                    return new ResultStateContainer { ResultState = ResultState.Success,ResultMessage = ResultMessage.GameRemoved };
                }
            }
            catch (Exception)
            {
                return new ResultStateContainer { ResultState = ResultState.Failure,ResultMessage=ResultMessage.Error };
            }

        }

        public ResultStateContainer PutIntegrationGame(IntegrationGameModel integrationGame)
        {
            //btw this code don't have user notification 
            //the code below is highly complex not touch
            try
            {
                var entity = _repository.IntegrationGameDetailRepository.GetWithInclude(x => x.Id == integrationGame.IntegrationGameId).FirstOrDefault();
                if (entity != null)
                {
                    entity.IntegrationGameName = integrationGame.GameName;
                    entity.IntegrationGameDescription = integrationGame.GameDescription;
                    _repository.Save();
                    //if (integrationGame.GameTranslations.Count() == 0)
                    //    return ResultState.GameTranslationAdded;//uwaga tutaj bede obslugiwal edycje innej translacji i tylko tyle 
                    integrationGame.GameTranslations.ForEach(x =>
                    {
                        if (x.HasTranslation)
                        {
                            entity = _repository.IntegrationGameDetailRepository.GetWithInclude(z => z.IntegrationGame.Id == integrationGame.Id && z.Language.Id == x.Language.Id).FirstOrDefault();
                            integrationGame.IntegrationGameDetailModels.ForEach(g =>
                            {
                                entity.IntegrationGameFeatures.FirstOrDefault(z => z.GameFeatureLanguage.GameFeature.Id == g.GameFeatureId)
                                .GameFeatureDetailLanguage = _repository.GameFeatureDetailLanguageRepository
                                .GetWithInclude(h => h.GameFeatureDetail.Id == g.GameFeatureDetailId && h.Language.Id == x.Language.Id).FirstOrDefault();
                            });
                            _repository.Save();
                            entity = null;
                        }
                    });
                    return new ResultStateContainer { ResultState = ResultState.Success,ResultMessage = ResultMessage.GameEdited };
                }
                return new ResultStateContainer { ResultState = ResultState.Success , ResultMessage = ResultMessage.GameNotExists };
            }
            catch(Exception)
            {
                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.Error };
            }
           
        }


        public Language GetLanguage(int id)
        {
            return _repository.LanguageRepository.Get(x => x.Id == id);
        }
        public List<GameFeatureModel> GetGameFeatures(int language)
        {
            //id for header , gamefeaturename for details
            var gameFeatures = new List<GameFeatureModel>();
            _repository.GameFeatureLanguageRepository.GetWithInclude(x => x.Language.Id == language).ToList().ForEach(x => gameFeatures.Add(new GameFeatureModel { Id = x.GameFeature.Id, GameFeatureName = x.GameFeatureName, LanguageId = language }));

            //works for language <> en
            BuildFeaturesTemplate(language, gameFeatures);
            return gameFeatures;
        }
        public void BuildFeaturesTemplate(int language, List<GameFeatureModel> gameFeatures)
        {
            //this is not good i use statis language name ... 
            var checkIsExists = _repository.LanguageRepository.GetFirst(x => x.LanguageName == "English");
            if (checkIsExists != null)
            {
                int enId = checkIsExists.Id;
                if (enId != language) // then i build template features     
                    _repository.GameFeatureLanguageRepository.GetWithInclude(x => x.Language.Id == enId).ToList().ForEach(x => gameFeatures.ForEach(z =>
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
                    var entityToEdit = _repository.IntegrationGameDetailRepository.GetWithInclude(x => x.Id == game.IntegrationGameId).FirstOrDefault();
                    if (entityToEdit == null)
                        return new ResultStateContainer { ResultMessage = ResultMessage.GameTranslationNotExists,ResultState = ResultState.Failure };

                    entityToEdit.IntegrationGameName = game.GameName;
                    entityToEdit.IntegrationGameDescription = game.GameDetails;
                    _repository.Save();
                    return new ResultStateContainer { ResultState = ResultState.Success ,ResultMessage=ResultMessage.GameTranslationEdited };
                }
                if (CheckNewGameLanguage(game) != ResultMessage.GameCanBeAdded)
                    return new ResultStateContainer (ResultState.Failure,ResultMessage.GameHaveTranslationForThisLanguage, game.Id);
                var entity = _repository.IntegrationGameRepository.GetWithInclude(x => x.Id == game.Id).FirstOrDefault();
                var gameDetails = BuildIntegrationGameDetail(game);
                if(!gameDetails.IntegrationGameFeatures.Any() )
                    return new ResultStateContainer {ResultState = ResultState.Failure,ResultMessage = ResultMessage.GameFeatureTranslationNotExists};
                entity?.IntegrationGameDetails.Add(gameDetails);
                _repository.Save();
                return new ResultStateContainer (ResultState.Success,  ResultMessage.SeveralLanguageGameAdded, game.Id );
            }
            catch (Exception)
            {
                return new ResultStateContainer {ResultState= ResultState.Failure,ResultMessage = ResultMessage.Error };
            }
        }
        public ResultMessage CheckNewGameLanguage(NewIntegrationGameModel game)
        {
            if (
               _repository.IntegrationGameDetailRepository.GetWithInclude(
                    x => x.IntegrationGame.Id == game.Id && x.Language.Id == game.Language).FirstOrDefault() == null)
                return ResultMessage.GameCanBeAdded;
            return ResultMessage.GameHaveTranslationForThisLanguage;
        }
        public ResultStateContainer InsertIntegrationGame(NewIntegrationGameModel game)
        {
            //probably i set here multiple language insert game 
            if (_repository.IntegrationGameDetailRepository.GetWithInclude(x => x.IntegrationGameName.Equals(game.GameName)).FirstOrDefault() != null)
                return new ResultStateContainer { ResultState = ResultState.Failure, ResultMessage = ResultMessage.GameExists };
            if (game.Id == 0)
                return InsertSingleLanguageGame(game);
            return InsertSeveralLanguageGame(game);
        }
    }
}