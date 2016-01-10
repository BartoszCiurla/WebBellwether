using System;
using System.Collections.Generic;
using System.Linq;
using WebBellwether.Models.Results;
using WebBellwether.Models.ViewModels.IntegrationGame;
using WebBellwether.Repositories.Entities.IntegrationGame;
using WebBellwether.Repositories.Entities.Translations;
using WebBellwether.Services.Factories;

namespace WebBellwether.Services.Services.IntegrationGameService
{
    public interface IManagementFeaturesService
    {
        bool PutGameFeature(GameFeatureViewModel gameFeature);
        bool PutGameFeatureDetail(GameFeatureDetailViewModel gameFeatureDetail);
        GameFeatureViewModel[] GetGameFeatures(int languageId);
        GameFeatureDetailViewModel[] GetGameFeatureDetails(int languageId);
        GameFeatureViewModel[] GetGameFeatuesModelWithDetails(int languageId);
        GameFeatureViewModel[] CreateGameFeatures(int languageId);
    }
    public class GameFeatureManagementService : IManagementFeaturesService
    {
        private const string TemplateLanguageName = "English";
        public bool PutGameFeature(GameFeatureViewModel gameFeature)
        {
            var entity =
                RepositoryFactory.Context.GameFeatureLanguages.FirstOrDefault(
                    x => x.GameFeature.Id == gameFeature.Id && x.Language.Id == gameFeature.LanguageId);
            if (entity == null)
                throw new Exception(ResultMessage.GameFeatureNotExists.ToString());
            entity.GameFeatureName = gameFeature.GameFeatureName;
            RepositoryFactory.Context.SaveChanges();
            return true;
        }
        public bool PutGameFeatureDetail(GameFeatureDetailViewModel gameFeatureDetail)
        {
            var entity =
                RepositoryFactory.Context.GameFeatureDetailLanguages.FirstOrDefault(
                    x => x.Id == gameFeatureDetail.Id && x.Language.Id == gameFeatureDetail.LanguageId);
            if (entity == null)
                throw new Exception(ResultMessage.GameFeatureDetailNotExists.ToString());
            entity.GameFeatureDetailName = gameFeatureDetail.GameFeatureDetailName;
            RepositoryFactory.Context.SaveChanges();
            return true;
        }
        public GameFeatureViewModel[] GetGameFeatures(int languageId)
        {
            //id for header , gamefeaturename for details
            var gameFeatures =
            RepositoryFactory.Context.GameFeatureLanguages.Where(x => x.Language.Id == languageId)
                .Select(
                    x =>
                        new GameFeatureViewModel
                        {
                            Id = x.GameFeature.Id,
                            GameFeatureName = x.GameFeatureName,
                            LanguageId = languageId
                        }).ToList();
            //works for language <> en
            BuildFeaturesTemplate(languageId, gameFeatures);
            return gameFeatures.ToArray();
        }

        public GameFeatureViewModel[] GetGameFeatuesModelWithDetails(int languageId)
        {
            var gameFeaturesDetails =
                RepositoryFactory.Context.GameFeatureLanguages.Where(x => x.Language.Id == languageId)
                    .Select(x => new GameFeatureViewModel
                    {
                        Id = x.GameFeature.Id,
                        GameFeatureName = x.GameFeatureName,
                    }).ToList();
            foreach (var gameFeaturesDetail in gameFeaturesDetails)
            {
                gameFeaturesDetail.GameFeatureDetailModels = new List<GameFeatureDetailViewModel>();
            }
            gameFeaturesDetails.ForEach(x =>
            {
                RepositoryFactory.Context.GameFeatureDetailLanguages.Where(z => z.Language.Id == languageId && z.GameFeatureDetail.GameFeature.Id == x.Id).ToList().ForEach(
                        y =>
                        {
                            x.GameFeatureDetailModels.Add(new GameFeatureDetailViewModel { Id = y.Id, GameFeatureDetailId = y.GameFeatureDetail.Id, GameFeatureId = y.GameFeatureDetail.GameFeature.Id, GameFeatureDetailName = y.GameFeatureDetailName, Language = y.Language.LanguageName, LanguageId = y.Language.Id });
                        });
            });
            return gameFeaturesDetails.ToArray();
        }

        public GameFeatureDetailViewModel[] GetGameFeatureDetails(int languageId)
        {
            var gameFeatureDetails =
                RepositoryFactory.Context.GameFeatureDetailLanguages.Where(x => x.Language.Id == languageId)
                    .Select(
                        x =>
                            new GameFeatureDetailViewModel
                            {
                                Id = x.Id,
                                GameFeatureDetailId = x.GameFeatureDetail.Id,
                                GameFeatureId = x.GameFeatureDetail.GameFeature.Id,
                                GameFeatureDetailName = x.GameFeatureDetailName,
                                Language = x.Language.LanguageName,
                                LanguageId = x.Language.Id
                            }).ToList();
            BuildFeaturesDetailsTemplate(languageId, gameFeatureDetails);

            return gameFeatureDetails.ToArray();
        }

