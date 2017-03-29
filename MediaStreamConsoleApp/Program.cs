using System;
using System.Linq;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.MediaServices.Client;
using MediaCloudManager;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Aqa.NonExaminedAssessments.MediaFileProcessor;

namespace MediaServicesGettingStarted
{
    class Program
    {
        #region setUpConfigForStorageAquire
        private static readonly string _supportFiles = Path.GetFullPath(@"../..\Media");

        // Paths to support files (within the above base path). You can use 
        // the provided sample media files from the "supportFiles" folder, or 
        // provide paths to your own media files below to run these samples.
        private static readonly string _singleWMVInputFilePath =
            Path.GetFullPath(_supportFiles + @"\interview1.wmv");

        private static readonly string _singleMP4InputFilePath =
            Path.GetFullPath(_supportFiles + @"\BigBuckBunny.mp4");

        private static readonly string _singleMP3InputFilePath =
            Path.GetFullPath(_supportFiles + @"\SampleAudio_0.5mb.mp3");

        private static readonly string _outputFilesFolder =
            Path.GetFullPath(_supportFiles + @"\OutputFiles");
        // Read values from the App.config file.
        private static readonly string _mediaServicesAccountName = 
            ConfigurationManager.AppSettings["MediaServicesAccountName"];
        
        private static readonly string _mediaServicesAccountKey =
            ConfigurationManager.AppSettings["MediaServicesAccountKey"];

        // Field for service context.
        private static CloudMediaContext _context = null;
        private static MediaServicesCredentials _cachedCredentials = null;
        #endregion

        static void Main(string[] args)
        {
             
            ////var mcm = new MediaCloudManager.MediaCloudManager(_mediaServicesAccountName, _mediaServicesAccountKey);
            ////IAsset singleMP4Asset = mcm.CreateAssetAndUploadSingleFile(AssetCreationOptions.None, _singleMP4InputFilePath);
            ////IAsset singleMP3sset = mcm.CreateAssetAndUploadSingleFile(AssetCreationOptions.None, _singleMP3InputFilePath);
            ////IAsset adaptiveBitrateAsset = mcm.CreateEncodingJob(singleMP4Asset, "H264 Adaptive Bitrate MP4 Set 720p");
            ////IAsset adaptiveBitrateAsset = mcm.CreateEncodingJob(singleMP3sset, "AAC Good Quality Audio");

            ////string streamingURL = mcm.GetStreamingOriginLocatorURL(adaptiveBitrateAsset);
            //// mcm.blobRetrive();

            ////CloudStorageAccount storageAccount1 = new CloudStorageAccount(new StorageCredentials("colanut", "2ZlvU8CoFlbtkiU356mYB3OuGolkMvN4401ZSEZr8wJxRHJr7+sGN/aDc8AAn3h80ECocSKctGNAu9J/qLiLew=="), false);
            ////CloudBlobClient cloudBlobClient = storageAccount1.CreateCloudBlobClient();
            ////CloudBlobContainer sourceContainer = cloudBlobClient.GetContainerReference("apples");

            var mServ = new MediaService();
            //var mTempBlobServ = new CloudBlobService("mediasvch9rcgqbnckzkv", "X0IRGCuXN1zRFKBeAIlc2SYtG6vQB8lEPi52O/1aNoyea5wtIzqbyM5t0nF9cJXoFii/N6mdxBt2Cl2YIrcuTw==");
            //var mdestinationBlobServ = new CloudBlobService("mediasvch9rcgqbnckzkv", "X0IRGCuXN1zRFKBeAIlc2SYtG6vQB8lEPi52O/1aNoyea5wtIzqbyM5t0nF9cJXoFii/N6mdxBt2Cl2YIrcuTw==");

            var mTempBlobServ = new CloudBlobService();
            var mdestinationBlobServ = new CloudBlobService();

            
            var vidAssEncObj = new VideoAssetEncoder(mServ);
            var AudAssEncObj = new AudioAssetEncoder(mServ);
            var medAssMan = new MediaAssetManager(mServ);

            ////var bob = new MediaAssetManager(mServ);
            ////var blobRef = mTempBlobServ.context.CreateCloudBlobClient().GetContainerReference("apples");

            ////IAsset asset = MediaAssetManager.CreateAssetFromExistingBlobs(blobRef, mServ, mdestinationBlobServ);
            ////asset.Update();
            var uploadFilesblobContainerObj = mTempBlobServ.context.CreateCloudBlobClient().GetContainerReference("asemupload");

            var assetListList = mServ.Context.Assets.ToList<IAsset>();

            //foreach (var asst in assetListList )
            //{
            //    if (asst.Name == "Output-hashhash#####SampleAudio_0.5mb.mp3")
            //    {
            //        Console.WriteLine(asst.Name);
            //        var assetfiles = asst.AssetFiles.ToList();
            //        foreach (var loctr in asst.Locators )
            //        {
            //            var uriBuilder = new UriBuilder(loctr.Path);
            //            uriBuilder.Path += "/" + asst.Name;
            //            Console.Write(uriBuilder.Uri.AbsoluteUri);
            //        }
            //    }
            //}


            //IAsset asset = MediaAssetManager.CreateAssetFromExistingBlobs(uploadFilesblobContainerObj, mServ, mdestinationBlobServ, "SampleAudio_0.5mb.mp3");
            //IAsset asset = MediaAssetManager.CreateAssetFromExistingBlobs(uploadFilesblobContainerObj, mServ, mdestinationBlobServ, "Example.ogg");
            //IAsset streamableAsset = AudAssEncObj.EncodeAudioAsset(asset, "SampleAudio_0.5mb.mp3");
            //var bob = AssetUrlBuilder.BuildSASAudioUrl(streamableAsset, mServ, 30);
            //var bob = AudAssEncObj.BuildSasUrlForAudioFile(streamableAsset, mServ.Context);

            //Console.WriteLine(bob);



            IAsset asset = MediaAssetManager.CreateAssetFromExistingBlobs(uploadFilesblobContainerObj, mServ, mdestinationBlobServ, "BigBuckBunny.mp4");
            IAsset streamableAsset = vidAssEncObj.EncodeVideoAsset(asset, "BigBuckBunny.mp4");
            var bob = AssetUrlBuilder.BuildStreamingURL(streamableAsset, mServ, 30);
            Console.WriteLine(bob);

            //asset.Delete();




        }

        
        


