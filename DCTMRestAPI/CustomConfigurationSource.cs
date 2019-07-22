using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCTMRestAPI
{
    public class CustomConfigurationSource : JsonConfigurationSource
    {
        //public CustomConfigurationSource() { }


        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new CustomConfigurationProvider(this);
        }
    }
}
