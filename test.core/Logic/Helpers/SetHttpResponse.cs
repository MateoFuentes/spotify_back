using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.core.Entities.Rta;

namespace test.core.Logic.Helpers
{
    public class SetHttpResponse : ControllerBase
    {
        public ERta oReply = new ERta();

        #region Metodo para validar respuesta de servicios.
        public IActionResult GetHttpResponse(ERta response)
        {
            oReply.Status = response.Status;
            oReply.Message = response.Message;
            oReply.Data = response.Data;

            switch (response.Status)
            {
                case 200:
                    return Ok(oReply);

                case 201:
                    return Created("", oReply);

                case 204:
                    oReply.Status = 204;
                    return NoContent();

                case 401:
                    oReply.Status = 401;
                    return Unauthorized();

                case 404:
                    oReply.Status = 404;
                    return NotFound(oReply);

                case 500:
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = oReply.Message });

                default:
                    return BadRequest(oReply);
            };

        }
        #endregion
    }
}
