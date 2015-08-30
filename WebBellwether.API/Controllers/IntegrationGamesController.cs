using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using WebBellwether.API.Context;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Models;
using WebBellwether.API.Models.IntegrationGame;
using WebBellwether.API.Repositories;
using WebBellwether.API.Results;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/IntegrationGames")]
    public class IntegrationGamesController : ApiController
    {
        private readonly EfDbContext _ctx = new EfDbContext();
        private IntegrationGamesRepository _repo;
        public IntegrationGamesController()
        {
            _repo = new IntegrationGamesRepository(_ctx);
        }

        [System.Web.Http.AllowAnonymous]
        [Route("GetGameFeatureDetails")]
        public IHttpActionResult GetGameFeatureDetails(int language)
        {
            return Ok(_repo.GetGameFeatureDetails(language));
        }

        [System.Web.Http.Authorize]
        [Route("PostIntegrationGame")]
        public IHttpActionResult PostIntegrationGame(NewIntegrationGameModel gameModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //this is stupid but works ehm this don't work i must return id ffss 
            ResultStateContainer result = _repo.InsertIntegrationGame(gameModel);
            switch (result.ResultState)
            {
                case ResultState.GameAdded:
                    return Ok(result.Value);//standard 
                case ResultState.ThisGameExistsInDb:
                    return BadRequest(result.Value.ToString());//if game name exists in db
                case ResultState.SeveralLanguageGameAdded:
                    return Ok(result.Value); //standard user can add more game translation
                case ResultState.GameHaveTranslationForThisLanguage:
                    return BadRequest(result.Value.ToString()); // if game have translation for language
                default:
                    return BadRequest();
            }
        }

        [System.Web.Http.Authorize]
        [Route("PostGameFeature")]
        public IHttpActionResult PostGameFeature(GameFeatureModel gameFeatureModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (_repo.PutGameFeature(gameFeatureModel) == ResultState.GameFeatureEdited)
                return Ok(gameFeatureModel);
            return NotFound();
        }

        [System.Web.Http.Authorize]
        [Route("PostGameFeatureDetail")]
        public IHttpActionResult PostGameFeatureDetail(GameFeatureDetailModel gameFeatureDetailModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (_repo.PutGameFeatureDetail(gameFeatureDetailModel) == ResultState.GameFeatureDetailEdited)
                return Ok(gameFeatureDetailModel);
            return NotFound();
        }

        [System.Web.Http.AllowAnonymous]
        [Route("GetIntegrationGames")]
        public IHttpActionResult GetIntegrationGames(int language)//tutaj bedzie treba przesyłać id jezyka ... 
        {
            return Ok(_repo.GetIntegrationGames(language));
        }

        [System.Web.Http.Authorize]
        [Route("GetIntegrationGamesWithAvailableLanguage")]
        public IHttpActionResult GetIntegrationGamesWithAvailableLanguage(int language)
        {
            return Ok(_repo.GetIntegrationGamesWithAvailableLanguages(language));
        }

        [System.Web.Http.AllowAnonymous]
        [Route("GetGameFeatures")]
        public IHttpActionResult GetGameFeatures(int language)//tutaj bedzie treba przesyłać id jezyka ... 
        {
            return Ok(_repo.GetGameFeatures(language));
        }
    }
}
