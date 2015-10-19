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

        public JokeService(JokeUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<JokeModel> GetJokes(int language)
        {
            var jokes = new List<JokeModel>();
            var entity = _unitOfWork.JokeDetailRepository.GetWithInclude(x => x.Language.Id == language).ToList();
            entity.ForEach(x =>
            {
                jokes.Add(new JokeModel
                {
                    Id=x.Joke.Id,
                    Language = x.Language,
                    JokeContent = x.JokeContent,
                    JokeCategoryId = x.JokeCategoryDetail.JokeCategory.Id // global id for cateogry ,good for multilingual 
                });
            });
            return jokes;
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
                    Language = x.Language,
                    JokeContent = x.JokeContent,
                    JokeCategoryName = x.JokeCategoryDetail.JokeCategoryName,
                    JokeCategoryId = x.JokeCategoryDetail.JokeCategory.Id, // global id
                    JokeCategoryDetailId = x.JokeCategoryDetail.Id, // id for translation
                    JokeTranslations = FillAvailableTranslation(x.Joke.Id, languages)
                });
            });
            return jokes;
        }
        public List<AvailableLanguage> FillAvailableTranslation(int jokeId,List<Language> allLanguages)
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
    }
}
