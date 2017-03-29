using Microsoft.WindowsAzure.MediaServices.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aqa.NonExaminedAssessments.MediaFileProcessor
{
    public  class MediaAssetEncoder
    {
        public IAsset CreateEncodingJob(IAsset asset, string preset, CloudMediaContext _context, string fileHash)
        {
            // Declare a new job.
            IJob job = _context.Jobs.Create(preset + " encoding job");
            // Get a media processor reference, and pass to it the name of the 
            // processor to use for the specific task.
            var mediaProcessors = _context.MediaProcessors.Where(p => p.Name.Contains("Media Encoder")).ToList();

            var latestMediaProcessor = mediaProcessors.OrderBy(mp => new Version(mp.Version)).LastOrDefault();

            // Create a task with the encoding details, using a string preset.
            ITask task = job.Tasks.AddNew(preset + " encoding task", latestMediaProcessor, preset, TaskOptions.ProtectedConfiguration);

            // Specify the input asset to be encoded.
            task.InputAssets.Add(asset);

            // Add an output asset to contain the results of the job. 
            // This output is specified as AssetCreationOptions.None, which 
            // means the output asset is not encrypted. 
            task.OutputAssets.AddNew(string.Format("{0}-{1}", "Output", fileHash), AssetCreationOptions.None);

            // Launch the job.
            job.Submit();
            Task progressJobTask = job.GetExecutionProgressTask(CancellationToken.None);
            progressJobTask.Wait();

            // If job state is Error the event handling 
            // method for job progress should log errors.  Here we check 
            // for error state and exit if needed.
            if (job.State == JobState.Error)
            {
                throw new Exception("\nExiting method due to job error.");
                //will need to log this somwehere
            }
            return job.OutputMediaAssets[0];
        }
    }
}

