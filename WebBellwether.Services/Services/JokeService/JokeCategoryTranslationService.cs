using System.Collections.Generic;
using System.Linq;
using WebBellwether.Models.Models.Translation;
using WebBellwether.Repositories.Entities.Translations;
using WebBellwether.Services.Factories;
using WebBellwether.Services.Utility;

namespace WebBellwether.Services.Services.JokeService
{
    public interface IJokeCategoryTranslationService
    {
        List<AvailableLanguage> FillAvailableJokeCategoryTranslation(int jokeCategoryId);
    }
    public class JokeCategoryTranslationService:IJokeCategoryTranslationService
    {
        public List<AvailableLanguage> FillAvailableJokeCategoryTranslation(int jokeCategoryId)
        {
            List<LanguageDao> allLanguages = RepositoryFactory.Context.Languages.ToList();
            var translation =
                RepositoryFactory.Context.JokeCategoryDetails.Where(x => x.JokeCategory.Id == jokeCategoryId).ToList()
                    .Select(
                        z =>
                            new AvailableLanguage
                            {
                                Language = ModelMapper.Map<Language, LanguageDao>(z.Language),
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
