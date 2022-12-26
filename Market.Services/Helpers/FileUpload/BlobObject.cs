using System;
namespace Market.Services.Helpers.FileUpload
{
    public class BlobObject
    {
        public Stream? Content { get; set; }
        public string? ContentType { get; set; }
    }
}

