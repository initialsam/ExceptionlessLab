using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExceptionlessLab.Controllers
{
    public class TestController : ApiController
    {
        public IHttpActionResult Get()
        {
            var zero = 0;
            var temp = 1 / zero;
            return Ok();
        }

public IHttpActionResult Get(int id)
{
    return Ok(new
        {
            code = 200,
            id = id
        });
}
    }
}
