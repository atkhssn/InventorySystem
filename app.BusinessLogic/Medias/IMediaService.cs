using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace app.Service.Medias
{
    public interface IMediaService
    {
        string UploadFile(IFormFile file, string folderName);
        string UpdateFile(string media, IFormFile file, string folderName);
        void DeleteFile(string filename);
         string GetMimeType(string extension);
    }
}
