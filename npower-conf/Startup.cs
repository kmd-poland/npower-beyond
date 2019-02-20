using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using npower_conf;

[assembly: WebJobsStartup(typeof(Startup))]

namespace npower_conf
{
    internal class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver
                {
                    NamingStrategy =
                    {
                        ProcessDictionaryKeys = false
                    }
                }
            };
        }
    }
}
