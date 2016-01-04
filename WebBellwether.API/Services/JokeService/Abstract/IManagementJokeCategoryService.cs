using System.Collections.Generic;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Models.Joke;
using WebBellwether.API.Results;

namespace WebBellwether.API.Services.JokeService.Abstract
{
    public interface IManagementJokeCategoryService
    {
        List<JokeCategoryModel> GetJokeCategories(int languageId);
        JokeCategoryModel GetJokeCategoryTranslation(int jokeCategoryId, int languageId);
        ResultStateContainer InsertJokeCategory(JokeCategoryModel jokeCategory);
        ResultStateContainer InsertSeveralLanguageJokeCategory(JokeCategoryModel jokeCategory);
        ResultStateContainer InsertSingleLanguageJokeCategory(JokeCategoryModel jokeCategory);
        ResultStateContainer PutJokeCategory(JokeCategoryModel jokeCategory);
        ResultStateContainer DeleteJokeCategory(JokeCategoryModel jokeCategory);
        LanguageDao GetLanguage(int id);
    }
}
