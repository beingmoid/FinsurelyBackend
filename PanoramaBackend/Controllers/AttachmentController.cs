using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NukesLab.Core.Repository;
using PanoramaBackend.Controllers;
using PanoramBackend.Data.Entities;
using PanoramBackend.Services;
using PanoramBackend.Services.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static NukesLab.Core.Common.Constants;

namespace PanoramaBackend.Api.Controllers
{

    public class AttachmentController : BaseController<Attachments,int>
    {
        private readonly IWebHostEnvironment _env;
        private readonly IFileUploader _fileUploader;

        public AttachmentController(RequestScope requestScope,IAttachmentsService
            service, IFileUploader fileUploader,
            IWebHostEnvironment env)
            :base(requestScope,service)
        {
            _env = env;
            _fileUploader = fileUploader;
        }

        private async Task<string> Upload(IList<IFormFile> files)
        {
            string wwwPath = _env.WebRootPath;
            string contentPath = _env.ContentRootPath;
            string path = Path.Combine(contentPath, "uploads");
            string baseUri = "";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.GetTempFileName();
                    var ext = new FileInfo(formFile.FileName).Extension;
                    var some_val = new Random().Next(1, 1000) + new DateTime().Ticks;
                    path += "\\" + some_val+ ext;
                     baseUri = $"{Request.Scheme}://{Request.Host}"+ "/uploads/" + some_val + ext;
                    using (var stream = new System.IO.FileStream(path, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
           
            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return baseUri;
        }

        [HttpPost("UploadFile"), DisableRequestSizeLimit]
        public async Task<ActionResult> UploadFile([FromForm(Name = "file")] IList<IFormFile> files)
        {
            if (files.Count > 0)
            {
                //var blobs = new List<BlobUploadDTO>();
                //foreach (var file in files)
                //{
                //    using (var ms = new MemoryStream())
                //    {
                //        file.CopyTo(ms);
                //        var fileBytes = ms.ToArray();
                //        string s = Convert.ToBase64String(fileBytes);
                //        var blob = await _fileUploader.UploadFileAsync(file.FileName, fileBytes, "lym-files");
                //        blobs.Add(blob);
                //    }


                //}
              var file=  await this.Upload(files);
                var blobs = new BlobUploadDTO();
                blobs.BlobFileName = files[0].FileName;
                blobs.BlobURI = file;
                OtherConstants.isSuccessful = true;
                return new JsonResult(constructResponse(blobs));

            }
            OtherConstants.isSuccessful = false;
            return new JsonResult(constructResponse((new BlobUploadDTO())));
        }


    }
}

