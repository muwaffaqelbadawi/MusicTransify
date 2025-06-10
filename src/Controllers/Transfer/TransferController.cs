using System;
using MusicTransify.src.Controllers.Common;
using MusicTransify.src.Services.Common;

namespace MusicTransify.src.Controllers.Transfer
{
    public class TransferController : AuthController
    {
        public TransferController(
            AuthService authService,
            SessionService sessionService,
            ILogger<AuthController> logger
        ) : base(authService, sessionService, logger)
        {
        }
    }
}