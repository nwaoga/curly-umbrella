using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MediaServices.Client;

namespace Aqa.NonExaminedAssessments.MediaFileProcessor
{
    public interface ICloudMediaService
    {
        CloudMediaContext Context
        {
            get;
        }
    }
}
