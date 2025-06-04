using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MusicTransify.src.Controllers.Common
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly ILogger<BaseApiController> _logger;

        public BaseApiController(ILogger<BaseApiController> logger) : base()
        {
            _logger = logger;
        }
    }
}