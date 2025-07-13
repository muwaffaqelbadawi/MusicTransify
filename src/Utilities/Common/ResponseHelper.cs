using System;
using Microsoft.AspNetCore.Mvc;

namespace MusicTransify.src.Utilities.Common
{
    public static class ResponseHelpers
    {
        public static IActionResult PlainText(this ControllerBase controller, string text)
        {
            controller.Response.ContentType = "text/plain";
            return controller.Ok(text);
        }
    }
}