using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebBellwether.API.Context;
using WebBellwether.API.Entities.IntegrationGames;
using WebBellwether.API.Entities.Translations;
using WebBellwether.API.Models;
using WebBellwether.API.Repositories;

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

        [AllowAnonymous]
        [Route("GetGameFeatureDetails")]
        public IHttpActionResult GetGameFeatureDetails(int language)
        {
            return Ok(_repo.GetGameFeatureDetails(language));
        }

        [Authorize]
        [Route("PostIntegrationGame")]
        public IHttpActionResult PostIntegrationGame(IntegrationGameModel game)
        {
            _repo.InsertIntegrationGame(game);
            return Ok();
        }

        [Authorize]
        [Route("PostGameFeature")]
        public IHttpActionResult PostGameFeature(GameFeatureModel gameFeatureModel)
        {
            //oczywiście tutaj moża by obsłużyc tego resultstata zwracanego z repo ale to w przyszłości przy refaktoryzacji
            _repo.PutGameFeature(gameFeatureModel);
            return Ok();
        }

        [Authorize]
        [Route("PostGameFeatureDetail")]
        public IHttpActionResult PostGameFeatureDetail(GameFeatureDetailModel gameFeatureDetailModel)
        {
            return Ok();
        }

        [AllowAnonymous]
        [Route("GetIntegrationGames")]
        public IHttpActionResult GetIntegrationGames(int language)//tutaj bedzie treba przesyłać id jezyka ... 
        {
            return Ok(_repo.GetIntegrationGames(language));
        }

        [AllowAnonymous]
        [Route("GetGameFeatures")]
        public IHttpActionResult GetGameFeatures(int language)//tutaj bedzie treba przesyłać id jezyka ... 
        {
            return Ok(_repo.GetGameFeatures(language));
        }
    }
}

//mój aktualny problem to Visual Studio 2015 CTP - Javascript support missing
//https://social.msdn.microsoft.com/Forums/sqlserver/en-US/f3693b97-7fb1-4b9a-9953-1970b767a407/visual-studio-2015-ctp-javascript-support-missing?forum=visualstudiogeneral
//