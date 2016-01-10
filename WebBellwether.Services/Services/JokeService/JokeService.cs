using System.Linq;
using WebBellwether.Models.ViewModels.Joke;
using WebBellwether.Services.Factories;

namespace WebBellwether.Services.Services.JokeService
{
    public interface IJokeService
    {
        JokeViewModel[] GetJokes(int languageId);
    }
    public class JokeService : IJokeService
    {
        public JokeViewModel[] GetJokes(int language)
        {
            return
                RepositoryFactory.Context.JokeDetails.Where(x => x.Language.Id == language).Select(x => new JokeViewModel
                {
                    Id = x.Joke.Id,
                    LanguageId = x.Language.Id,
                    JokeContent = x.JokeContent,
                    JokeCategoryId = x.JokeCategoryDetail.JokeCategory.Id
                }).ToArray();
        }         
    }
}
