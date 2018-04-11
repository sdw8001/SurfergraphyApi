using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SurfergraphyApi.Utils
{
    public class ConnectionManager
    {
        public static CloudStorageAccount GetCloudStorageAcount()
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=surfergraphystorage;AccountKey=C79IdVQVXn2zHO9C/gZAC3Lo50pHkz8pRvvC0QvwgqjylqjsyMLRG1EitWaLGF/UJvGLDZ61dhfn0Rpk+s90zQ==;EndpointSuffix=core.windows.net";

            return CloudStorageAccount.Parse(connectionString);
        }
    }
}