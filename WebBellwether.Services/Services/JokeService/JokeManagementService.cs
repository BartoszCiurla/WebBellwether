using System;
using System.Collections.Generic;
using System.Linq;
using WebBellwether.Models.Models.Translation;
using WebBellwether.Models.Results;
using WebBellwether.Models.ViewModels.Joke;
using WebBellwether.Repositories.Entities.Joke;
using WebBellwether.Repositories.Entities.Translations;
using WebBellwether.Services.Factories;
using WebBellwether.Services.Utility;

namespace WebBellwether.Services.Services.JokeService
{
    public interface IJokeManagementService
    {
        JokeViewModel InsertJoke(JokeViewModel joke);
        bool RemoveJoke(JokeViewModel joke);
        JokeViewModel GetJokeTranslation(int jokeId, int languageId);
        bool PutJoke(JokeViewModel joke);
        JokeViewModel[] GetJokesWithAvailableLanguages(int languageId);
    }
    public class JokeManagementService : IJokeManagementService
    {    
        public JokeViewModel InsertJoke(JokeViewModel joke)
        {
           if(!ValidateInsertJoke(joke))
                throw new Exception(ThrowMessage.JokeNotAdded.ToString());
            return ValidateGetInsertedJoke(joke);
        }
     
        public bool RemoveJoke(JokeViewModel joke)
        {
            return joke.TemporarySeveralTranslationDelete
                ? RemoveAllJokeTranslation(joke)
                : RemoveSelectedJokeTranslation(joke);
        }

        public JokeViewModel GetJokeTranslation(int jokeId, int languageId)
        {
            var entity =
                RepositoryFactory.Context.JokeDetails.FirstOrDefault(
                    x => x.Joke.Id == jokeId && x.Language.Id == languageId);
            if (entity == null)
                throw new Exception(ThrowMessage.JokeTranslationNotExists.ToString());
            JokeViewModel joke = new JokeViewModel { Id = jokeId, JokeContent = entity.JokeContent, LanguageId = entity.Language.Id, JokeId = entity.Id };
            return joke;
        }

