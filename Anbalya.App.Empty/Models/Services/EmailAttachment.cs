using System;

namespace Models.Services
{
    public class EmailAttachment
    {
        public EmailAttachment(string fileName, string contentType, byte[] content)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name is required.", nameof(fileName));
            }

            if (content is null || content.Length == 0)
            {
                throw new ArgumentException("Attachment content cannot be empty.", nameof(content));
            }

            FileName = fileName;
            ContentType = string.IsNullOrWhiteSpace(contentType) ? "application/octet-stream" : contentType;
            Content = content;
        }

        public string FileName { get; }
        public string ContentType { get; }
        public byte[] Content { get; }
    }
}
