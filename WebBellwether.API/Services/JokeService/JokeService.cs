using System.Collections.Generic;
using System.Linq;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Models;
using WebBellwether.API.Models.Joke;
using WebBellwether.API.UnitOfWork;
using WebBellwether.API.Results;

namespace WebBellwether.API.Services.JokeService
{
    public class JokeService
    {
        private readonly JokeUnitOfWork _unitOfWork;
        private readonly ManagementJokeCategoryService _managementJokeCategoryService;
        private readonly ManagementJokeService _managementJokeService;

        public JokeService(JokeUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _managementJokeCategoryService = new ManagementJokeCategoryService(unitOfWork);
            _managementJokeService = new ManagementJokeService(unitOfWork);
        }
        public List<JokeModel> GetJokes(int language)
        {
            var jokes = new List<JokeModel>();
            var entity = _unitOfWork.JokeDetailRepository.GetWithInclude(x => x.Language.Id == language).ToList();
            entity.ForEach(x =>
            {
                jokes.Add(new JokeModel
                {
                    Id = x.Joke.Id,
                    LanguageId = x.Language.Id,
                    JokeContent = x.JokeContent,
                    JokeCategoryId = x.JokeCategoryDetail.JokeCategory.Id // global id for cateogry ,good for multilingual 
                });
            });
            return jokes;
        }
        public ResultStateContainer InsertJoke(JokeModel joke)
        {
            ResultStateContainer result = _managementJokeService.InsertJoke(joke);
            if (result.ResultState == ResultState.JokeAdded)
            {
                List<Language> languages = _unitOfWork.LanguageRepository.GetAll().ToList(); 
                var newJoke = _unitOfWork.JokeDetailRepository.GetWithInclude(x => x.JokeContent.Equals(joke.JokeContent)).FirstOrDefault();
                joke.Id = newJoke.Joke.Id;
                joke.JokeId = newJoke.Id;
                joke.JokeTranslations = FillAvailableTranslation(joke.Id, languages);
                result.Value = joke;
                return result;
            }
            else return result;
        }
        public ResultStateContainer DeleteJokeCategory(JokeCategoryModel jokeCategory)
        {
            return _managementJokeCategoryService.DeleteJokeCategory(jokeCategory);
        }
        public JokeCategoryModel GetJokeCategoryTranslation(int jokeCategoryId, int languageId)
        {
            return _managementJokeCategoryService.GetJokeCategoryTranslation(jokeCategoryId, languageId);
        }
        public ResultStateContainer PutJokeCategory(JokeCategoryModel jokeCategory)
        {
            return _managementJokeCategoryService.PutJokeCategory(jokeCategory);
        }
        public List<JokeModel> GetJokesWithAvailableLanguages(int language)
        {
            List<Language> languages = _unitOfWork.LanguageRepository.GetAll().ToList();
            var jokes = new List<JokeModel>();
            var entity = _unitOfWork.JokeDetailRepository.GetWithInclude(x => x.Language.Id == language).ToList();
            entity.ForEach(x =>
            {
                jokes.Add(new JokeModel
                {
                    Id = x.Joke.Id, // global id
                    JokeId = x.Id, // id for translation
                    LanguageId = x.Language.Id,
                    JokeContent = x.JokeContent,
                    JokeCategoryName = x.JokeCategoryDetail.JokeCategoryName,
                    JokeCategoryId = x.JokeCategoryDetail.JokeCategory.Id, // global id
                    JokeCategoryDetailId = x.JokeCategoryDetail.Id, // id for translation
                    JokeTranslations = FillAvailableTranslation(x.Joke.Id, languages)
                });
            });
            return jokes;
        }
        public List<AvailableLanguage> FillAvailableTranslation(int jokeId, List<Language> allLanguages)
        {
            var translation = new List<AvailableLanguage>();
            _unitOfWork.JokeDetailRepository.GetWithInclude(x => x.Joke.Id == jokeId).ToList().ForEach(z => translation.Add(new AvailableLanguage { Language = z.Language, HasTranslation = true }));
            allLanguages.ForEach(x =>
            {
                if (translation.FirstOrDefault(y => y.Language.Id == x.Id) == null)
                    translation.Add(new AvailableLanguage { Language = x, HasTranslation = false });
            });
            return translation;
        }
        public ResultStateContainer InsertJokeCategory(JokeCategoryModel categoryModel)
        {
            ResultStateContainer result = _managementJokeCategoryService.InsertJokeCategory(categoryModel);
            if (result.ResultState == ResultState.JokeCategoryAdded)
            {
                var newJokeCategory = _unitOfWork.JokeCategoryDetailRepository.GetWithInclude(x => x.JokeCategoryName == categoryModel.JokeCategoryName).FirstOrDefault();
                categoryModel.Id = newJokeCategory.JokeCategory.Id;
                categoryModel.JokeCategoryId = newJokeCategory.Id;
                categoryModel.JokeCategoryTranslations = FillAvailableJokeCategoryTranslation(categoryModel.Id, _unitOfWork.LanguageRepository.GetAll().ToList());
                result.Value = categoryModel;
                return result;
            }
            else
                return result;
        }
        public List<JokeCategoryModel> GetJokeCategories(int languageId)
        {
            return _managementJokeCategoryService.GetJokeCategories(languageId);
        }
        public List<JokeCategoryModel> GetJokeCategoriesWithAvailableLanguage(int language)
        {
            List<Language> languages = _unitOfWork.LanguageRepository.GetAll().ToList();
            var jokeCategories = new List<JokeCategoryModel>();
            var entity = _unitOfWork.JokeCategoryDetailRepository.GetWithInclude(x => x.Language.Id == language).ToList();
            entity.ForEach(x =>
            {
                jokeCategories.Add(new JokeCategoryModel
                {
                    Id = x.JokeCategory.Id, // global id
                    JokeCategoryId = x.Id,
                    JokeCategoryName = x.JokeCategoryName,
                    LanguageId = x.Language.Id,
                    JokeCategoryTranslations = FillAvailableJokeCategoryTranslation(x.JokeCategory.Id, languages)
                });
            });
            return jokeCategories;
        }
        public List<AvailableLanguage> FillAvailableJokeCategoryTranslation(int jokeCategoryId, List<Language> allLanguages)
        {
            var translation = new List<AvailableLanguage>();
            _unitOfWork.JokeCategoryDetailRepository.GetWithInclude(x => x.JokeCategory.Id == jokeCategoryId).ToList().ForEach(z => translation.Add(new AvailableLanguage { Language = z.Language, HasTranslation = true }));
            allLanguages.ForEach(x =>
            {
                if (translation.FirstOrDefault(y => y.Language.Id == x.Id) == null)
                    translation.Add(new AvailableLanguage { Language = x, HasTranslation = false });
            });
            return translation;
        }
    }
}
