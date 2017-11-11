using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CMax.Api.Versioning.Sample.Controllers
{
    public class SameController : Controller
    {
        [HttpGet]
        [VersionedRoute("api/same", 1)]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [VersionedRoute("api/same", 2)]
        public IEnumerable<string> GetV2()
        {
            return new string[] { "value1", "value2" };
        }
    }
}