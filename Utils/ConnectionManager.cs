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
            var accountName = ConfigurationManager.AppSettings["storage:account:name"];
            var accountKey = ConfigurationManager.AppSettings["storage:account:key"];

            return new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
        }
    }
}