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
    public class ManagementJokeService
    {
        private readonly JokeUnitOfWork _unitOfWork;
        public ManagementJokeService(JokeUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ResultStateContainer InsertJoke(JokeModel joke)
        {
            if (_unitOfWork.JokeDetailRepository.GetFirst(x => x.JokeContent.Equals(joke.JokeContent)) != null)
                //Such verification is very feeble outright stupid, but I do not have time for anything better
                return new ResultStateContainer { ResultState = ResultState.JokeExists };
            if (joke.Id == 0)//new joke
                return InsertSingleLanguageJoke(joke);
            return InsertSeveralLanguageJoke(joke);//joke translation
        }
        public ResultStateContainer DeleteJoke(JokeModel joke)
        {
            try
            {
                //all translation
                if (joke.TemporarySeveralTranslationDelete)
                {
                    var jokeDetail = _unitOfWork.JokeDetailRepository.GetWithInclude(x => x.Joke.Id == joke.Id);
                    if (jokeDetail == null)
                        return new ResultStateContainer { ResultState = ResultState.JokeDetailNotExists };
                    jokeDetail.ToList().ForEach(x =>
                    {
                        _unitOfWork.JokeDetailRepository.Delete(x);
                    });
                    var mainJoke = _unitOfWork.JokeRepository.GetWithInclude(x => x.Id == joke.Id).FirstOrDefault();
                    if (mainJoke == null)
                        return new ResultStateContainer { ResultState = ResultState.JokeNotExists};
                    _unitOfWork.JokeRepository.Delete(mainJoke);
                    _unitOfWork.Save();
                    return new ResultStateContainer { ResultState = ResultState.JokeDeleted };
                }
                else
                {
                    var jokeDetail = _unitOfWork.JokeDetailRepository.GetWithInclude(x => x.Id == joke.JokeId).FirstOrDefault();
                    if (jokeDetail != null)
                        _unitOfWork.JokeDetailRepository.Delete(jokeDetail);
                    int jokeTranslationCount = 0;
                    if (joke.JokeTranslations != null)
                        joke.JokeTranslations.ForEach(x =>
                        {
                            if (x.HasTranslation)
                                jokeTranslationCount++;
                        });
                    if (jokeTranslationCount == 1)//have only one translation , can delete main id for joke . Safe is safe ...
                    {
                        var mainJoke = _unitOfWork.JokeRepository.GetWithInclude(x => x.Id == joke.Id).FirstOrDefault();
                        if (mainJoke != null)
                            _unitOfWork.JokeRepository.Delete(mainJoke);
                    }
                    _unitOfWork.Save();
                    return new ResultStateContainer { ResultState = ResultState.JokeDeleted };
                }


            }
            catch (Exception e)
            {
                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
            }
        }

        public JokeModel GetJokeTranslation(int jokeId,int languageId)
        {
            var entity = _unitOfWork.JokeDetailRepository.GetWithInclude(x => x.Joke.Id == jokeId && x.Language.Id == languageId).FirstOrDefault();
            if (entity == null)
                return null;
            JokeModel joke = new JokeModel { Id = jokeId, JokeContent = entity.JokeContent, LanguageId = entity.Language.Id, JokeId = entity.Id };
            return joke;
        }

        public ResultStateContainer PutJoke(JokeModel joke)
        {
            try
            {
                //ta czesc zawsze tworzy problemy,
                //aktualny to zmiana samej kategorii 
                //dodatkowo wypadało by poświęcić jeden dzień na reforge i ten dzień nastąpi 26.10.2015 po pracy 
                var entity = _unitOfWork.JokeDetailRepository.GetWithInclude(x => x.Id == joke.JokeId).FirstOrDefault();
                if (entity == null)
                    return new ResultStateContainer { ResultState = ResultState.JokeNotExists };
                if (_unitOfWork.JokeDetailRepository.GetFirst(x => x.JokeContent.Equals(joke.JokeContent)) != null)
                    return new ResultStateContainer { ResultState = ResultState.JokeExists };
                if (entity.JokeCategoryDetail.Id != joke.JokeCategoryDetailId) // in edit i change category 
                {

                    JokeCategoryDetail categoryDetail = GetJokeCategory(joke.JokeCategoryDetailId, joke.LanguageId);
                    if (categoryDetail == null)
                        return new ResultStateContainer { ResultState = ResultState.JokeCategoryNotExistsInDb,Value = GetLanguage(joke.LanguageId) };
                    entity.JokeCategoryDetail = categoryDetail;
                    foreach (AvailableLanguage al in joke.JokeTranslations)
                    {
                        if (al.HasTranslation && al.Language.Id != joke.LanguageId)
                        {
                            JokeCategoryDetail categoryDetailForOtherTranslation = GetJokeCategory(joke.JokeCategoryDetailId, al.Language.Id);
                            if (categoryDetailForOtherTranslation == null)
                                return new ResultStateContainer { ResultState = ResultState.JokeCategoryNotExistsInDb, Value = al.Language };
                            var otherTranslationJoke = _unitOfWork.JokeDetailRepository.GetWithInclude(x => x.Joke.Id == joke.Id).FirstOrDefault();
                            otherTranslationJoke.JokeCategoryDetail = categoryDetailForOtherTranslation;
                        }
                            
                    }
                }
                entity.JokeContent = joke.JokeContent;
                _unitOfWork.Save();
                return new ResultStateContainer { ResultState = ResultState.JokeEdited };
            }
            catch ( Exception e)
            {

                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
            }
        }

        public ResultStateContainer InsertSingleLanguageJoke(JokeModel joke)
        {
            try
            {
                //first i check joke category 
                JokeCategoryDetail entityCategory = GetJokeCategory(joke.JokeCategoryId, joke.LanguageId);
                if (entityCategory == null)
                    return new ResultStateContainer { ResultState = ResultState.JokeCategoryNotExistsInDb };
                JokeDetail entityDetail = new JokeDetail
                {
                    JokeContent = joke.JokeContent,
                    Language = GetLanguage(joke.LanguageId),
                    JokeCategoryDetail = entityCategory
                };
                if (entityDetail.Language == null)
                    return new ResultStateContainer { ResultState = ResultState.LanguageNotExists };
                Joke entity = new Joke
                {
                    CreationDate = DateTime.UtcNow,
                    JokeDetails = new List<JokeDetail>()
                };
                entity.JokeDetails.Add(entityDetail);
                _unitOfWork.JokeRepository.Insert(entity);
                _unitOfWork.Save();
                return new ResultStateContainer { ResultState = ResultState.JokeAdded };
            }
            catch (Exception e)
            {
                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
            }
        }
        public JokeCategoryDetail GetJokeCategory(int jokeCategory, int languageId)
        {
            return _unitOfWork.JokeCategoryDetailRepository.GetWithInclude(x => x.JokeCategory.Id == jokeCategory && x.Language.Id == languageId).FirstOrDefault();
        }
        public ResultStateContainer InsertSeveralLanguageJoke(JokeModel joke)
        {
            try
            {
                if(joke.JokeId != 0)
                {
                    var entityToEdit = _unitOfWork.JokeDetailRepository.GetWithInclude(x => x.Id == joke.JokeId).FirstOrDefault();
                    if (entityToEdit == null)
                        return new ResultStateContainer { ResultState = ResultState.JokeTranslationNotExists };
                    entityToEdit.JokeContent = joke.JokeContent;
                    _unitOfWork.Save();
                    return new ResultStateContainer { ResultState = ResultState.JokeTranslationEdited };
                }
                if (_unitOfWork.JokeDetailRepository.GetFirst(x => x.JokeContent.Equals(joke.JokeContent)) != null)
                    return new ResultStateContainer { ResultState = ResultState.JokeExists };                
                Language lang = GetLanguage(joke.LanguageId);
                if (lang == null)
                    return new ResultStateContainer { ResultState = ResultState.LanguageNotExists };
                JokeCategoryDetail categoryDetail = GetJokeCategory(joke.JokeCategoryId, joke.LanguageId);
                if (categoryDetail == null)
                    return new ResultStateContainer { ResultState = ResultState.JokeCategoryNotExistsInDb };                
                var entity = _unitOfWork.JokeRepository.GetWithInclude(x => x.Id == joke.Id).FirstOrDefault();
                entity?.JokeDetails.Add(new JokeDetail
                {
                    JokeContent = joke.JokeContent,
                    Language = lang,
                    JokeCategoryDetail = categoryDetail
                });
                _unitOfWork.Save();
                return new ResultStateContainer { ResultState = ResultState.JokeAdded };

            }
            catch (Exception e)
            {

                return new ResultStateContainer { ResultState = ResultState.Error, Value = e };
            }
        }
        //This function is often duplicated it a little disturbing
        public Language GetLanguage(int id)
        {
            return _unitOfWork.LanguageRepository.Get(x => x.Id == id);
        }
    }
}