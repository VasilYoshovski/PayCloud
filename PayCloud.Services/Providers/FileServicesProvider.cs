using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayCloud.Services.Providers
{
    public class FileServicesProvider : IFileServicesProvider
    {
        public bool FileExists(string filePath)
        {
            return System.IO.File.Exists(filePath.Trim());
        }

        public (bool result, string message) DeleteFile(string filePath)
        {
            // Delete a file by using File class static method...
            if (FileExists(filePath.Trim()))
            {
                // Use a try block to catch IOExceptions, to
                // handle the case of the file already being
                // opened by another process.
                try
                {
                    System.IO.File.Delete(filePath);
                    return (true, "");
                }
                catch (System.IO.IOException e)
                {
                    return (false, e.Message);
                }
            }
            return (true, $"Missing file: {filePath}");
        }

        //public (string result, string message) CreateFile(string filePath, IFormFile fileContent)
        //{
        //    filePath.Trim();
        //    return (filePath, "");
        //}

        public (bool result, string message) CreateFolder(string filePath)
        {
            // Create folder...
            if (FileExists(filePath.Trim()))
            {
                return (true, $"Folder already exists: {filePath}");
            }
            try
            {
                System.IO.Directory.CreateDirectory(filePath);
                return (true, "");
            }
            catch (System.IO.IOException e)
            {
                return (false, e.Message);
            }
        }
    }
}
