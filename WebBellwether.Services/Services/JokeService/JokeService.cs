using System.Collections.Generic;
using System.Linq;
using WebBellwether.Models.Models.Joke;
using WebBellwether.Models.Models.Translation;
using WebBellwether.Models.Results;
using WebBellwether.Repositories.Entities.Translations;
using WebBellwether.Services.Factories;

namespace WebBellwether.Services.Services.JokeService
{
    public interface IJokeService
    {
        List<JokeModel> GetJokes(int language);
        ResultStateContainer PutJoke(JokeModel joke);
        ResultStateContainer InsertJoke(JokeModel joke);
        JokeModel GetJokeTranslation(int jokeId, int languageId);
        ResultStateContainer DeleteJoke(JokeModel joke);
        ResultStateContainer DeleteJokeCategory(JokeCategoryModel jokeCategory);
        JokeCategoryModel GetJokeCategoryTranslation(int jokeCategoryId, int languageId);
        ResultStateContainer PutJokeCategory(JokeCategoryModel jokeCategory);
        List<JokeModel> GetJokesWithAvailableLanguages(int language);
        List<AvailableLanguage> FillAvailableTranslation(int jokeId, List<LanguageDao> allLanguages);
        ResultStateContainer InsertJokeCategory(JokeCategoryModel categoryModel);
        List<JokeCategoryModel> GetJokeCategories(int languageId);
        List<JokeCategoryModel> GetJokeCategoriesWithAvailableLanguage(int language);
        List<AvailableLanguage> FillAvailableJokeCategoryTranslation(int jokeCategoryId, List<LanguageDao> allLanguages);
    }
    public class JokeService : IJokeService
    {
        private readonly IManagementJokeCategoryService _managementJokeCategoryService;
        private readonly IManagementJokeService _managementJokeService;

        public JokeService()
        {
            _managementJokeCategoryService = new ManagementJokeCategoryService();
            _managementJokeService = new ManagementJokeService();
        }
        public List<JokeModel> GetJokes(int language)
        {
            return
                RepositoryFactory.Context.JokeDetails.Where(x => x.Language.Id == language).Select(x => new JokeModel
                {
                    Id = x.Joke.Id,
                    LanguageId = x.Language.Id,
                    JokeContent = x.JokeContent,
                    JokeCategoryId = x.JokeCategoryDetail.JokeCategory.Id
                }).ToList();
        }
        public ResultStateContainer PutJoke(JokeModel joke)
        {
            return _managementJokeService.PutJoke(joke);
        }
        public ResultStateContainer InsertJoke(JokeModel joke)
        {
            ResultStateContainer result = _managementJokeService.InsertJoke(joke);
            if (result.ResultState == ResultState.Success)
            {
                List<LanguageDao> languages = RepositoryFactory.Context.Languages.ToList();
                var newJoke =
                    RepositoryFactory.Context.JokeDetails.FirstOrDefault(x => x.JokeContent.Equals(joke.JokeContent));
                if (newJoke != null)
                {
                    joke.Id = newJoke.Joke.Id;
                    joke.JokeId = newJoke.Id;
                    joke.JokeTranslations = FillAvailableTranslation(joke.Id, languages);
                }
                result.ResultValue = joke;
                return result;
            }
            return result;
        }
        public JokeModel GetJokeTranslation(int jokeId, int languageId)
        {
            return _managementJokeService.GetJokeTranslation(jokeId, languageId);
        }
        public ResultStateContainer DeleteJoke(JokeModel joke)
        {
            return _managementJokeService.DeleteJoke(joke);
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
            List<LanguageDao> languages = RepositoryFactory.Context.Languages.ToList();
            var jokes = new List<JokeModel>();
            var jokesDao = RepositoryFactory.Context.JokeDetails.Where(x => x.Language.Id == language).ToList();
            if (!jokesDao.Any())
                return null;
            jokesDao.ForEach(x =>
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
        public List<AvailableLanguage> FillAvailableTranslation(int jokeId, List<LanguageDao> allLanguages)
        {
            var translation =
                RepositoryFactory.Context.JokeDetails.Where(x => x.Joke.Id == jokeId)
                    .Select(
                        x =>
                            new AvailableLanguage
                            {
                                Language =
                                    new Language
                                    {
                                        Id = x.Language.Id,
                                        IsPublic = x.Language.IsPublic,
                                        LanguageName = x.Language.LanguageName,
                                        LanguageShortName = x.Language.LanguageShortName
                                    },
                                HasTranslation = true
                            }).ToList();
            allLanguages.ForEach(x =>
            {
                if (translation.FirstOrDefault(y => y.Language.Id == x.Id) == null)
                    translation.Add(new AvailableLanguage { Language = new Language { Id = x.Id, IsPublic = x.IsPublic, LanguageName = x.LanguageName, LanguageShortName = x.LanguageShortName }, HasTranslation = false });
            });
            return translation;
        }
        public ResultStateContainer InsertJokeCategory(JokeCategoryModel categoryModel)
        {
            ResultStateContainer result = _managementJokeCategoryService.InsertJokeCategory(categoryModel);
            if (result.ResultState == ResultState.Success)
            {
                var newJokeCategory =
                    RepositoryFactory.Context.JokeCategoryDetails.FirstOrDefault(
                        x => x.JokeCategoryName == categoryModel.JokeCategoryName);
                if (newJokeCategory != null)
                {
                    categoryModel.Id = newJokeCategory.JokeCategory.Id;
                    categoryModel.JokeCategoryId = newJokeCategory.Id;
                }
                categoryModel.JokeCategoryTranslations = FillAvailableJokeCategoryTranslation(categoryModel.Id, RepositoryFactory.Context.Languages.ToList());
                result.ResultValue = categoryModel;
                return result;
            }
            return result;
        }
        public List<JokeCategoryModel> GetJokeCategories(int languageId)
        {
            return _managementJokeCategoryService.GetJokeCategories(languageId);
        }
        public List<JokeCategoryModel> GetJokeCategoriesWithAvailableLanguage(int language)
        {
            List<LanguageDao> languages = RepositoryFactory.Context.Languages.ToList();
            var jokeCategoriesDao =
                RepositoryFactory.Context.JokeCategoryDetails.Where(x => x.Language.Id == language).ToList();
            if (!jokeCategoriesDao.Any())
                return null;
            var jokeCategories = new List<JokeCategoryModel>();
            jokeCategoriesDao.ForEach(
                x =>
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
        public List<AvailableLanguage> FillAvailableJokeCategoryTranslation(int jokeCategoryId, List<LanguageDao> allLanguages)
        {
            var translation =
                RepositoryFactory.Context.JokeCategoryDetails.Where(x => x.JokeCategory.Id == jokeCategoryId)
                    .Select(
                        z =>
                            new AvailableLanguage
                            {
                                Language =
                                    new Language
                                    {
                                        Id = z.Language.Id,
                                        IsPublic = z.Language.IsPublic,
                                        LanguageName = z.Language.LanguageName,
                                        LanguageShortName = z.Language.LanguageShortName
                                    },
                                HasTranslation = true
                            }).ToList();
            allLanguages.ForEach(x =>
            {
                if (translation.FirstOrDefault(y => y.Language.Id == x.Id) == null)
                    translation.Add(new AvailableLanguage { Language = new Language { Id = x.Id, IsPublic = x.IsPublic, LanguageName = x.LanguageName, LanguageShortName = x.LanguageShortName }, HasTranslation = false });
            });
            return translation;
        }
    }
}
