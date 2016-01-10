using System;
using System.Collections.Generic;
using System.Linq;
using WebBellwether.Models.Models.Translation;
using WebBellwether.Models.Results;
using WebBellwether.Models.ViewModels.IntegrationGame;
using WebBellwether.Repositories.Entities.IntegrationGame;
using WebBellwether.Repositories.Entities.Translations;
using WebBellwether.Services.Factories;
using WebBellwether.Services.Utility;

namespace WebBellwether.Services.Services.IntegrationGameService
{
    public interface IManagementIntegrationGamesService
    {
        List<IntegrationGameViewModel> GetIntegrationGamesWithAvailableLanguages(int languageId);       
        IntegrationGameViewModel InsertIntegrationGame(NewIntegrationGameViewModel game);
        bool PutIntegrationGame(IntegrationGameViewModel game);
        bool RemoveIntegratiomGame(IntegrationGameViewModel game);
        IntegrationGameViewModel GetGameTranslation(int gameId, int languageId);

    }
    public class IntegrationGameManagementService : IManagementIntegrationGamesService
    {
        public List<IntegrationGameViewModel> GetIntegrationGamesWithAvailableLanguages(int languageId)
        {
            var gameDetailsDao =
                RepositoryFactory.Context.IntegrationGameDetails.Where(x => x.Language.Id == languageId).ToList();
            if (!gameDetailsDao.Any())
                return null;
            var games = new List<IntegrationGameViewModel>();
            gameDetailsDao.ForEach(x =>
            {
                games.Add(new IntegrationGameViewModel
                {
                    Id = x.IntegrationGame.Id, // this is global id 
                    IntegrationGameId = x.Id, // id for translation
                    Language = ModelMapper.Map<Language, LanguageDao>(x.Language),
                    GameName = x.IntegrationGameName,
                    GameDescription = x.IntegrationGameDescription,
                    IntegrationGameDetailModels = FillGameDetailModel(x.Id), //i take id from integrationgamedetails
                    GameTranslations = FillAvailableTranslation(x.IntegrationGame.Id)
                });
            });
            return games;
        }


        public IntegrationGameViewModel GetGameTranslation(int gameId, int languageId)
        {
            var entity =
                RepositoryFactory.Context.IntegrationGameDetails.FirstOrDefault(
                    x => x.IntegrationGame.Id == gameId && x.Language.Id == languageId);
            if (entity != null)
                return new IntegrationGameViewModel { Id = gameId, GameName = entity.IntegrationGameName, GameDescription = entity.IntegrationGameDescription, Language = new Language { Id = entity.Language.Id, IsPublic = entity.Language.IsPublic, LanguageName = entity.Language.LanguageName, LanguageShortName = entity.Language.LanguageShortName }, IntegrationGameId = entity.Id };
            return null;
        }             

