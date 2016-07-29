using System;

namespace Imedit.Models
{
    public class Photo
    {
        public Uri PhotoUri { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public string ImageName { get; set; }
        public string FolderName { get; set; }
        public bool HasSubFolder { get { return !string.IsNullOrEmpty(this.FolderName); } }

        public override string ToString()
        {
            return $"{ImageName}${FolderName}${PhotoUri.OriginalString.Substring(0, 10)}";
        }
    }
}