        private static void OldConsoleUpload()
        {
            // Create and cache the Media Services credentials in a static class variable.
            _cachedCredentials = new MediaServicesCredentials(_mediaServicesAccountName, _mediaServicesAccountKey);

            // Used the chached credentials to create CloudMediaContext.
            _context = new CloudMediaContext(_cachedCredentials);

            #region dont really Need
            //IAsset singleWMVAsset = CreateAssetAndUploadSingleFile(AssetCreationOptions.None, _singleWMVInputFilePath);

            // EncodeToH264 creates a job with one task
            // that converts a mezzanine file (in this case interview1.wmv)
            // into an MP4 file (in this case, "H264 Broadband 720p").
            //IAsset MP4Asset = CreateEncodingJob(singleWMVAsset, "H264 Broadband 720p");


            // BuildSasUrlForMP4File creates a SAS Locator
            // and builds the SAS Url that can be used to 
            // progressively download the MP4 file.
            //string fullSASURL = BuildSasUrlForMP4File(MP4Asset);

            //Console.WriteLine("Progressive download URL: {0}", fullSASURL);

            // Download all the files in the asset locally
            // (that includes the mainifest.xml file).
            // DownloadAssetToLocal(MP4Asset, _outputFilesFolder);

            Console.WriteLine();
            #endregion

            IAsset singleMP4Asset = CreateAssetAndUploadSingleFile(AssetCreationOptions.None, _singleMP4InputFilePath);
            // EncodeToAdaptiveBitrate creates a job with one task
            // that encodes a mezzanine file (in this case BigBuckBunny.mp4)
            // into an adaptive bitrate MP4 set (in this case, "H264 Adaptive Bitrate MP4 Set 720p").
            IAsset adaptiveBitrateAsset = CreateEncodingJob(singleMP4Asset, "H264 Adaptive Bitrate MP4 Set 720p");

            // Get the Streaming Origin Locator URL.
            string streamingURL = GetStreamingOriginLocatorURL(adaptiveBitrateAsset, _context);

            // Add Smooth Streaming, HLS, and DASH format to the streaming URL.  
            // NOTE: To be able to play these streams based on the 
            // adaptiveBitrateAsset asset, you MUST have at least one
            // On-demand Streaming reserved unit. 
            // For more information, see: 
            //    Dynamic Packaging (http://msdn.microsoft.com/en-us/library/azure/jj889436.aspx)
            Console.WriteLine("Smooth Streaming format:");
            Console.WriteLine("{0}", streamingURL + "/Manifest");
            Console.WriteLine("Apple HLS format:");
            Console.WriteLine("{0}", streamingURL + "/Manifest(format=m3u8-aapl)");
            Console.WriteLine("MPEG DASH format:");
            Console.WriteLine("{0}", streamingURL + "/Manifest(format=mpd-time-csf)");
            Console.Read();
        }

