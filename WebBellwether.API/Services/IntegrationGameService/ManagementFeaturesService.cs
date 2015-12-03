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

        private Language GetLanguage(int languageId)
        {
            return _repository.LanguageRepository.GetWithInclude(x => x.Id == languageId).FirstOrDefault();
        }
        public bool CreateGameFeatures(int languageId)
        {
            try
            {
                Language languageForGameFeatures = GetLanguage(languageId);
                if (languageForGameFeatures == null)
                    return false;
                IEnumerable<GameFeatureLanguage> newGameFeatures = CreateGameFeaturesLanguages(languageForGameFeatures);
                if (newGameFeatures == null)
                    return false;
                IEnumerable<GameFeatureDetailLanguage> newGameFeatureDetails =
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

        private IEnumerable<GameFeatureDetailLanguage> CreateGameFeaturesDetailsLanguage(Language language)
        {
            var templateGameFeaturesDetails = GetTemplateGameFeatureDetails();
            if (templateGameFeaturesDetails == null)
                return null;
            var newFeatureDetailsLanguages = new List<GameFeatureDetailLanguage>();
            templateGameFeaturesDetails.ToList().ForEach(x =>
            {
                newFeatureDetailsLanguages.Add(new GameFeatureDetailLanguage {GameFeatureDetail = x,GameFeatureDetailName = "",Language = language});
            });
            return newFeatureDetailsLanguages;
        }

        private IEnumerable<GameFeatureDetail> GetTemplateGameFeatureDetails()
        {
            return
                _repository.GameFeatureDetailLanguageRepository.GetWithInclude(x => x.Language.Id == 1)
                    .Select(y => y.GameFeatureDetail);
        } 

        private IEnumerable<GameFeatureLanguage> CreateGameFeaturesLanguages(Language language)
        {
            var templateGameFeatures = GetTemplateGameFeatures();
            if (templateGameFeatures == null)
                return null;
            var newFeaturesLanguages = new List<GameFeatureLanguage>();
            templateGameFeatures.ToList().ForEach(gameFeature =>
            {
                newFeaturesLanguages.Add(new GameFeatureLanguage {GameFeature = gameFeature,Language = language,GameFeatureName = ""});
            });
            return newFeaturesLanguages;
        }

        private IEnumerable<GameFeature> GetTemplateGameFeatures()
        {
            return
                _repository.GameFeatureLanguageRepository.GetWithInclude(x => x.Language.Id == 1)
                    .Select(y => y.GameFeature);
        } 

    }
}
