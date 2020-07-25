using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProcessApi.Core;
using ProcessApi.Models;

namespace ProcessApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProcessesController : ControllerBase
    {
        private readonly IProcessInformationStorage _storage;

        public ProcessesController(IProcessInformationStorage storage)
        {
            _storage = storage;
        }

        [HttpGet]
        public IEnumerable<ProcessInfo> Get() => _storage.GetProcessesInformation();
    }
}