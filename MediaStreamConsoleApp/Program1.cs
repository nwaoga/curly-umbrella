using Microsoft.WindowsAzure.MediaServices.Client;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaStreamConsoleApp
{
    class Program1
    {
        
        static void run(string[] args)
        {
            upload2();
            createStreamUrl();
        }

        public static void upload()
        {
            // Create a .NET console app
            // Set the project properties to use the full .NET Framework (not Client Profile)
            // With NuGet Package Manager, install windowsazure.mediaservices
            // add: using Microsoft.WindowsAzure.MediaServices.Client;
            var uploadFilePath = @"C:\Users\en\Desktop\test\DobermanPinscher.jpg";
            var context = new CloudMediaContext("nwaoga", "QlPFaGJXYAqlvP3M6TuWFwjwmvxyRWvczFMcdlpeYJg=");

            var uploadAsset = context.Assets.Create(Path.GetFileNameWithoutExtension(uploadFilePath), AssetCreationOptions.None);
            
            var assetFile = uploadAsset.AssetFiles.Create(Path.GetFileName(uploadFilePath));
            assetFile.Upload(uploadFilePath);

        }

        public static void upload2()
        {
            var MystorageCredentials = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials("nwaoga", "QlPFaGJXYAqlvP3M6TuWFwjwmvxyRWvczFMcdlpeYJg=");
            var cloudStorage = new CloudStorageAccount(MystorageCredentials, true);
        }

        public static void createStreamUrl()
        {
            var context = new CloudMediaContext("nwaoga", "QlPFaGJXYAqlvP3M6TuWFwjwmvxyRWvczFMcdlpeYJg=");
            // TODO: Replace with an IAsset.Id string if you are not using the previous snippets
            var streamingAssetId = "nb:cid:UUID:a02cf8cb-b333-4574-ba1a-566ceab0702f"; // "YOUR ASSET ID";
            var daysForWhichStreamingUrlIsActive = 365;
            var streamingAsset = context.Assets.Where(a => a.Id == streamingAssetId).FirstOrDefault();
            var accessPolicy = context.AccessPolicies.Create(streamingAsset.Name, TimeSpan.FromDays(daysForWhichStreamingUrlIsActive),
                                                     AccessPermissions.Read);
            string streamingUrl = string.Empty;
            var assetFiles = streamingAsset.AssetFiles.ToList();
            var streamingAssetFile = assetFiles.Where(f => f.Name.ToLower().EndsWith("m3u8-aapl.ism")).FirstOrDefault();
            if (streamingAssetFile != null)
            {
                var locator = context.Locators.CreateLocator(LocatorType.OnDemandOrigin, streamingAsset, accessPolicy);
                Uri hlsUri = new Uri(locator.Path + streamingAssetFile.Name + "/manifest(format=m3u8-aapl)");
                streamingUrl = hlsUri.ToString();
            }
            streamingAssetFile = assetFiles.Where(f => f.Name.ToLower().EndsWith(".ism")).FirstOrDefault();
            if (string.IsNullOrEmpty(streamingUrl) && streamingAssetFile != null)
            {
                var locator = context.Locators.CreateLocator(LocatorType.OnDemandOrigin, streamingAsset, accessPolicy);
                Uri smoothUri = new Uri(locator.Path + streamingAssetFile.Name + "/manifest");
                streamingUrl = smoothUri.ToString();
            }
            streamingAssetFile = assetFiles.Where(f => f.Name.ToLower().EndsWith(".mp4")).FirstOrDefault();
            if (string.IsNullOrEmpty(streamingUrl) && streamingAssetFile != null)
            {
                var locator = context.Locators.CreateLocator(LocatorType.Sas, streamingAsset, accessPolicy);
                var mp4Uri = new UriBuilder(locator.Path);
                mp4Uri.Path += "/" + streamingAssetFile.Name;
                streamingUrl = mp4Uri.ToString();
            }
            Console.WriteLine("Streaming Url: " + streamingUrl);

            Console.Read();
        }
    }
}
