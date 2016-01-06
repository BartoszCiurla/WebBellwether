using System;
using System.Collections.Generic;
using System.Linq;
using WebBellwether.Models.Models.IntegrationGame;
using WebBellwether.Models.Results;
using WebBellwether.Repositories.Entities.IntegrationGame;
using WebBellwether.Repositories.Entities.Translations;
using WebBellwether.Services.Factories;

namespace WebBellwether.Services.Services.IntegrationGameService
{
    public interface IManagementFeaturesService
    {
        ResultMessage PutGameFeature(GameFeatureModel gameFeatureModel);
        ResultMessage PutGameFeatureDetail(GameFeatureDetailModel gameFeatureDetailModel);
        List<GameFeatureDetailModel> GetGameFeatureDetails(int language);
        List<GameFeatureModel> GetGameFeatuesModelWithDetails(int language);
        void BuildFeaturesDetailsTemplate(int language, List<GameFeatureDetailModel> gameFeatureDetailModels);
        bool CreateGameFeatures(int languageId);
    }
    public class ManagementFeaturesService : IManagementFeaturesService
    {
        public ResultMessage PutGameFeature(GameFeatureModel gameFeatureModel)
        {
            var entity =
                RepositoryFactory.Context.GameFeatureLanguages.FirstOrDefault(
                    x => x.GameFeature.Id == gameFeatureModel.Id && x.Language.Id == gameFeatureModel.LanguageId);
            if (entity == null)
                return ResultMessage.GameFeatureNotExists;
            entity.GameFeatureName = gameFeatureModel.GameFeatureName;
            RepositoryFactory.Context.SaveChanges();
            return ResultMessage.GameFeatureEdited;
        }
        public ResultMessage PutGameFeatureDetail(GameFeatureDetailModel gameFeatureDetailModel)
        {
            var entity =
                RepositoryFactory.Context.GameFeatureDetailLanguages.FirstOrDefault(
                    x => x.Id == gameFeatureDetailModel.Id && x.Language.Id == gameFeatureDetailModel.LanguageId);
            if (entity == null)
                return ResultMessage.GameFeatureDetailNotExists;
            entity.GameFeatureDetailName = gameFeatureDetailModel.GameFeatureDetailName;
            RepositoryFactory.Context.SaveChanges();
            return ResultMessage.GameFeatureDetailEdited;
        }


        public List<GameFeatureModel> GetGameFeatuesModelWithDetails(int language)
        {
            var gameFeaturesDetails =
                RepositoryFactory.Context.GameFeatureLanguages.Where(x => x.Language.Id == language)
                    .Select(x => new GameFeatureModel
                    {
                        Id = x.GameFeature.Id,
                        GameFeatureName = x.GameFeatureName,                        
                    }).ToList();
            foreach (var gameFeaturesDetail in gameFeaturesDetails)
            {
                gameFeaturesDetail.GameFeatureDetailModels = new List<GameFeatureDetailModel>();
            }
            gameFeaturesDetails.ForEach(x =>
            {
                RepositoryFactory.Context.GameFeatureDetailLanguages.Where(z => z.Language.Id == language && z.GameFeatureDetail.GameFeature.Id == x.Id).ToList().ForEach(
                        y =>
                        {
                            x.GameFeatureDetailModels.Add(new GameFeatureDetailModel { Id = y.Id, GameFeatureDetailId = y.GameFeatureDetail.Id, GameFeatureId = y.GameFeatureDetail.GameFeature.Id, GameFeatureDetailName = y.GameFeatureDetailName, Language = y.Language.LanguageName, LanguageId = y.Language.Id });
                        });
            });
            return gameFeaturesDetails;
        }

        public List<GameFeatureDetailModel> GetGameFeatureDetails(int language)
        {
            var gameFeatureDetails =
                RepositoryFactory.Context.GameFeatureDetailLanguages.Where(x => x.Language.Id == language)
                    .Select(
                        x =>
                            new GameFeatureDetailModel
                            {
                                Id = x.Id,
                                GameFeatureDetailId = x.GameFeatureDetail.Id,
                                GameFeatureId = x.GameFeatureDetail.GameFeature.Id,
                                GameFeatureDetailName = x.GameFeatureDetailName,
                                Language = x.Language.LanguageName,
                                LanguageId = x.Language.Id
                            }).ToList();
            BuildFeaturesDetailsTemplate(language, gameFeatureDetails);

            return gameFeatureDetails;
        }
        public void BuildFeaturesDetailsTemplate(int language, List<GameFeatureDetailModel> gameFeatureDetailModels)
        {
            var checkIsExists = RepositoryFactory.Context.Languages.FirstOrDefault(x => x.LanguageName == "English");
            if (checkIsExists != null)
            {
                int enId = checkIsExists.Id;
                if (enId != language) // then i build template features for edit 
                {
                    RepositoryFactory.Context.GameFeatureDetailLanguages.Where(x => x.Language.Id == enId)
                         .ToList()
                         .ForEach(x => gameFeatureDetailModels.ForEach(z =>
                         {
                             if (z.GameFeatureDetailId == x.GameFeatureDetail.Id)
                                 z.GameFeatureDetailTemplateName = x.GameFeatureDetailName;
                         }));
                }
            }
        }

        private LanguageDao GetLanguage(int languageId)
        {
            return RepositoryFactory.Context.Languages.FirstOrDefault(x => x.Id == languageId);
        }
        public bool CreateGameFeatures(int languageId)
        {

            LanguageDao languageForGameFeatures = GetLanguage(languageId);
            if (languageForGameFeatures == null)
                return false;
            IEnumerable<GameFeatureLanguageDao> newGameFeatures = CreateGameFeaturesLanguages(languageForGameFeatures);
            if (newGameFeatures == null)
                return false;
            IEnumerable<GameFeatureDetailLanguageDao> newGameFeatureDetails =
                CreateGameFeaturesDetailsLanguage(languageForGameFeatures);
            if (newGameFeatureDetails == null)
                return false;
            RepositoryFactory.Context.GameFeatureLanguages.AddRange(newGameFeatures);
            RepositoryFactory.Context.GameFeatureDetailLanguages.AddRange(newGameFeatureDetails);
            RepositoryFactory.Context.SaveChanges();
            return true;
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
                return null;
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
