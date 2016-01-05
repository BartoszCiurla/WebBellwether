using System.Collections.Generic;
using WebBellwether.Models.Models.Joke;
using WebBellwether.Models.Models.Translation;
using WebBellwether.Models.Results;
using WebBellwether.Repositories.Entities.Translations;

namespace WebBellwether.Services.Services.JokeService.Abstract
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
}
