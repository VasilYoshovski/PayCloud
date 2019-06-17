using System;
using System.Collections.Generic;
using System.Text;

namespace PayCloud.Services.Providers
{
    public interface IFileServicesProvider
    {
        bool FileExists(string filePath);
        (bool result, string message) DeleteFile(string filePath);
        (bool result, string message) CreateFolder(string filePath);
    }
}
