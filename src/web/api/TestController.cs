using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace web.api
{
  [AllowAnonymous]
  [Route("api/[controller]/[action]")]
  [ApiController]
  public class TestController : ControllerBase
  {
    private ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger)
    {
      _logger = logger;
    }

    [HttpPost]
    public IActionResult Ping()
    {
      //  Si usamos POST en postman podemos acceder a este metodo ya sea asi:
      //  http://localhost:5000/api/test/ping
      //  y tambien asi:
      //  http://localhost:5000/api/test/ping?id=10
      //  aunque por supuesto en la segunda opcion la clave id es ignorada por el binder
      //
      return Ok("PONG");
    }

    //  [Route("{id}")]
    [Route("{id:int?}")]
    public ActionResult<string> Ping(int id)
    {
      //  al poner Route significa que se accede asi: 
      //  http://localhost:5000/api/test/ping/10
      //  http://localhost:5000/api/test/ping/100
      //  y no asi:
      //  http://localhost:5000/api/test/ping?id=10
      //
      if (id == 10)
        return BadRequest();

      return $"PONG {id}";
    }

    public IActionResult Ping1()
    {
      //  http://localhost:5000/api/test/ping1
      //
      return Ok("PONG1");
    }

    public IEnumerable<string> Ping2()
    {
      //  http://localhost:5000/api/test/ping2
      //
      return new [] {"PONG 1", "PONG 2"};
    }
  }
}