        /// <summary>
        /// Creates an asset and uploads a single file.
        /// </summary>
        /// <param name="assetCreationOptions">Asset creation options.</param>
        /// <param name="singleFilePath">File path.</param>
        /// <returns></returns>
        static public IAsset CreateAssetAndUploadSingleFile(AssetCreationOptions assetCreationOptions, string singleFilePath )
        {
            var assetName = "UploadSingleFile_" + DateTime.UtcNow.ToString();
            var asset = _context.Assets.Create(assetName, assetCreationOptions);

            var fileName = Path.GetFileName(singleFilePath);

            var assetFile = asset.AssetFiles.Create(fileName);

            Console.WriteLine("Created assetFile {0}", assetFile.Name);

            var accessPolicy = _context.AccessPolicies.Create(assetName, TimeSpan.FromDays(80),
                                                                AccessPermissions.Write | AccessPermissions.List);

            var locator = _context.Locators.CreateLocator(LocatorType.Sas, asset, accessPolicy);

            Console.WriteLine("Upload {0}", assetFile.Name);

            assetFile.Upload(singleFilePath);
            Console.WriteLine("Done uploading {0}", assetFile.Name);

            locator.Delete();
            accessPolicy.Delete();

            return asset;
        }

        /// <summary>
        /// Creates an encoding job using the specified preset.
        /// </summary>
        /// <param name="asset">The source asset.</param>
        /// <param name="preset">Preset name.</param>
        /// <returns></returns>
        static public IAsset CreateEncodingJob(IAsset asset, string preset)
        {
            // Declare a new job.
            IJob job = _context.Jobs.Create(preset + " encoding job");
            // Get a media processor reference, and pass to it the name of the 
            // processor to use for the specific task.
            var mediaProcessors =
                  _context.MediaProcessors.Where(p => p.Name.Contains("Media Encoder")).ToList();

            var latestMediaProcessor =
                mediaProcessors.OrderBy(mp => new Version(mp.Version)).LastOrDefault();


            // Create a task with the encoding details, using a string preset.
            ITask task = job.Tasks.AddNew(preset + " encoding task",
                latestMediaProcessor,
                preset, 
                Microsoft.WindowsAzure.MediaServices.Client.TaskOptions.ProtectedConfiguration);

            // Specify the input asset to be encoded.
            task.InputAssets.Add(asset);
            // Add an output asset to contain the results of the job. 
            // This output is specified as AssetCreationOptions.None, which 
            // means the output asset is not encrypted. 
            task.OutputAssets.AddNew("Output asset",
                AssetCreationOptions.None);

            // Use the following event handler to check job progress.  
            job.StateChanged += new
                    EventHandler<JobStateChangedEventArgs>(StateChanged);

            // Launch the job.
            job.Submit();

            // Optionally log job details. This displays basic job details
            // to the console and saves them to a JobDetails-{JobId}.txt file 
            // in your output folder.
            LogJobDetails(job.Id);

            // Check job execution and wait for job to finish. 
            Task progressJobTask = job.GetExecutionProgressTask(CancellationToken.None);
            progressJobTask.Wait();

            // If job state is Error the event handling 
            // method for job progress should log errors.  Here we check 
            // for error state and exit if needed.
            if (job.State == JobState.Error)
            {
                throw new Exception("\nExiting method due to job error.");
            }

            return job.OutputMediaAssets[0];
        }
    
