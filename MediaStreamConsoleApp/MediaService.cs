using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MediaServices.Client;
using System.Configuration;

namespace Aqa.NonExaminedAssessments.MediaFileProcessor
{
    public class MediaService : ICloudMediaService
    {
        private CloudMediaContext _context;

        public MediaService()
        {
            var _cachedCredentials = new MediaServicesCredentials(ConfigurationManager.AppSettings["MediaServicesAccountName"], ConfigurationManager.AppSettings["MediaServicesAccountKey"]);
            // Used the chached credentials to create CloudMediaContext.

            this._context = new CloudMediaContext(_cachedCredentials);
        }

        public CloudMediaContext Context
        {
            get
            {
                return this._context;
            }
        }
    }
}
