using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpotifyWebAPI_Intro.Models;
using SpotifyWebAPI_Intro.Services;

namespace SpotifyWebAPI_Intro.Controllers
{
    [ApiController]
    [Route("login")] // Base route "login"
    public class LogInController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        public LogInController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
    }
}