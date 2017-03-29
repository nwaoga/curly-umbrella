using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.MediaServices.Client;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aqa.NonExaminedAssessments.MediaFileProcessor
{
    public class CloudBlobService
    {
        private CloudStorageAccount _context;

        public CloudBlobService()
        {
            this._context = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("AzureBlobStorage"));
        }

        public CloudStorageAccount context
        {
            get
            {
                return this._context;
            }

        }
    }
}