        public GameFeatureViewModel[] CreateGameFeatures(int languageId)
        {
            if (ValidateCreateGameFeature(languageId))
                return GetGameFeatures(languageId);
            throw new Exception(ResultMessage.GameFeatureNotExists.ToString());
        }

        private void BuildFeaturesDetailsTemplate(int languageId, List<GameFeatureDetailViewModel> gameFeatureDetails)
        {
            var checkIsExists = ValidateGetTemplateLanguage();
            int enId = checkIsExists.Id;
            if (enId != languageId) // then i build template features for edit 
            {
                RepositoryFactory.Context.GameFeatureDetailLanguages.Where(x => x.Language.Id == enId)
                     .ToList()
                     .ForEach(x => gameFeatureDetails.ForEach(z =>
                     {
                         if (z.GameFeatureDetailId == x.GameFeatureDetail.Id)
                             z.GameFeatureDetailTemplateName = x.GameFeatureDetailName;
                     }));
            }
        }

        private LanguageDao ValidateGetTemplateLanguage()
        {
            var templateLanguage = RepositoryFactory.Context.Languages.FirstOrDefault(x => x.LanguageName == TemplateLanguageName);
            if (templateLanguage == null)
                throw new Exception(ResultMessage.LanguageNotExists.ToString());
            return templateLanguage;
        }

        private LanguageDao GetLanguage(int languageId)
        {
            return RepositoryFactory.Context.Languages.FirstOrDefault(x => x.Id == languageId);
        }

        private bool ValidateCreateGameFeature(int languageId)
        {
            LanguageDao languageForGameFeatures = GetLanguage(languageId);
            if (languageForGameFeatures == null)
                throw new Exception(ResultMessage.LanguageNotExists.ToString());
            IEnumerable<GameFeatureLanguageDao> newGameFeatures = CreateGameFeaturesLanguages(languageForGameFeatures);
            IEnumerable<GameFeatureDetailLanguageDao> newGameFeatureDetails =
                CreateGameFeaturesDetailsLanguage(languageForGameFeatures);
            if (newGameFeatureDetails == null)
                throw new Exception(ResultMessage.GameFeatureDetailNotExists.ToString());
            RepositoryFactory.Context.GameFeatureLanguages.AddRange(newGameFeatures);
            RepositoryFactory.Context.GameFeatureDetailLanguages.AddRange(newGameFeatureDetails);
            RepositoryFactory.Context.SaveChanges();
            return true;
        }


        private void BuildFeaturesTemplate(int languageId, List<GameFeatureViewModel> gameFeatures)
        {
            var checkIsExists = ValidateGetTemplateLanguage();
            int enId = checkIsExists.Id;
            if (enId != languageId)
                RepositoryFactory.Context.GameFeatureLanguages.Where(x => x.Language.Id == enId).ToList().ForEach(x => gameFeatures.ForEach(z =>
                {
                    if (z.Id == x.GameFeature.Id)
                        z.GameFeatureTemplateName = x.GameFeatureName;
                }));
        }

        private IEnumerable<GameFeatureDetailLanguageDao> CreateGameFeaturesDetailsLanguage(LanguageDao language)
        {
            var templateGameFeaturesDetails = GetTemplateGameFeatureDetails();
            if (templateGameFeaturesDetails == null)
                return null;
            var newFeatureDetailsLanguages = new List<GameFeatureDetailLanguageDao>();
            templateGameFeaturesDetails.ToList().ForEach(x =>
            {
                newFeatureDetailsLanguages.Add(new GameFeatureDetailLanguageDao { GameFeatureDetail = x, GameFeatureDetailName = "", Language = language });
            });
            return newFeatureDetailsLanguages;
        }

        private IEnumerable<GameFeatureDetailDao> GetTemplateGameFeatureDetails()
        {
            return
                RepositoryFactory.Context.GameFeatureDetailLanguages.Where(x => x.Language.Id == 1)
                    .Select(x => x.GameFeatureDetail);
        }

        private IEnumerable<GameFeatureLanguageDao> CreateGameFeaturesLanguages(LanguageDao language)
        {
            var templateGameFeatures = GetTemplateGameFeatures();
            if (templateGameFeatures == null)
                throw new Exception(ResultMessage.GameFeatureNotExists.ToString());
            var newFeaturesLanguages = new List<GameFeatureLanguageDao>();
            templateGameFeatures.ToList().ForEach(gameFeature =>
            {
                newFeaturesLanguages.Add(new GameFeatureLanguageDao { GameFeature = gameFeature, Language = language, GameFeatureName = "" });
            });
            return newFeaturesLanguages;
        }

        private IEnumerable<GameFeatureDao> GetTemplateGameFeatures()
        {
            return
                RepositoryFactory.Context.GameFeatureLanguages.Where(x => x.Language.Id == 1).Select(x => x.GameFeature);
        }

    }
}
