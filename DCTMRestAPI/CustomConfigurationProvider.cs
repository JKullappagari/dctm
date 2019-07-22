using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DCTMRestAPI
{
    public class CustomConfigurationProvider : JsonConfigurationProvider
    {
        public CustomConfigurationProvider(CustomConfigurationSource source) : base(source)
        {
        }

        public override void Load(Stream stream)
        {
            // Let the base class do the heavy lifting.
            base.Load(stream);

            // Do decryption here, you can tap into the Data property like so:

            //Data["abc:password"] = MyEncryptionLibrary.Decrypt(Data["abc:password"]);

            // But you have to make your own MyEncryptionLibrary, not included here
        }
    }
}
