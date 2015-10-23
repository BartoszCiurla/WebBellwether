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
            if (joke.JokeId == 0)//new joke
               return InsertSingleLanguageJoke(joke);
            return InsertSeveralLanguageJoke(joke);//joke translation
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
                if(entityDetail.Language == null)
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
        public JokeCategoryDetail GetJokeCategory(int jokeCategory,int languageId)
        {
           return _unitOfWork.JokeCategoryDetailRepository.GetWithInclude(x => x.JokeCategory.Id == jokeCategory && x.Language.Id == languageId).FirstOrDefault();
        }
        public ResultStateContainer InsertSeveralLanguageJoke(JokeModel joke)
        {
            return null;
        }
        //This function is often duplicated it a little disturbing
        public Language GetLanguage(int id)
        {
            return _unitOfWork.LanguageRepository.Get(x => x.Id == id);
        }
    }
}