using PanoramBackend.Services.Core;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static NukesLab.Core.Common.Constants;
using Azure.Storage.Blobs;
using Azure;
using PanoramBackend.Services.Services;
using Azure.Storage.Blobs.Models;

namespace PanoramBackend.Services
{
    public class AzureBlobUploader : IFileUploader
    {
        public async Task<BlobUploadDTO> UploadFileAsync(string strFileName, byte[] byteArray, string containerName)
        {
            BlobUploadDTO image = new BlobUploadDTO();
            var stream = new MemoryStream(byteArray);
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(Blobs.ConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }
            if (strFileName != null)
            {
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(strFileName);
                await cloudBlockBlob.UploadFromStreamAsync(stream);

                image.BlobFileName = cloudBlockBlob.Name;
                image.BlobURI = cloudBlockBlob.Uri.AbsoluteUri;
                return image;
            }
            return image;
        }

        public async Task<bool> DeleteFileAsync(string uniqueFileIdentifier, string containerName)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(Blobs.ConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);

            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(uniqueFileIdentifier);
            return await blob.DeleteIfExistsAsync();
        }


        public void Dispose()
        {
           //
        }

        public async Task<bool> CreateContainer(string tenantId)
        {

                try
            {
                BlobServiceClient blobServiceClient= new BlobServiceClient(Blobs.ConnectionString);
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(Blobs.ConnectionString);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                // Create the container

                BlobContainerClient container = await blobServiceClient.CreateBlobContainerAsync(tenantId);

                    if (await container.ExistsAsync())
                    {
                         return true;
                    }
                return false;
            }
                catch (RequestFailedException e)
                {
                    throw new ServiceException("HTTP error code {0}: {1}",
                                        e.Status.ToString(), e.ErrorCode);
                  
             
                }

           
            }
        public async Task<bool> DeleteContainer(string tenantId)
        {

            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(Blobs.ConnectionString);
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(Blobs.ConnectionString);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                // Create the container
  
                var response = await blobServiceClient.DeleteBlobContainerAsync(tenantId);

                if (response!=null)
                {
                    return true;
                }
                return false;
            }
            catch (RequestFailedException e)
            {
                throw new ServiceException("HTTP error code {0}: {1}",
                                    e.Status.ToString(), e.ErrorCode);


            }


        }

    }
    public interface IFileUploader: IFileUploaderService
    {
        Task<BlobUploadDTO> UploadFileAsync(string strFileName, byte[] byteArray, string containerName);
        Task<bool> DeleteFileAsync(string uniqueFileIdentifier, string containerName);
        Task<bool> CreateContainer(string tenantId);
        Task<bool> DeleteContainer(string tenantId);
    }
    public class BlobUploadDTO
    {
        public string BlobFileName { get; set; }
        public string BlobURI { get; set; }
    }

    public interface IFileUploaderService : IBaseService
    {

    }
}
