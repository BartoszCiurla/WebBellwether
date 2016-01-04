using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Entities.Joke;
using WebBellwether.API.Models.Joke;
using WebBellwether.API.Results;

namespace WebBellwether.API.Services.JokeService.Abstract
{
    public interface IManagementJokeService
    {
        ResultStateContainer InsertJoke(JokeModel joke);
        ResultStateContainer DeleteJoke(JokeModel joke);
        JokeModel GetJokeTranslation(int jokeId, int languageId);
        ResultStateContainer PutJoke(JokeModel joke);
        ResultStateContainer InsertSingleLanguageJoke(JokeModel joke);
        JokeCategoryDetailDao GetJokeCategory(int jokeCategory, int languageId);
        ResultStateContainer InsertSeveralLanguageJoke(JokeModel joke);
        LanguageDao GetLanguage(int id);
    }
}
