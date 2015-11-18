using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Results;
using WebBellwether.API.Models.Translation;
using System.IO;
using Newtonsoft.Json;
using WebBellwether.API.Repositories.Abstract;
using Newtonsoft.Json.Linq;

namespace WebBellwether.API.Services.LanguageService
{
    public class DeleteLanguageStructureService
    {
        private IAggregateRepositories _repository;
        public DeleteLanguageStructureService(IAggregateRepositories repository)
        {
            _repository = repository;
        }
        public ResultStateContainer DeleteIntegrationGame(int languageId)
        {
            try
            {
                return new ResultStateContainer { ResultState = ResultState.Error};
                //game feature detail languages -> gamefeaturedetails


                //gamefeaturelanguages -> gamefeature 



                //integrationgamefeatures

                //integrationgamedetails


            }
            catch (Exception e)
            {

                return new ResultStateContainer { ResultState = ResultState.Error,Value = e };
            }
        }
    }
}