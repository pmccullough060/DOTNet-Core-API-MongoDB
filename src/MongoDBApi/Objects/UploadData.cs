using System.Collections.Generic;
using Newtonsoft.Json;

namespace MongoDBApi.Objects
{
    public class UploadData : IUploadData
    {
        public int NoFiles {get; set;}
        public double SizeOnDisk {get; set;}
        public List<string> FilePaths {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string Build(int noFiles, double sizeOnDisk, List<string> filePaths)
        {
            NoFiles = noFiles;
            SizeOnDisk = sizeOnDisk;
            FilePaths = filePaths;
            return this.ToString();
        }
    }
}