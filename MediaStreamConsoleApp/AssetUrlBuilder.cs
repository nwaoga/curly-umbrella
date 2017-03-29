using System;
using Microsoft.WindowsAzure.MediaServices.Client;

using System.Linq;

namespace Aqa.NonExaminedAssessments.MediaFileProcessor
{
    public static class AssetUrlBuilder
    {
        public static string BuildSASAudioUrl(IAsset asset, ICloudMediaService mediaService, int days)
        {

            // Declare an access policy for permissions on the asset. 
            IAccessPolicy policy =
                mediaService.Context.AccessPolicies.Create("AQA Audio Streaming policy",
                    TimeSpan.FromDays(days),
                    AccessPermissions.Read);

            // Create a SAS locator to enable direct access to the asset 
            ILocator locator = mediaService.Context.Locators.CreateLocator(LocatorType.Sas,
                asset,
                policy,
                DateTime.UtcNow.AddMinutes(-5));

            var mp4File = asset.AssetFiles.ToList().Single(f => f.Name.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase));

            var uriBuilder = new UriBuilder(locator.Path);
            uriBuilder.Path += "/" + mp4File.Name;

            
            //Return the SAS URL.
            return uriBuilder.Uri.AbsoluteUri;

        }

        public static string BuildStreamingURL(IAsset asset, ICloudMediaService mediaService, int days)
        {
            // Get a reference to the streaming manifest file from the collection of files in the asset. 
            var theManifest = asset.AssetFiles.ToList().Single(f => f.Name.EndsWith(".ism", StringComparison.OrdinalIgnoreCase));

            IAccessPolicy policy = mediaService.Context.AccessPolicies.Create("AQA Video Streaming policy",
                TimeSpan.FromDays(days),
                AccessPermissions.Read);

            // Create a locator to the streaming content on an origin. 
            ILocator originLocator = mediaService.Context.Locators.CreateLocator(LocatorType.OnDemandOrigin,
                asset,
                policy,
                DateTime.UtcNow.AddMinutes(-5));
            var smoothDirective = "&format=smooth";
            return string.Format("{0}{1}{2}{3}", originLocator.Path, theManifest.Name, "/Manifest", smoothDirective);
        }
    }
}