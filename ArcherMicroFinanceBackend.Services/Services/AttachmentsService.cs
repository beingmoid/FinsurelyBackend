using PanoramaBackend.Data.Entities;
using PanoramaBackend.Data.Repository;
using PanoramaBackend.Services.Core;
using NukesLab.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace PanoramaBackend.Services.Services
{
    public class AttachmentsService : BaseService<Attachments, int>, IAttachmentsService
    {
        private readonly IFileUploader _fileUploader;
        public AttachmentsService(RequestScope scopeContext, IAttachmentRepository repo, IFileUploader fileUploader) : base(scopeContext, repo)
        {
            _fileUploader = fileUploader;
        }

        public async Task<List<BlobUploadDTO>> UploadFile(IList<IFormFile> files)
        {
            if (files.Count > 0)
            {
                var blobs = new List<BlobUploadDTO>();
                foreach (var file in files)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        string s = Convert.ToBase64String(fileBytes);
                        var blob = await _fileUploader.UploadFileAsync(file.FileName, fileBytes, "lym-files");
                        blobs.Add(blob);
                    }
                }
                return blobs;
            }
            return null;
        }
    }
    public interface IAttachmentsService : IBaseService<Attachments, int>
    {
        Task<List<BlobUploadDTO>> UploadFile( IList<IFormFile> files); 
    }
}
