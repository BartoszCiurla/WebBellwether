using System.Collections.Generic;
using WebBellwether.Models.Models.Joke;
using WebBellwether.Models.Results;
using WebBellwether.Repositories.Entities.Translations;

namespace WebBellwether.Services.Services.JokeService.Abstract
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