        /// <summary>
        /// Create an origin locator in order to get the streaming URLs.
        /// </summary>
        /// <param name="assetToStream"></param>
        /// <returns></returns>
        static string GetStreamingOriginLocatorURL(IAsset assetToStream, CloudMediaContext m_context)
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
            IAccessPolicy policy = m_context.AccessPolicies.Create("Streaming policy",
                TimeSpan.FromDays(30),
                AccessPermissions.Read);

            // Create a locator to the streaming content on an origin. 
            ILocator originLocator = m_context.Locators.CreateLocator(LocatorType.OnDemandOrigin, 
                assetToStream,
                policy,
                DateTime.UtcNow.AddMinutes(-5));

            // Display the base path to the streaming asset on the origin server.
            Console.WriteLine("Streaming asset base path on origin: ");
            Console.WriteLine(originLocator.Path);
            Console.WriteLine();

            // Create a full URL to the manifest file. 
            return originLocator.Path + manifestFile.Name;
        }

        /// <summary>
        /// Builds a SAS URL for the specified asset.
        /// The SAS URL is used to download files.
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        static public string BuildSasUrlForMP4File(IAsset asset, CloudMediaContext m_context)
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

            Console.WriteLine("Full URL to file: ");
            Console.WriteLine(uriBuilder.Uri.AbsoluteUri);
            Console.WriteLine();