        public bool RemoveIntegratiomGame(IntegrationGameViewModel game)
        {
            return game.TemporarySeveralTranslationDelete
                ? RemoveAllGameTranslation(game)
                : RemoveSelectedGameTranslation(game);
        }

  
        public bool PutIntegrationGame(IntegrationGameViewModel game)
        {
            game.GameTranslations = FillAvailableTranslation(game.Id);
            var entity = ValidateGetIntegrationGameDetailById(game.IntegrationGameId);
            entity.IntegrationGameName = game.GameName;
            entity.IntegrationGameDescription = game.GameDescription;
            RepositoryFactory.Context.SaveChanges();
            game.GameTranslations.ForEach(x =>
            {
                if (x.HasTranslation)
                {
                    entity =
                        RepositoryFactory.Context.IntegrationGameDetails.FirstOrDefault(
                            z => z.IntegrationGame.Id == game.Id && z.Language.Id == x.Language.Id);
                    game.IntegrationGameDetailModels.ForEach(g =>
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
            return true;
        }
     
        public IntegrationGameViewModel InsertIntegrationGame(NewIntegrationGameViewModel game)
        {
            if (!ValidateInsertIntegrationGame(game))
                throw new Exception(ResultMessage.JokeNotAdded.ToString());
            return ValidateGetInsertedGame(game);
        }

        private IntegrationGameDetailDao ValidateGetIntegrationGameDetailById(int integrationGameId)
        {
            var game = RepositoryFactory.Context.IntegrationGameDetails.FirstOrDefault(x => x.Id == integrationGameId);
            if (game == null)
                throw new Exception(ResultMessage.GameNotExists.ToString());
            return game;
        }

        private List<IntegrationGameFeatureDao> GetGameFeatures(int[] featuresIds, int languageId)
        {
            var result = new List<IntegrationGameFeatureDao>();
            featuresIds.ToList().ForEach(x =>
            {
                var entity =
                    RepositoryFactory.Context.GameFeatureDetailLanguages.FirstOrDefault(
                        z => z.GameFeatureDetail.Id == x && z.Language.Id == languageId);
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

        private bool RemoveAllGameTranslation(IntegrationGameViewModel game)
        {
            var gameFeatureEntity =
                       RepositoryFactory.Context.IntegrationGameFeatures.Where(
                           x => x.IntegrationGameDetail.IntegrationGame.Id == game.Id);
            RepositoryFactory.Context.IntegrationGameFeatures.RemoveRange(gameFeatureEntity);
            var gameEntity =
                RepositoryFactory.Context.IntegrationGameDetails.Where(
                    x => x.IntegrationGame.Id == game.Id);            
            RepositoryFactory.Context.IntegrationGameDetails.RemoveRange(gameEntity);
            var mainGameEntity =
                RepositoryFactory.Context.IntegrationGames.FirstOrDefault(x => x.Id == game.Id);
            RepositoryFactory.Context.IntegrationGames.Remove(mainGameEntity);
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private bool RemoveSelectedGameTranslation(IntegrationGameViewModel game)
        {
            var gameEntity = ValidateGetIntegrationGameDetailById(game.IntegrationGameId);                
            var gameFeatureEntity =
                RepositoryFactory.Context.IntegrationGameFeatures.Where(
                    x => x.IntegrationGameDetail.Id == gameEntity.Id);
            if (!gameFeatureEntity.Any())
                throw new Exception(ResultMessage.GameFeatureTranslationNotExists.ToString());
            int gameTranslationCount = 0;
            game.GameTranslations?.ForEach(x =>
            {
                if (x.HasTranslation)
                    gameTranslationCount++;
            });
            RepositoryFactory.Context.IntegrationGameFeatures.RemoveRange(gameFeatureEntity);
            RepositoryFactory.Context.IntegrationGameDetails.Remove(gameEntity);
            if (gameTranslationCount == 1)//have only one translation , can delete main id for game . Safe is safe ...
            {
                var mainGameEntity =
                    RepositoryFactory.Context.IntegrationGames.FirstOrDefault(x => x.Id == game.Id);
                if (mainGameEntity != null)
                    //delete main game
                    RepositoryFactory.Context.IntegrationGames.Remove(mainGameEntity);
            }
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private LanguageDao GetLanguage(int languageId)
        {
            return RepositoryFactory.Context.Languages.FirstOrDefault(x => x.Id == languageId);
        }

        private IntegrationGameDetailDao BuildIntegrationGameDetail(NewIntegrationGameViewModel game)
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

        private bool IsAnotherGameTranslation(NewIntegrationGameViewModel game)
        {
            return game.IntegrationGameId != 0;
        }

        private bool PutGameTranslation(NewIntegrationGameViewModel game)
        {
            var gameToEdit = ValidateGetIntegrationGameDetailById(game.IntegrationGameId);            
            gameToEdit.IntegrationGameName = game.GameName;
            gameToEdit.IntegrationGameDescription = game.GameDetails;
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private bool InsertAnotherGameTranslation(NewIntegrationGameViewModel game)
        {
            if (IsAnotherGameTranslation(game))
            {
                return PutGameTranslation(game);
            }
            CheckIsTranslationForThisLanguageExists(game);
            var entity = RepositoryFactory.Context.IntegrationGames.FirstOrDefault(x => x.Id == game.Id);
            var gameDetails = BuildIntegrationGameDetail(game);
            if (!gameDetails.IntegrationGameFeatures.Any())
                throw new Exception(ResultMessage.GameFeatureTranslationNotExists.ToString());
            entity?.IntegrationGameDetails.Add(gameDetails);
            RepositoryFactory.Context.SaveChanges();
            return true;
        }
        private List<IntegrationGameDetailViewModel> FillGameDetailModel(int integrationGameDetailId)
        {
            List<IntegrationGameDetailViewModel> result =
                RepositoryFactory.Context.IntegrationGameFeatures.Where(
                    x => x.IntegrationGameDetail.Id == integrationGameDetailId)
                    .Select(z => new IntegrationGameDetailViewModel
                    {
                        Id = z.GameFeatureDetailLanguage.Id,
                        GameFeatureId = z.GameFeatureLanguage.GameFeature.Id,
                        GameFeatureLanguageId = z.GameFeatureLanguage.Id,
                        GameFeatureName = z.GameFeatureLanguage.GameFeatureName,
                        GameFeatureDetailId = z.GameFeatureDetailLanguage.GameFeatureDetail.Id,
                        GameFeatureDetailName = z.GameFeatureDetailLanguage.GameFeatureDetailName
                    }).ToList();
            return result;
        }
        private void CheckIsTranslationForThisLanguageExists(NewIntegrationGameViewModel game)
        {
            if (
                RepositoryFactory.Context.IntegrationGameDetails.Any(
                    x => x.IntegrationGame.Id == game.Id && x.Language.Id == game.Language))
                throw new Exception(ResultMessage.GameHaveTranslationForThisLanguage.ToString());
        }

        private bool InsertMainGameTranslation(NewIntegrationGameViewModel game)
        {
            IntegrationGameDao entity = new IntegrationGameDao
            {
                CreationDate = DateTime.UtcNow,
                IntegrationGameDetails = new List<IntegrationGameDetailDao>()            
            };
            entity.IntegrationGameDetails.Add(BuildIntegrationGameDetail(game));
            RepositoryFactory.Context.IntegrationGames.Add(entity);
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private IntegrationGameViewModel ValidateGetInsertedGame(NewIntegrationGameViewModel game)
        {
            var insertedGame = RepositoryFactory.Context.IntegrationGameDetails.FirstOrDefault(
                        x => x.IntegrationGameName.Equals(game.GameName));
            if (insertedGame == null)
                throw new Exception(ResultMessage.GameNotExists.ToString());
            return IsMainGameTranslation(game)
                ? GetInsertedMainGame(insertedGame)
                : GetInsertedAnotherGame(insertedGame);
        }

        private IntegrationGameViewModel GetInsertedMainGame(IntegrationGameDetailDao insertedGame)
        {
            var game = GetInsertedAnotherGame(insertedGame);
            game.GameTranslations = FillAvailableTranslation(game.Id);
            game.IntegrationGameDetailModels = FillGameDetailModel(game.IntegrationGameId);
            return game;
        }

        private List<AvailableLanguage> FillAvailableTranslation(int gameId)
        {
            List<LanguageDao> allLanguages = RepositoryFactory.Context.Languages.ToList();
            var translation =
                RepositoryFactory.Context.IntegrationGameDetails.Where(x => x.IntegrationGame.Id == gameId).ToList()
                    .Select(
                        z =>
                            new AvailableLanguage
                            {
                                Language = ModelMapper.Map<Language, LanguageDao>(z.Language),
                                HasTranslation = true
                            }).ToList();
            allLanguages.ForEach(x =>
            {
                if (translation.FirstOrDefault(y => y.Language.Id == x.Id) == null)
                    translation.Add(new AvailableLanguage { Language = ModelMapper.Map<Language, LanguageDao>(x), HasTranslation = false });
            });
            return translation;
        }

        private IntegrationGameViewModel GetInsertedAnotherGame(IntegrationGameDetailDao insertedGame)
        {
            return new IntegrationGameViewModel
            {
                Id = insertedGame.IntegrationGame.Id, // this is global id 
                IntegrationGameId = insertedGame.Id, // id for translation
                Language = ModelMapper.Map<Language, LanguageDao>(insertedGame.Language),
                GameName = insertedGame.IntegrationGameName,
                GameDescription = insertedGame.IntegrationGameDescription,
            };
        }

        private bool ValidateInsertIntegrationGame(NewIntegrationGameViewModel game)
        {
            if (IsGameExists(game))
                throw new Exception(ResultMessage.GameExists.ToString());
            return IsMainGameTranslation(game) ? InsertMainGameTranslation(game) : InsertAnotherGameTranslation(game);
        }

        private bool IsGameExists(NewIntegrationGameViewModel game)
        {
            return RepositoryFactory.Context.IntegrationGameDetails.Any(x => x.IntegrationGameName.Equals(game.GameName));
        }

        private bool IsMainGameTranslation(NewIntegrationGameViewModel game)
        {
            return game.Id == 0;
        }
    }
}