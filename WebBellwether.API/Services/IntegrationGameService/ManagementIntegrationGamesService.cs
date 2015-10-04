using System;
using System.Collections.Generic;
using System.Linq;
using WebBellwether.API.Entities.IntegrationGame;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Models.IntegrationGame;
using WebBellwether.API.Repositories;
using WebBellwether.API.Results;
using WebBellwether.API.Services.IntegrationGameService.Abstract;

namespace WebBellwether.API.Services.IntegrationGameService
{
    public class ManagementIntegrationGamesService : IManagementIntegrationGamesService
    {
        private readonly UnitOfWork _unitOfWork;
        public ManagementIntegrationGamesService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ResultStateContainer InsertSingleLanguageGame(NewIntegrationGameModel game)
        {
            IntegrationGame entity = new IntegrationGame
            {
                CreationDate = DateTime.UtcNow,
                IntegrationGameDetails = new List<IntegrationGameDetail>
                {
                   BuildIntegrationGameDetail(game)
                }
            };
            _unitOfWork.IntegrationGameRepository.Insert(entity);
            _unitOfWork.Save();
            var integrationGameDetail = _unitOfWork.IntegrationGameDetailRepository.GetWithInclude(x => x.IntegrationGameName.Contains(game.GameName)).SingleOrDefault();
            int integrationGameId = 0;
            if (integrationGameDetail != null)
            {
                integrationGameId =
                    integrationGameDetail
                        .IntegrationGame.Id;
            }
            return new ResultStateContainer { ResultState = ResultState.GameAdded, Value = integrationGameId };
        }
        public IntegrationGameDetail BuildIntegrationGameDetail(NewIntegrationGameModel game)
        {
            return new IntegrationGameDetail
            {
                Language = GetLanguage(game.Language.Id),
                IntegrationGameName = game.GameName,
                IntegrationGameDescription = game.GameDetails,
                IntegrationGameFeatures = GetGameFeatures(game.Features, game.Language.Id)
            };
        }
        public List<IntegrationGameFeature> GetGameFeatures(int[] features, int language)
        {
            //its very important features == gameFeatureDetail.id not gamefeaturedetaillanguages.id i must use language to take good record 
            //first part , take feature detail
            var result = new List<IntegrationGameFeature>();
            features.ToList().ForEach(x =>
            {
                var entity = _unitOfWork.GameFeatureDetailLanguageRepository.Get(z => z.GameFeatureDetail.Id == x && z.Language.Id == language);
                if (entity != null)
                    result.Add(new IntegrationGameFeature { GameFeatureDetailLanguage = entity });
            });
            //second part ,take feature
            result.ForEach(x =>
            {
                var entity =
                    _unitOfWork.GameFeatureLanguageRepository.GetWithInclude(
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
                    var gameFeatureEntity = _unitOfWork.IntegrationGameFeatureRepository.GetWithInclude(x => x.IntegrationGameDetail.IntegrationGame.Id == integrationGame.Id);
                    if (gameFeatureEntity != null)
                        gameFeatureEntity.ToList().ForEach(x =>
                        {
                            //delete game feature
                            _unitOfWork.IntegrationGameFeatureRepository.Delete(x);
                        });
                    var gameEntity = _unitOfWork.IntegrationGameDetailRepository.GetWithInclude(x => x.IntegrationGame.Id == integrationGame.Id);
                    if (gameEntity != null)
                        gameEntity.ToList().ForEach(x =>
                        {
                            //delete game detail 
                            _unitOfWork.IntegrationGameDetailRepository.Delete(x);
                        });
                    var mainGameEntity = _unitOfWork.IntegrationGameRepository.GetWithInclude(x => x.Id == integrationGame.Id).FirstOrDefault();
                    if (gameEntity != null)
                        //delete main game
                        _unitOfWork.IntegrationGameRepository.Delete(mainGameEntity);
                    _unitOfWork.Save();
                    return new ResultStateContainer { ResultState = ResultState.GameRemoved };
                }
                else
                {
                    var gameEntity = _unitOfWork.IntegrationGameDetailRepository.GetWithInclude(x => x.Id == integrationGame.IntegrationGameId).FirstOrDefault();
                    if (gameEntity == null)
                        return new ResultStateContainer { ResultState= ResultState.GameTranslationNotExists};
                    var gameFeatureEntity = _unitOfWork.IntegrationGameFeatureRepository.GetWithInclude(x => x.IntegrationGameDetail.Id == gameEntity.Id);
                    if (gameFeatureEntity == null)
                        return new ResultStateContainer { ResultState = ResultState.GameFeatureTranslationNotExists };
                    int gameTranslationCount = 0;
                    integrationGame.GameTranslations.ForEach(x =>
                    {
                        if (x.HasTranslation)
                            gameTranslationCount++;
                    });
                    //delete game feature
                    gameFeatureEntity.ToList().ForEach(x =>
                    {
                        _unitOfWork.IntegrationGameFeatureRepository.Delete(x);
                    });
                    //delete game detail 
                    _unitOfWork.IntegrationGameDetailRepository.Delete(gameEntity);
                    if (gameTranslationCount == 1)//have only one translation , can delete main id for game 
                    {
                        var mainGameEntity = _unitOfWork.IntegrationGameRepository.GetWithInclude(x => x.Id == integrationGame.Id).FirstOrDefault();
                        if(mainGameEntity != null)
                            //delete main game
                            _unitOfWork.IntegrationGameRepository.Delete(mainGameEntity);
                    }
                    _unitOfWork.Save();
                    return new ResultStateContainer { ResultState = ResultState.GameRemoved };
                }
            }
            catch (Exception e)
            {
                return new ResultStateContainer { ResultState = ResultState.RemoveGameError, Value = e };
            }
            
        }

        public ResultState PutIntegrationGame(IntegrationGameModel integrationGame)
        {
            //the code below is highly complex not touch
            var entity = _unitOfWork.IntegrationGameDetailRepository.GetWithInclude(x => x.Id == integrationGame.IntegrationGameId).FirstOrDefault();
            if (entity != null)
            {
                entity.IntegrationGameName = integrationGame.GameName;
                entity.IntegrationGameDescription = integrationGame.GameDescription;
                _unitOfWork.Save();
                integrationGame.GameTranslations.ForEach(x =>
                {
                    if (x.HasTranslation)
                    {
                        entity = _unitOfWork.IntegrationGameDetailRepository.GetWithInclude(z => z.IntegrationGame.Id == integrationGame.Id && z.Language.Id == x.Language.Id).FirstOrDefault();
                        integrationGame.IntegrationGameDetailModels.ForEach(g =>
                        {
                            entity.IntegrationGameFeatures.FirstOrDefault(z => z.GameFeatureLanguage.GameFeature.Id == g.GameFeatureId)
                            .GameFeatureDetailLanguage = _unitOfWork.GameFeatureDetailLanguageRepository
                            .GetWithInclude(h => h.GameFeatureDetail.Id == g.GameFeatureDetailId && h.Language.Id == x.Language.Id).FirstOrDefault();
                        });
                        _unitOfWork.Save();
                        entity = null;
                    }
                });                
                return ResultState.GameAdded;
            }
            return ResultState.GameAdded;
        }


        public Language GetLanguage(int id)
        {
            return _unitOfWork.LanguageRepository.Get(x => x.Id == id);
        }
        public List<GameFeatureModel> GetGameFeatures(int language)
        {
            //id for header , gamefeaturename for details
            var gameFeatures = new List<GameFeatureModel>();
            _unitOfWork.GameFeatureLanguageRepository.GetWithInclude(x => x.Language.Id == language).ToList().ForEach(x => gameFeatures.Add(new GameFeatureModel { Id = x.GameFeature.Id, GameFeatureName = x.GameFeatureName, LanguageId = language }));

            //works for language <> en
            BuildFeaturesTemplate(language, gameFeatures);
            return gameFeatures;
        }
        public void BuildFeaturesTemplate(int language, List<GameFeatureModel> gameFeatures)
        {
            //this is not good i use statis language name ... 
            var checkIsExists = _unitOfWork.LanguageRepository.GetFirst(x => x.LanguageName == "English");
            if (checkIsExists != null)
            {
                int enId = checkIsExists.Id;
                if (enId != language) // then i build template features     
                    _unitOfWork.GameFeatureLanguageRepository.GetWithInclude(x => x.Language.Id == enId).ToList().ForEach(x => gameFeatures.ForEach(z =>
                    {
                        if (z.Id == x.GameFeature.Id)
                            z.GameFeatureTemplateName = x.GameFeatureName;
                    }));
            }
        }
        public ResultStateContainer InsertSeveralLanguageGame(NewIntegrationGameModel game)
        {
            //if game have id i must check exists game for game language
            if (CheckNewGameLanguage(game) != ResultState.GameCanBeAdded)
                return new ResultStateContainer { ResultState = ResultState.GameHaveTranslationForThisLanguage, Value = game.Id };
            var entity = _unitOfWork.IntegrationGameRepository.GetFirst(x => x.Id == game.Id);
            entity?.IntegrationGameDetails.Add(BuildIntegrationGameDetail(game));
            _unitOfWork.Save();
            return new ResultStateContainer { ResultState = ResultState.SeveralLanguageGameAdded, Value = game.Id };
        }
        public ResultState CheckNewGameLanguage(NewIntegrationGameModel game)
        {
            if (
               _unitOfWork.IntegrationGameDetailRepository.GetFirst(
                    x => x.IntegrationGame.Id == game.Id && x.Language.Id == game.Language.Id) == null)
                return ResultState.GameCanBeAdded;
            return ResultState.GameHaveTranslationForThisLanguage;
        }
        public ResultStateContainer InsertIntegrationGame(NewIntegrationGameModel game)
        {
            //probably i set here multiple language insert game 
            if (_unitOfWork.IntegrationGameDetailRepository.GetFirst(x => x.IntegrationGameName.Contains(game.GameName)) != null)
                return new ResultStateContainer { ResultState = ResultState.ThisGameExistsInDb, Value = game.Id };
            if (game.Id == 0)
                return InsertSingleLanguageGame(game);
            return InsertSeveralLanguageGame(game);
        }
    }
}