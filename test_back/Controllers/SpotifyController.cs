using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using test.core.Interface.Spotify;
using test.core.Logic.Helpers;

namespace test_back.Controllers
{
    [Route("api")]
    [ApiController]
    public class SpotifyController : Controller
    {
        private readonly ISpotify _service;
        protected SetHttpResponse _setResponse;

        public SpotifyController(ISpotify service)
        {
            _service = service;
        }

        #region Buscar
        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el token JWT de acceso</response>
        /// <response code="400">Bad Request. Url no encontrada o formato del de objeto incorrecto</response>
        /// <response code="200">OK. Detalles retornados con éxito</response>
        [HttpGet]
        [Route("search/{consult}/{type}/{token}")]
        public async Task<IActionResult> Search([FromRoute] string consult, [FromRoute] string type, [FromRoute] string token )
        {
            var rta = await _service.Search(consult,type, token);
            _setResponse = new SetHttpResponse();
            return _setResponse.GetHttpResponse(rta);
        }
        #endregion

    }
}
