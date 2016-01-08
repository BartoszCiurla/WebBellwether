using System.Collections.Generic;
using System.Linq;
using WebBellwether.Models.Models.Translation;
using WebBellwether.Repositories.Entities.Translations;
using WebBellwether.Services.Factories;
using WebBellwether.Services.Utility;

namespace WebBellwether.Services.Services.JokeService
{
    public interface IJokeTranslationService
    {
        List<AvailableLanguage> FillAvailableTranslation(int jokeId);
    }
    public class JokeTranslationService:IJokeTranslationService
    {
        public List<AvailableLanguage> FillAvailableTranslation(int jokeId)
        {
            List<LanguageDao> allLanguages = RepositoryFactory.Context.Languages.ToList();
            var translation =
                RepositoryFactory.Context.JokeDetails.Where(x => x.Joke.Id == jokeId).ToList()
                    .Select(
                        x =>
                            new AvailableLanguage
                            {
                                Language = ModelMapper.Map<Language, LanguageDao>(x.Language),
                                HasTranslation = true
                            }).ToList();
            allLanguages.ForEach(x =>
            {
                if (translation.FirstOrDefault(y => y.Language.Id == x.Id) == null)
                    translation.Add(new AvailableLanguage { Language = ModelMapper.Map<Language, LanguageDao>(x), HasTranslation = false });
            });
            return translation;
        }
    }
}
