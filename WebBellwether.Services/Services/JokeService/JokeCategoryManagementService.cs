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
    public interface IJokeCategoryManagementService
    {
        JokeCategoryViewModel[] GetJokeCategories(int languageId);
        JokeCategoryViewModel GetJokeCategoryTranslation(int jokeCategoryId, int languageId);
        JokeCategoryViewModel InsertJokeCategory(JokeCategoryViewModel jokeCategory);
        JokeCategoryViewModel[] GetJokeCategoriesWithAvailableLanguage(int languageId);
        bool PutJokeCategory(JokeCategoryViewModel jokeCategory);
        bool RemoveJokeCategory(JokeCategoryViewModel jokeCategory);
    }
    public class JokeCategoryManagementService : IJokeCategoryManagementService
    {     
        public JokeCategoryViewModel[] GetJokeCategories(int languageId)
        {
            var result =
                RepositoryFactory.Context.JokeCategoryDetails.Where(x => x.Language.Id == languageId)
                    .Select(z => new JokeCategoryViewModel
                    {
                        Id = z.JokeCategory.Id,
                        JokeCategoryId = z.Id,
                        JokeCategoryName = z.JokeCategoryName,
                        LanguageId = z.Language.Id
                    }).ToArray();
            return result;
        }

        public JokeCategoryViewModel[] GetJokeCategoriesWithAvailableLanguage(int languageId)
        {
            var jokeCategoriesDao =
                RepositoryFactory.Context.JokeCategoryDetails.Where(x => x.Language.Id == languageId).ToList();
            if (!jokeCategoriesDao.Any())
                return null;
            var jokeCategories = new List<JokeCategoryViewModel>();
            jokeCategoriesDao.ForEach(
                x =>
                {
                    jokeCategories.Add(new JokeCategoryViewModel
                    {
                        Id = x.JokeCategory.Id,
                        JokeCategoryId = x.Id,
                        JokeCategoryName = x.JokeCategoryName,
                        LanguageId = x.Language.Id,
                        JokeCategoryTranslations = FillAvailableJokeCategoryTranslation(x.JokeCategory.Id)
                    });
                });
            return jokeCategories.ToArray();
        }

        public JokeCategoryViewModel GetJokeCategoryTranslation(int jokeCategoryId, int languageId)
        {
            var entity =
                RepositoryFactory.Context.JokeCategoryDetails.FirstOrDefault(
                    x => x.JokeCategory.Id == jokeCategoryId && x.Language.Id == languageId);
            if (entity == null)
                return null;
            return new JokeCategoryViewModel { Id = jokeCategoryId, JokeCategoryName = entity.JokeCategoryName, LanguageId = entity.Language.Id, JokeCategoryId = entity.Id };
        }
        public JokeCategoryViewModel InsertJokeCategory(JokeCategoryViewModel jokeCategory)
        {
            if (!ValidateInsertJokeCategory(jokeCategory))
                throw new Exception(ThrowMessage.JokeCategoryNotAdded.ToString());
            return ValidateGetInsertedJokeCategory(jokeCategory);
        }
      
        public bool PutJokeCategory(JokeCategoryViewModel jokeCategory)
        {
            var entity = GetJokeCategoryDetail(jokeCategory.JokeCategoryId);
            if (IsJokeCategoryContentExists(jokeCategory))
                throw new Exception(ThrowMessage.ThisJokeCategoryExists.ToString());
            entity.JokeCategoryName = jokeCategory.JokeCategoryName;
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        public bool RemoveJokeCategory(JokeCategoryViewModel jokeCategory)
        {
            return jokeCategory.TemporarySeveralTranslationDelete
                ? RemoveAllJokeCategoryTranslation(jokeCategory)
                : RemoveSelectedJokeCategoryTranslation(jokeCategory);
        }

        private List<AvailableLanguage> FillAvailableJokeCategoryTranslation(int jokeCategoryId)
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

        private JokeCategoryViewModel ValidateGetInsertedJokeCategory(JokeCategoryViewModel jokeCategory)
        {
            var insertedJokeCategory =
                   RepositoryFactory.Context.JokeCategoryDetails.FirstOrDefault(
                       x => x.JokeCategoryName == jokeCategory.JokeCategoryName);
            if (insertedJokeCategory == null)
                throw new Exception(ThrowMessage.JokeCategoryNotAdded.ToString());
            jokeCategory.Id = insertedJokeCategory.JokeCategory.Id;
            jokeCategory.JokeCategoryId = insertedJokeCategory.Id;
            jokeCategory.JokeCategoryTranslations =FillAvailableJokeCategoryTranslation(jokeCategory.Id);
            return jokeCategory;
        }
        private bool ValidateInsertJokeCategory(JokeCategoryViewModel jokeCategory)
        {
            if (IsJokeCategoryContentExists(jokeCategory))
                throw new Exception(ThrowMessage.ThisJokeCategoryExists.ToString());
            return IsMainJokeCategoryTranslation(jokeCategory) ?
                InsertMainJokeCategoryTranslation(jokeCategory) :
            InsertAnotherJokeCategoryTranslation(jokeCategory);
        }

        private bool IsMainJokeCategoryTranslation(JokeCategoryViewModel jokeCategory)
        {
            return jokeCategory.Id == 0;
        }
        private bool IsJokeCategoryContentExists(JokeCategoryViewModel jokeCategory)
        {
            return
                RepositoryFactory.Context.JokeCategoryDetails.Any(
                    x => x.JokeCategoryName == jokeCategory.JokeCategoryName);
        }

        private bool IsAnotherJokeCategoryTranslationToEdit(JokeCategoryViewModel jokeCategory)
        {
            return jokeCategory.JokeCategoryId > 0;
        }

        private bool PutJokeCategoryContent(JokeCategoryViewModel jokeCategory)
        {
            var jokeCategoryToEdit =
                        RepositoryFactory.Context.JokeCategoryDetails.FirstOrDefault(
                            x => x.Id == jokeCategory.JokeCategoryId);
            if (jokeCategoryToEdit == null)
                throw new Exception(ThrowMessage.JokeCategoryTranslationNotExists.ToString());
            jokeCategoryToEdit.JokeCategoryName = jokeCategory.JokeCategoryName;
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private bool InsertAnotherJokeCategoryTranslation(JokeCategoryViewModel jokeCategory)
        {
            if (IsAnotherJokeCategoryTranslationToEdit(jokeCategory))
                return PutJokeCategoryContent(jokeCategory);
            var entity = GetJokeCategory(jokeCategory.Id);
            entity?.JokeCategoryDetail.Add(new JokeCategoryDetailDao { JokeCategoryName = jokeCategory.JokeCategoryName, Language = GetLanguage(jokeCategory.LanguageId) });
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private JokeCategoryDao GetJokeCategory(int jokeCategoryId)
        {
            return RepositoryFactory.Context.JokeCategories.FirstOrDefault(x => x.Id == jokeCategoryId);
        }

        private JokeCategoryDetailDao GetJokeCategoryDetail(int jokeCategoryId)
        {
            JokeCategoryDetailDao jokeCategoryDetail = RepositoryFactory.Context.JokeCategoryDetails.FirstOrDefault(x => x.Id == jokeCategoryId);
            if (jokeCategoryDetail == null)
                throw new Exception(ThrowMessage.JokeCategoryNotExists.ToString());
            return jokeCategoryDetail;
        }
        private bool InsertMainJokeCategoryTranslation(JokeCategoryViewModel jokeCategory)
        {
            JokeCategoryDao entity = new JokeCategoryDao
            {
                CreationDate = DateTime.UtcNow,
                JokeCategoryDetail = new List<JokeCategoryDetailDao>
                {
                    new JokeCategoryDetailDao
                    {
                        JokeCategoryName = jokeCategory.JokeCategoryName,
                        Language = GetLanguage(jokeCategory.LanguageId)
                    }
                }
            };
            RepositoryFactory.Context.JokeCategories.Add(entity);
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private bool RemoveAllJokeCategoryTranslation(JokeCategoryViewModel jokeCategory)
        {
            var jokeCategoryDetail =
                      RepositoryFactory.Context.JokeCategoryDetails.Where(x => x.JokeCategory.Id == jokeCategory.Id);
            RepositoryFactory.Context.JokeCategoryDetails.RemoveRange(jokeCategoryDetail);
            var mainJokeCategory = GetJokeCategory(jokeCategory.Id);
            if (mainJokeCategory != null)
                RepositoryFactory.Context.JokeCategories.Remove(mainJokeCategory);
            RepositoryFactory.Context.SaveChanges();
            return true;
        }

        private bool RemoveSelectedJokeCategoryTranslation(JokeCategoryViewModel jokeCategory)
        {
            var jokeCategoryDetail = GetJokeCategoryDetail(jokeCategory.JokeCategoryId);
            RepositoryFactory.Context.JokeCategoryDetails.Remove(jokeCategoryDetail);
            int jokeCategoryTranslationCount = 0;
            jokeCategory.JokeCategoryTranslations?.ForEach(x =>
            {
                if (x.HasTranslation)
                    jokeCategoryTranslationCount++;
            });
            if (jokeCategoryTranslationCount == 1)
            {
                var mainJokeCategory = GetJokeCategory(jokeCategory.Id);
                if (mainJokeCategory != null)
                    RepositoryFactory.Context.JokeCategories.Remove(mainJokeCategory);
            }
            RepositoryFactory.Context.SaveChanges();
            return true;
        }
       
        private LanguageDao GetLanguage(int id)
        {
            LanguageDao languageDao = RepositoryFactory.Context.Languages.FirstOrDefault(x => x.Id == id);
            if (languageDao == null)
                throw new Exception(ThrowMessage.LanguageNotExists.ToString());
            return languageDao;
        }
    }
}
