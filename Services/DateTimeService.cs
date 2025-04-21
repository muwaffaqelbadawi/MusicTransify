using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyWebAPI_Intro.Services
{

    /// <inheritdoc />
    public class DateTimeService : IDateTimeService
    {
        public DateTime GetUTCNow()
        {
            return DateTime.UtcNow;
        }
    }
}