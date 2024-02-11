using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.core.Entities.Rta
{
    public class ERta
    {
        public int Status { get; set; } //Tipo de estado para la respuesta: 200=OK, 400=BadRequest, 404=NotFound, 500=InternalServerError
        public string Message { get; set; } //Mensaje de error o exito
        public object Data { get; set; } //Cualquier tipo de información
    }
}
