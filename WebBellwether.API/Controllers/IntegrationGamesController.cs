﻿using System.Web.Http;
using WebBellwether.API.Models.IntegrationGame;
using WebBellwether.API.Results;
using WebBellwether.API.Services.IntegrationGameService;
using WebBellwether.API.Services.IntegrationGameService.Abstract;
using WebBellwether.API.Repositories;

namespace WebBellwether.API.Controllers
{
    [RoutePrefix("api/IntegrationGames")]
    public class IntegrationGamesController : ApiController
    {
        private readonly IIntegrationGameService _service;
        public IntegrationGamesController()
        {
            _service = new IntegrationGameService(new AggregateRepositories());
        }

        [AllowAnonymous]
        [Route("GetGameFeatureDetails")]
        public IHttpActionResult GetGameFeatureDetails(int language)
        {
            return Ok(_service.GetGameFeatureDetails(language));
        }

        [AllowAnonymous]
        [Route("GetGameFeatuesModelWithDetails")]
        public IHttpActionResult GetGameFeatuesModelWithDetails(int language)
        {
            return Ok(_service.GetGameFeatuesModelWithDetails(language));
        }

        [Authorize]
        [Route("PostIntegrationGame")]
        public IHttpActionResult PostIntegrationGame(NewIntegrationGameModel gameModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ResultStateContainer result = _service.InsertIntegrationGame(gameModel);
            return result.ResultState == ResultState.Success ? Ok(result.ResultValue) : (IHttpActionResult)BadRequest(result.ResultMessage.ToString());        
        }
        [Authorize]
        [Route("PostEditIntegrationGame")]
        public IHttpActionResult PostEditIntegrationGame(IntegrationGameModel integrationGame)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            ResultStateContainer result =  _service.PutIntegrationGame(integrationGame);
            return result.ResultState == ResultState.Success ? Ok() : (IHttpActionResult)BadRequest(result.ResultMessage.ToString());
        }

        [Authorize]
        [Route("PostDeleteIntegrationGame")]
        public IHttpActionResult PostDeleteIntegrationGame(IntegrationGameModel integrationGame)
        {
            ResultStateContainer result = _service.DeleteIntegratiomGame(integrationGame);
            return result.ResultState == ResultState.Success ? Ok(result.ResultValue) : (IHttpActionResult)BadRequest(result.ResultMessage.ToString());
        }

        [Authorize]
        [Route("PostGameFeature")]
        public IHttpActionResult PostGameFeature(GameFeatureModel gameFeatureModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (_service.PutGameFeature(gameFeatureModel) == ResultMessage.GameFeatureEdited)
                return Ok(gameFeatureModel);
            return NotFound();
        }

        [Authorize]
        [Route("PostGameFeatureDetail")]
        public IHttpActionResult PostGameFeatureDetail(GameFeatureDetailModel gameFeatureDetailModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (_service.PutGameFeatureDetail(gameFeatureDetailModel) == ResultMessage.GameFeatureDetailEdited)
                return Ok(gameFeatureDetailModel);
            return NotFound();
        }

        [AllowAnonymous]
        [Route("GetIntegrationGames")]
        public IHttpActionResult GetIntegrationGames(int language)
        {
            return Ok(_service.GetIntegrationGames(language));
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetIntegrationGameTranslation")]
        public IHttpActionResult GetIntegrationGameTranslation(int id,int languageId)//here i have game main id and language id 
        {
           return Ok(_service.GetGameTranslation(id,languageId));
        }

        [Authorize]
        [Route("GetIntegrationGamesWithAvailableLanguage")]
        public IHttpActionResult GetIntegrationGamesWithAvailableLanguage(int language)
        {
            return Ok(_service.GetIntegrationGamesWithAvailableLanguages(language));
        }

        [AllowAnonymous]
        [Route("GetGameFeatures")]
        public IHttpActionResult GetGameFeatures(int language)
        {
            return Ok(_service.GetGameFeatures(language));
        }
    }//153 linie kody były prawie 50 mniej ;) 
}
