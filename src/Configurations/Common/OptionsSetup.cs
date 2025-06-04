using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace MusicTransify.src.Configurations.Common
{
    public class OptionsSetup<TOptions> : IConfigureOptions<TOptions> where TOptions : class
    {
        private readonly IConfiguration _configuration;

        public OptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(TOptions options)
        {
            // Use the class name for the section binding
            _configuration.GetSection(typeof(TOptions).Name).Bind(options);
        }
    }
}