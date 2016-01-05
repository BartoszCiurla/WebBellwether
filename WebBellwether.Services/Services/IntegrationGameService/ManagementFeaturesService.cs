using System;
using System.Collections.Generic;
using System.Linq;
using WebBellwether.Models.Models.IntegrationGame;
using WebBellwether.Models.Results;
using WebBellwether.Repositories.Entities.IntegrationGame;
using WebBellwether.Repositories.Entities.Translations;
using WebBellwether.Repositories.Repositories.Abstract;
using IManagementFeaturesService = WebBellwether.Services.Services.IntegrationGameService.Abstract.IManagementFeaturesService;

namespace WebBellwether.Services.Services.IntegrationGameService
{
    public class ManagementFeaturesService : IManagementFeaturesService
    {
        private readonly IAggregateRepositories _repository;

        public ManagementFeaturesService(IAggregateRepositories repository)
        {
            _repository = repository;
        }
        public ResultMessage PutGameFeature(GameFeatureModel gameFeatureModel)
        {
            var entity =
               _repository.GameFeatureLanguageRepository.GetWithInclude(
                    x => x.GameFeature.Id == gameFeatureModel.Id && x.Language.Id == gameFeatureModel.LanguageId).SingleOrDefault();

            if (entity == null)
                return ResultMessage.GameFeatureNotExists;
            entity.GameFeatureName = gameFeatureModel.GameFeatureName;
            _repository.Save();
            return ResultMessage.GameFeatureEdited;
        }
        public ResultMessage PutGameFeatureDetail(GameFeatureDetailModel gameFeatureDetailModel)
        {
            var entity =
               _repository.GameFeatureDetailLanguageRepository.GetWithInclude(
                    x => x.Id == gameFeatureDetailModel.Id && x.Language.Id == gameFeatureDetailModel.LanguageId).SingleOrDefault();
            if (entity == null)
                return ResultMessage.GameFeatureDetailNotExists;
            entity.GameFeatureDetailName = gameFeatureDetailModel.GameFeatureDetailName;
            _repository.Save();
            return ResultMessage.GameFeatureDetailEdited;
        }


        public List<GameFeatureModel> GetGameFeatuesModelWithDetails(int language)
        {
            var gameFeaturesDetails = new List<GameFeatureModel>();
            _repository.GameFeatureLanguageRepository.GetWithInclude(x => x.Language.Id == language).ToList().ForEach(
                x =>
                {
                    gameFeaturesDetails.Add(new GameFeatureModel
                    {
                        Id = x.GameFeature.Id,
                        GameFeatureName = x.GameFeatureName,
                        GameFeatureDetailModels = new List<GameFeatureDetailModel>()
                    });
                });
            gameFeaturesDetails.ForEach(x =>
            {
                _repository.GameFeatureDetailLanguageRepository.GetWithInclude(
                    z => z.Language.Id == language && z.GameFeatureDetail.GameFeature.Id == x.Id).ToList().ForEach(
                        y =>
                        {
                            x.GameFeatureDetailModels.Add(new GameFeatureDetailModel { Id = y.Id, GameFeatureDetailId = y.GameFeatureDetail.Id, GameFeatureId = y.GameFeatureDetail.GameFeature.Id, GameFeatureDetailName = y.GameFeatureDetailName, Language = y.Language.LanguageName, LanguageId = y.Language.Id });
                        });                

            });
            return gameFeaturesDetails;
        }

        public List<GameFeatureDetailModel> GetGameFeatureDetails(int language)
        {
            var gameFeatureDetails = new List<GameFeatureDetailModel>();
            _repository.GameFeatureDetailLanguageRepository.GetWithInclude(x => x.Language.Id == language).ToList().ForEach(x =>
            {
                gameFeatureDetails.Add(new GameFeatureDetailModel { Id = x.Id, GameFeatureDetailId = x.GameFeatureDetail.Id, GameFeatureId = x.GameFeatureDetail.GameFeature.Id, GameFeatureDetailName = x.GameFeatureDetailName, Language = x.Language.LanguageName, LanguageId = x.Language.Id });
            });
            BuildFeaturesDetailsTemplate(language, gameFeatureDetails);

            return gameFeatureDetails;
        }
        public void BuildFeaturesDetailsTemplate(int language, List<GameFeatureDetailModel> gameFeatureDetailModels)
        {
            var checkIsExists = _repository.LanguageRepository.GetFirst(x => x.LanguageName == "English");
            if (checkIsExists != null)
            {
                int enId = checkIsExists.Id;
                if (enId != language) // then i build template features for edit 
                {
                    _repository.GameFeatureDetailLanguageRepository.GetWithInclude(x => x.Language.Id == enId)
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
            return _repository.LanguageRepository.GetWithInclude(x => x.Id == languageId).FirstOrDefault();
        }
        public bool CreateGameFeatures(int languageId)
        {
            try
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
                newGameFeatures.ToList().ForEach(x =>
                {
                    _repository.GameFeatureLanguageRepository.Insert(x);
                });
                newGameFeatureDetails.ToList().ForEach(x =>
                {
                    _repository.GameFeatureDetailLanguageRepository.Insert(x);
                });
                _repository.Save();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
           
        }

        private IEnumerable<GameFeatureDetailLanguageDao> CreateGameFeaturesDetailsLanguage(LanguageDao language)
        {
            var templateGameFeaturesDetails = GetTemplateGameFeatureDetails();
            if (templateGameFeaturesDetails == null)
                return null;
            var newFeatureDetailsLanguages = new List<GameFeatureDetailLanguageDao>();
            templateGameFeaturesDetails.ToList().ForEach(x =>
            {
                newFeatureDetailsLanguages.Add(new GameFeatureDetailLanguageDao {GameFeatureDetail = x,GameFeatureDetailName = "",Language = language});
            });
            return newFeatureDetailsLanguages;
        }

        private IEnumerable<GameFeatureDetailDao> GetTemplateGameFeatureDetails()
        {
            return
                _repository.GameFeatureDetailLanguageRepository.GetWithInclude(x => x.Language.Id == 1)
                    .Select(y => y.GameFeatureDetail);
        } 

        private IEnumerable<GameFeatureLanguageDao> CreateGameFeaturesLanguages(LanguageDao language)
        {
            var templateGameFeatures = GetTemplateGameFeatures();
            if (templateGameFeatures == null)
                return null;
            var newFeaturesLanguages = new List<GameFeatureLanguageDao>();
            templateGameFeatures.ToList().ForEach(gameFeature =>
            {
                newFeaturesLanguages.Add(new GameFeatureLanguageDao {GameFeature = gameFeature,Language = language,GameFeatureName = ""});
            });
            return newFeaturesLanguages;
        }

        private IEnumerable<GameFeatureDao> GetTemplateGameFeatures()
        {
            return
                _repository.GameFeatureLanguageRepository.GetWithInclude(x => x.Language.Id == 1)
                    .Select(y => y.GameFeature);
        } 

    }
}
