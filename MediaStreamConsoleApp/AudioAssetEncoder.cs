using Microsoft.WindowsAzure.MediaServices.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aqa.NonExaminedAssessments.MediaFileProcessor
{
    public class AudioAssetEncoder : MediaAssetEncoder
    {
        public string _preset = "AAC Good Quality Audio";
        private ICloudMediaService cloudMediaService;

        public AudioAssetEncoder(ICloudMediaService context)
        {
            cloudMediaService = context;
        }

        public IAsset EncodeAudioAsset(IAsset asset, string fileHash)
        {
            return base.CreateEncodingJob(asset, this._preset, this.cloudMediaService.Context, fileHash);
        }

        /// <summary>
        /// Create an origin locator in order to get the streaming URLs.
        /// </summary>
        /// <param name="assetToStream"></param>
        /// <returns></returns>
        public string BuildSasUrlForAudioFile(IAsset asset, CloudMediaContext m_context)
        {
            // Declare an access policy for permissions on the asset. 
            // You can call an async or sync create method. 
            IAccessPolicy policy =
                m_context.AccessPolicies.Create("My 30 days readonly policy",
                    TimeSpan.FromDays(30),
                    AccessPermissions.Read);

            // Create a SAS locator to enable direct access to the asset 
            // in blob storage. You can call a sync or async create method.  
            // You can set the optional startTime param as 5 minutes 
            // earlier than Now to compensate for differences in time  
            // between the client and server clocks. 

            ILocator locator = m_context.Locators.CreateLocator(LocatorType.Sas,
                asset,
                policy,
                DateTime.UtcNow.AddMinutes(-5));

            var mp4File = asset.AssetFiles.ToList().
                           Where(f => f.Name.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase)).
                           FirstOrDefault();


            // Take the locator path, add the file name, and build 
            // a full SAS URL to access this file. This is the only 
            // code required to build the full URL.
            var uriBuilder = new UriBuilder(locator.Path);
            uriBuilder.Path += "/" + mp4File.Name;
            
            //Return the SAS URL.
            return uriBuilder.Uri.AbsoluteUri;
        }
    }
}