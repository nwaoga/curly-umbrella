using Microsoft.WindowsAzure.MediaServices.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aqa.NonExaminedAssessments.MediaFileProcessor
{
    public class VideoAssetEncoder : MediaAssetEncoder
    {
        public string _preset = "H264 Adaptive Bitrate MP4 Set 720p";
        private ICloudMediaService cloudMediaService;

        public VideoAssetEncoder(ICloudMediaService context)
        {
            cloudMediaService = context;
        }

        public IAsset EncodeVideoAsset(IAsset asset, string fileHash)
        {
            return base.CreateEncodingJob(asset, this._preset, this.cloudMediaService.Context, fileHash);
        }

        /// <summary>
        /// Create an origin locator in order to get the streaming URLs.
        /// </summary>
        /// <param name="assetToStream"></param>
        /// <returns></returns>
        public string GetStreamingOriginLocatorURL(IAsset assetToStream, int days)
        {
            // Get a reference to the streaming manifest file from the  
            // collection of files in the asset. 

            var theManifest =
                               from f in assetToStream.AssetFiles
                               where f.Name.EndsWith(".ism")
                               select f;

            // Cast the reference to a true IAssetFile type. 
            IAssetFile manifestFile = theManifest.First();


            // Create a 30-day readonly access policy. 
            IAccessPolicy policy = this.cloudMediaService.Context.AccessPolicies.Create("Streaming policy",
                TimeSpan.FromDays(days),
                AccessPermissions.Read);

            // Create a locator to the streaming content on an origin. 
            ILocator originLocator = this.cloudMediaService.Context.Locators.CreateLocator(LocatorType.OnDemandOrigin,
                assetToStream,
                policy,
                DateTime.UtcNow.AddMinutes(-5));

            // Create a full URL to the manifest file. 
            return originLocator.Path + manifestFile.Name + "/Manifest";
        }
    }
}
