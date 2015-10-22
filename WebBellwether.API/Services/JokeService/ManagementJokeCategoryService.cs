using System.Collections.Generic;
using System.Linq;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Models;
using WebBellwether.API.Entities.Joke;
using WebBellwether.API.Models.Joke;
using WebBellwether.API.UnitOfWork;
using WebBellwether.API.Results;
using System;

namespace WebBellwether.API.Services.JokeService
{
    public class ManagementJokeCategoryService
    {
        private readonly JokeUnitOfWork _unitOfWork;
        public ManagementJokeCategoryService(JokeUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public JokeCategoryModel GetJokeCategoryTranslation(int jokeCategoryId, int languageId)
        {
            var entity = _unitOfWork.JokeCategoryDetailRepository.GetWithInclude(x => x.JokeCategory.Id == jokeCategoryId && x.Language.Id == languageId).FirstOrDefault();
            if (entity == null)
                return null;
            JokeCategoryModel jokeCategory = new JokeCategoryModel { Id = jokeCategoryId, JokeCategoryName = entity.JokeCategoryName, LanguageId = entity.Language.Id, JokeCategoryId = entity.Id };
            return jokeCategory;
        }
        public ResultStateContainer InsertJokeCategory(JokeCategoryModel jokeCategory)
        {
            if (_unitOfWork.JokeCategoryDetailRepository.GetFirst(x => x.JokeCategoryName.Equals(jokeCategory.JokeCategoryName)) != null)
                return new ResultStateContainer { ResultState = ResultState.JokeCategoryExistsInDb };
            if (jokeCategory.Id == 0)
                return InsertSingleLanguageJokeCategory(jokeCategory);
            return InsertSeveralLanguageJokeCategory(jokeCategory);
        }
        public ResultStateContainer InsertSeveralLanguageJokeCategory(JokeCategoryModel jokeCategory)
        {
            try
            {
                if(jokeCategory.JokeCategoryId != 0)
                {
                    var entityToEdit = _unitOfWork.JokeCategoryDetailRepository.GetWithInclude(x => x.Id == jokeCategory.JokeCategoryId).FirstOrDefault();
                    if (entityToEdit == null)
                        return new ResultStateContainer { ResultState = ResultState.JokeCategoryTranslationNotExists };
                    entityToEdit.JokeCategoryName = jokeCategory.JokeCategoryName;
                    _unitOfWork.Save();
                    return new ResultStateContainer { ResultState = ResultState.JokeCategoryTranslationEdited };
                }
                if (_unitOfWork.JokeCategoryDetailRepository.GetFirst(x => x.JokeCategoryName.Equals(jokeCategory.JokeCategoryName)) != null)
                    return new ResultStateContainer { ResultState = ResultState.JokeCategoryExistsInDb };
                var entity = _unitOfWork.JokeCategoryRepository.GetWithInclude(x => x.Id == jokeCategory.Id).FirstOrDefault();
                entity?.JokeCategoryDetail.Add(new JokeCategoryDetail { JokeCategoryName = jokeCategory.JokeCategoryName, Language = _unitOfWork.LanguageRepository.GetFirst(x => x.Id == jokeCategory.LanguageId) });
                _unitOfWork.Save();
                return new ResultStateContainer { ResultState = ResultState.JokeCategoryTranslationAdded };
            }
            catch(Exception e)
            {
                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
            }
        }
        public ResultStateContainer InsertSingleLanguageJokeCategory(JokeCategoryModel jokeCategory)
        {
            try
            {
                JokeCategory entity = new JokeCategory
                {
                    CreationDate = DateTime.UtcNow,
                    JokeCategoryDetail = new List<JokeCategoryDetail>
                    {
                        new JokeCategoryDetail
                        {
                            JokeCategoryName = jokeCategory.JokeCategoryName,
                            Language = GetLanguage(jokeCategory.LanguageId)
                        }
                    }
                };
                _unitOfWork.JokeCategoryRepository.Insert(entity);
                _unitOfWork.Save();

                return new ResultStateContainer { ResultState = ResultState.JokeCategoryAdded, Value = jokeCategory.JokeCategoryName };
            }
            catch (Exception e)
            {
                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
            }
        }
        public ResultStateContainer PutJokeCategory(JokeCategoryModel jokeCategory)
        {
            try
            {
                var entity = _unitOfWork.JokeCategoryDetailRepository.GetWithInclude(x => x.Id == jokeCategory.JokeCategoryId).FirstOrDefault();
                if (entity == null)
                    return new ResultStateContainer { ResultState = ResultState.JokeCategoryNotExistsInDb };
                if (_unitOfWork.JokeCategoryDetailRepository.GetFirst(x => x.JokeCategoryName.Equals(jokeCategory.JokeCategoryName)) != null)
                    return new ResultStateContainer { ResultState = ResultState.ThisJokeCategoryExists };
                entity.JokeCategoryName = jokeCategory.JokeCategoryName;
                _unitOfWork.Save();
                return new ResultStateContainer { ResultState = ResultState.JokeCategoryEdited };
            }
            catch (Exception e)
            {
                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
            }
        }
        public ResultStateContainer DeleteJokeCategory(JokeCategoryModel jokeCategory)
        {
            try
            {
                //single translation
                if (jokeCategory.TemporarySeveralTranslationDelete)
                {
                    var jokeCategoryDetail = _unitOfWork.JokeCategoryDetailRepository.GetWithInclude(x => x.JokeCategory.Id == jokeCategory.Id);
                    if (jokeCategoryDetail != null)
                        jokeCategoryDetail.ToList().ForEach(x =>
                        {
                            _unitOfWork.JokeCategoryDetailRepository.Delete(x);
                        });
                    var mainJokeCategory = _unitOfWork.JokeCategoryRepository.GetWithInclude(x => x.Id == jokeCategory.Id).FirstOrDefault();
                    if (mainJokeCategory != null)
                        _unitOfWork.JokeCategoryRepository.Delete(mainJokeCategory);
                    _unitOfWork.Save();
                    return new ResultStateContainer { ResultState = ResultState.JokeCategoryDeleted };

                }
                //all translation
                else
                {
                    var jokeCategoryDetail = _unitOfWork.JokeCategoryDetailRepository.GetWithInclude(x => x.Id == jokeCategory.JokeCategoryId).FirstOrDefault();
                    if (jokeCategoryDetail == null)
                        return new ResultStateContainer { ResultState = ResultState.JokeCategoryTranslationNotExists };
                    _unitOfWork.JokeCategoryDetailRepository.Delete(jokeCategoryDetail);
                    int jokeCategoryTranslationCount = 0;
                    if (jokeCategory.JokeCategoryTranslations != null)
                        jokeCategory.JokeCategoryTranslations.ForEach(x =>
                        {
                            if (x.HasTranslation)
                                jokeCategoryTranslationCount++;
                        });
                    if (jokeCategoryTranslationCount == 1)//have only one translation , can delete main id for jokecategory . Safe is safe ...
                    {
                        var mainJokeCategory = _unitOfWork.JokeCategoryRepository.GetWithInclude(x => x.Id == jokeCategory.Id).FirstOrDefault();
                        if (mainJokeCategory != null)
                            _unitOfWork.JokeCategoryRepository.Delete(mainJokeCategory);
                    }
                    _unitOfWork.Save();
                    return new ResultStateContainer { ResultState = ResultState.JokeCategoryDeleted };

                }
            }
            catch (Exception e)
            {
                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
            }
        }

        public Language GetLanguage(int id)
        {
            return _unitOfWork.LanguageRepository.Get(x => x.Id == id);
        }

    }
}