            //Return the SAS URL.
            return uriBuilder.Uri.AbsoluteUri;
        }

        /// <summary>
        /// Download the asset to a local folder.
        /// The download progress is displayed in the console window.
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="outputFolder"></param>
        /// <returns></returns>
        static public IAsset DownloadAssetToLocal(IAsset asset, string outputFolder)
        {
            IAccessPolicy accessPolicy = _context.AccessPolicies.Create("File Download Policy", TimeSpan.FromDays(30), AccessPermissions.Read);
            ILocator locator = _context.Locators.CreateLocator(LocatorType.Sas, asset, accessPolicy);
            BlobTransferClient blobTransfer = new BlobTransferClient
            {
                NumberOfConcurrentTransfers = 10,
                ParallelTransferThreadCount = 10
            };

            Console.WriteLine("Files will be downloaded to:");
            Console.WriteLine("{0}", outputFolder);
            Console.WriteLine();

            var downloadTasks = new List<Task>();
            foreach (IAssetFile outputFile in asset.AssetFiles)
            {
                // Use the following event handler to check download progress.
                outputFile.DownloadProgressChanged += DownloadProgress;

                string localDownloadPath = Path.Combine(outputFolder, outputFile.Name);

                downloadTasks.Add(outputFile.DownloadAsync(Path.GetFullPath(localDownloadPath), blobTransfer, locator, CancellationToken.None));

                outputFile.DownloadProgressChanged -= DownloadProgress;
            }

            Task.WaitAll(downloadTasks.ToArray());

            return asset;
        }

        //////////////////////////////////////////////////
        // Delete tasks
        //////////////////////////////////////////////////

        static public void DeleteAsset(IAsset asset)
        {
            // Delete Asset's locators before
            // deleting the asset.
            foreach (var l in asset.Locators)
            {
                Console.WriteLine("Deleting the Locator {0}", l.Id);
                l.Delete();
            }

            Console.WriteLine("Deleting the Asset {0}", asset.Id);
            // delete the asset
            asset.Delete();
        }

        static public void DeleteAccessPolicy(string existingPolicyId)
        {
            // To delete a specific access policy, get a reference to the policy.  
            // based on the policy Id passed to the method.
            var policy = _context.AccessPolicies.
                Where(p => p.Id == existingPolicyId).FirstOrDefault();

            Console.WriteLine("Deleting policy {0}", existingPolicyId);
            policy.Delete();

        }

        //////////////////////////////////////////////////
        // Private helper methods.
        //////////////////////////////////////////////////

        static private void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.WriteLine(string.Format("{0} % download progress. ", e.Progress));
        }


        static private void StateChanged(object sender, JobStateChangedEventArgs e)
        {
            Console.WriteLine("Job state changed event:");
            Console.WriteLine("  Previous state: " + e.PreviousState);
            Console.WriteLine("  Current state: " + e.CurrentState);

            switch (e.CurrentState)
            {
                case JobState.Finished:
                    Console.WriteLine();
                    Console.WriteLine("********************");
                    Console.WriteLine("Job is finished.");
                    Console.WriteLine("Please wait while local tasks or downloads complete...");
                    Console.WriteLine("********************");
                    Console.WriteLine();
                    Console.WriteLine();
                    break;
                case JobState.Canceling:
                case JobState.Queued:
                case JobState.Scheduled:
                case JobState.Processing:
                    Console.WriteLine("Please wait...\n");
                    break;
                case JobState.Canceled:
                case JobState.Error:
                    // Cast sender as a job.
                    IJob job = (IJob)sender;
                    // Display or log error details as needed.
                    LogJobStop(job.Id);
                    break;
                default:
                    break;
            }
        }

        static private void LogJobStop(string jobId)
        {
            StringBuilder builder = new StringBuilder();
            IJob job = _context.Jobs.Where(j => j.Id == jobId).FirstOrDefault(); 

            builder.AppendLine("\nThe job stopped due to cancellation or an error.");
            builder.AppendLine("***************************");
            builder.AppendLine("Job ID: " + job.Id);
            builder.AppendLine("Job Name: " + job.Name);
            builder.AppendLine("Job State: " + job.State.ToString());
            builder.AppendLine("Job started (server UTC time): " + job.StartTime.ToString());
            builder.AppendLine("Media Services account name: " + _mediaServicesAccountName);
            // Log job errors if they exist.  
            if (job.State == JobState.Error)
            {
                builder.Append("Error Details: \n");
                foreach (ITask task in job.Tasks)
                {
                    foreach (ErrorDetail detail in task.ErrorDetails)
                    {
                        builder.AppendLine("  Task Id: " + task.Id);
                        builder.AppendLine("    Error Code: " + detail.Code);
                        builder.AppendLine("    Error Message: " + detail.Message + "\n");
                    }
                }
            }
            builder.AppendLine("***************************\n");
            // Write the output to a local file and to the console. The template 
            // for an error output file is:  JobStop-{JobId}.txt
            string outputFile = _outputFilesFolder + @"\JobStop-" + JobIdAsFileName(job.Id) + ".txt";
            WriteToFile(outputFile, builder.ToString());
            Console.Write(builder.ToString());
        }

        static private void LogJobDetails(string jobId)
        {
            StringBuilder builder = new StringBuilder();
            IJob job = _context.Jobs.Where(j => j.Id == jobId).FirstOrDefault(); 

            builder.AppendLine("\nJob ID: " + job.Id);
            builder.AppendLine("Job Name: " + job.Name);
            builder.AppendLine("Job submitted (client UTC time): " + DateTime.UtcNow.ToString());
            builder.AppendLine("Media Services account name: " + _mediaServicesAccountName);

            // Write the output to a local file and to the console. The template 
            // for an error output file is:  JobDetails-{JobId}.txt
            string outputFile = _outputFilesFolder + @"\JobDetails-" + JobIdAsFileName(job.Id) + ".txt";
            WriteToFile(outputFile, builder.ToString());
            Console.Write(builder.ToString());
        }

        static void WriteToFile(string outFilePath, string fileContent)
        {
            StreamWriter sr = File.CreateText(outFilePath);
            sr.Write(fileContent);
            sr.Close();
        }

        static private string JobIdAsFileName(string jobID)
        {
            return jobID.Replace(":", "_");
        }
    }
}
