using WebBellwether.Models.Models.Joke;
using WebBellwether.Models.Results;
using WebBellwether.Repositories.Entities.Joke;
using WebBellwether.Repositories.Entities.Translations;

namespace WebBellwether.Services.Services.JokeService.Abstract
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