        public bool PutJoke(JokeViewModel joke)
        {
            var entity = RepositoryFactory.Context.JokeDetails.FirstOrDefault(x => x.Id == joke.JokeId);
            if (entity == null)
                throw new Exception(ThrowMessage.JokeNotExists.ToString());
            if (entity.JokeCategoryDetail.JokeCategory.Id != joke.JokeCategoryId) // in edit i change category 
            {
                JokeCategoryDetailDao categoryDetail = GetJokeCategory(joke.JokeCategoryId, joke.LanguageId);
                entity.JokeCategoryDetail = categoryDetail;
                foreach (AvailableLanguage al in joke.JokeTranslations)
                {
                    if (al.HasTranslation && al.Language.Id != joke.LanguageId)
                    {
                        JokeCategoryDetailDao categoryDetailForOtherTranslation = GetJokeCategory(joke.JokeCategoryId, al.Language.Id);      
                        var otherTranslationJoke =
                            RepositoryFactory.Context.JokeDetails.FirstOrDefault(x => x.Joke.Id == joke.Id);
                        if (otherTranslationJoke != null)
                            otherTranslationJoke.JokeCategoryDetail = categoryDetailForOtherTranslation;
                    }
                }
            }
            else if (!entity.JokeContent.Equals(joke.JokeContent))
                if (RepositoryFactory.Context.JokeDetails.FirstOrDefault(x => x.JokeContent.Equals(joke.JokeContent)) !=
                    null)
                    throw new Exception(ThrowMessage.JokeExists.ToString());
            entity.JokeContent = joke.JokeContent;
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        public JokeViewModel[] GetJokesWithAvailableLanguages(int languageId)
        {
            var jokes = new List<JokeViewModel>();
            var jokesDao = RepositoryFactory.Context.JokeDetails.Where(x => x.Language.Id == languageId).ToList();
            if (!jokesDao.Any())
                throw new Exception(ThrowMessage.JokeCategoryNotExists.ToString());
            jokesDao.ForEach(x =>
            {
                jokes.Add(new JokeViewModel
                {
                    Id = x.Joke.Id,
                    JokeId = x.Id,
                    LanguageId = x.Language.Id,
                    JokeContent = x.JokeContent,
                    JokeCategoryName = x.JokeCategoryDetail.JokeCategoryName,
                    JokeCategoryId = x.JokeCategoryDetail.JokeCategory.Id,
                    JokeCategoryDetailId = x.JokeCategoryDetail.Id,
                    JokeTranslations = FillAvailableTranslation(x.Joke.Id)
                });
            });
            return jokes.ToArray();
        }

        private List<AvailableLanguage> FillAvailableTranslation(int jokeId)
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

        private JokeViewModel ValidateGetInsertedJoke(JokeViewModel joke)
        {
            var insertedJoke = RepositoryFactory.Context.JokeDetails.FirstOrDefault(x => x.JokeContent.Equals(joke.JokeContent));
            if (insertedJoke == null)
                throw new Exception(ThrowMessage.JokeNotAdded.ToString());
            joke.Id = insertedJoke.Joke.Id;
            joke.JokeId = insertedJoke.Id;
            joke.JokeTranslations = FillAvailableTranslation(joke.Id);
            return joke;
        }
        private bool ValidateInsertJoke(JokeViewModel joke)
        {
            if (IsJokeContentExists(joke))
                throw new Exception(ThrowMessage.JokeExists.ToString());
            return IsMainJokeTranslation(joke)
                ? InsertMainJokeTranslation(joke)
                : InsertAnotherJokeTranslation(joke);
        }

        private JokeCategoryDetailDao GetJokeCategory(int jokeCategoryId, int languageId)
        {
            JokeCategoryDetailDao jokeCategoryDao = 
                RepositoryFactory.Context.JokeCategoryDetails.FirstOrDefault(
                    x => x.JokeCategory.Id == jokeCategoryId && x.Language.Id == languageId);
            if(jokeCategoryDao == null)
                throw new Exception(ThrowMessage.JokeCategoryNotExists + GetLanguage(languageId).LanguageName);
            return jokeCategoryDao;
        }

        private bool InsertMainJokeTranslation(JokeViewModel joke)
        {
            JokeCategoryDetailDao jokeCategory = GetJokeCategory(joke.JokeCategoryId, joke.LanguageId);
            JokeDetailDao entityDetail = new JokeDetailDao
            {
                JokeContent = joke.JokeContent,
                Language = GetLanguage(joke.LanguageId),
                JokeCategoryDetail = jokeCategory
            };
            JokeDao entity = new JokeDao
            {
                CreationDate = DateTime.UtcNow,
                JokeDetails = new List<JokeDetailDao>()
            };
            entity.JokeDetails.Add(entityDetail);
            RepositoryFactory.Context.Jokes.Add(entity);
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private bool InsertAnotherJokeTranslation(JokeViewModel joke)
        {
            if (IsAnotherJokeTranslationToEdit(joke))
                return PutJokeContent(joke);
            LanguageDao lang = GetLanguage(joke.LanguageId);
            JokeCategoryDetailDao categoryDetail = GetJokeCategory(joke.JokeCategoryId, joke.LanguageId);
            var entity = RepositoryFactory.Context.Jokes.FirstOrDefault(x => x.Id == joke.Id);
            entity?.JokeDetails.Add(new JokeDetailDao
            {
                JokeContent = joke.JokeContent,
                Language = lang,
                JokeCategoryDetail = categoryDetail
            });
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private LanguageDao GetLanguage(int languageId)
        {
            LanguageDao languageDao = RepositoryFactory.Context.Languages.FirstOrDefault(x => x.Id == languageId);
            if (languageDao == null)
                throw new Exception(ThrowMessage.LanguageNotExists.ToString());
            return languageDao;
        }

        private bool IsAnotherJokeTranslationToEdit(JokeViewModel joke)
        {
            return joke.JokeId > 0;
        }

        private bool PutJokeContent(JokeViewModel joke)
        {
            var jokeToEdit = RepositoryFactory.Context.JokeDetails.FirstOrDefault(x => x.Id == joke.JokeId);
            if (jokeToEdit == null)
                throw new Exception(ThrowMessage.JokeTranslationNotExists.ToString());
            jokeToEdit.JokeContent = joke.JokeContent;
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private bool IsJokeContentExists(JokeViewModel joke)
        {
            return RepositoryFactory.Context.JokeDetails.Any(x => x.JokeContent == joke.JokeContent);
        }

        private bool IsMainJokeTranslation(JokeViewModel joke)
        {
            return joke.Id == 0;
        }

        private bool RemoveAllJokeTranslation(JokeViewModel joke)
        {
            var jokeDetails = RepositoryFactory.Context.JokeDetails.Where(x => x.Joke.Id == joke.Id);
            if (!jokeDetails.Any())
                throw new Exception(ThrowMessage.JokeDetailNotExists.ToString());
            RepositoryFactory.Context.JokeDetails.RemoveRange(jokeDetails);
            RepositoryFactory.Context.SaveChanges();
            var mainJoke = RepositoryFactory.Context.Jokes.FirstOrDefault(x => x.Id == joke.Id);
            if (mainJoke == null)
                throw new Exception(ThrowMessage.JokeNotExists.ToString());
            RepositoryFactory.Context.Jokes.Remove(mainJoke);
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private bool RemoveSelectedJokeTranslation(JokeViewModel joke)
        {
            var jokeDetail = RepositoryFactory.Context.JokeDetails.FirstOrDefault(x => x.Id == joke.JokeId);
            if (jokeDetail != null)
                RepositoryFactory.Context.JokeDetails.Remove(jokeDetail);
            int jokeTranslationCount = 0;
            if (joke.JokeTranslations == null)
            {
                joke.JokeTranslations?.ForEach(x =>
                {
                    if (x.HasTranslation)
                        jokeTranslationCount++;
                });
            }
            if (jokeTranslationCount == 1)//have only one translation , can delete main id for joke . Safe is safe ...
            {
                var mainJoke = RepositoryFactory.Context.Jokes.FirstOrDefault(x => x.Id == joke.Id);
                if (mainJoke != null)
                    RepositoryFactory.Context.Jokes.Remove(mainJoke);
            }
            RepositoryFactory.Context.SaveChanges();
            return true;
        }
    }
}