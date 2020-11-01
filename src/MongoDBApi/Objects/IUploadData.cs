using System.Collections.Generic;


namespace MongoDBApi.Objects
{
    public interface IUploadData
    {
        int NoFiles {get; set;}
        double SizeOnDisk {get; set;}
        List<string> FilePaths {get; set;}

        string ToString();
        string Build(int noFiles, double sizeOnDisk, List<string> filePaths);
    }
}