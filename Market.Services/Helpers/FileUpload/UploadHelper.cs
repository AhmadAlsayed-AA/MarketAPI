using System;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Market.Data.HelperModels;

namespace Market.Services.Helpers.FileUpload
{
    public interface IUploadHelper
    {
        Task<BlobObject> GetBlobFile(string fileName, string container);
        Task<string> UploadBlobFile(FileRequest file, string container);
        void DeleteBlob(string fileName, string container);
        Task<List<string>> ListBlobs(string container);

    }
    public class UploadHelper : IUploadHelper
    {
        private readonly BlobServiceClient _blobServiceClient;
        private BlobContainerClient client;
        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPEG", ".PNG" };

        public UploadHelper(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
            
        }
        public async void DeleteBlob(string fileName, string container)
        {
            client = _blobServiceClient.GetBlobContainerClient(container);

            var blobClient = client.GetBlobClient(fileName);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<BlobObject> GetBlobFile(string fileName, string container)
        {
            client = _blobServiceClient.GetBlobContainerClient(container);

            try
            {
                var blobClient = client.GetBlobClient(fileName);
                if (await blobClient.ExistsAsync())
                {
                    BlobDownloadResult content = await blobClient.DownloadContentAsync();
                    var downloadedData = content.Content.ToStream();

                    if (ImageExtensions.Contains(Path.GetExtension(fileName.ToUpperInvariant())))
                    {
                        var extension = Path.GetExtension(fileName);
                        return new BlobObject { Content = downloadedData, ContentType = "image/" + extension.Remove(0, 1) };
                    }
                    else
                    {
                        return new BlobObject { Content = downloadedData, ContentType = content.Details.ContentType };
                    }

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<string>> ListBlobs(string container)
        {
            client = _blobServiceClient.GetBlobContainerClient(container);

            List<string> lst = new List<string>();

            await foreach (var blobItem in client.GetBlobsAsync())
            {
                lst.Add(blobItem.Name);
            }

            return lst;
        }

        public async Task<string> UploadBlobFile(FileRequest file, string container)
        {
            client = _blobServiceClient.GetBlobContainerClient(container);
            var blobClient = client.GetBlobClient(file.FileName);
            var status =  blobClient.UploadAsync(file.FilePath);

            return file.FileName; 
        }
    
    }
}

