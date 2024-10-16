using ImageHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.IO;

public class ImageResult
{
    public byte[] ByteArray { get; set; }
    public string Base64 { get; set; }
    public IFormFile FormFile { get; set; }

    // Constructor that populates all fields, including a dynamic content type
    public ImageResult(byte[] byteArray, string fileName, ImageFormat contentType)
    {
        ByteArray = byteArray;
        Base64 = Convert.ToBase64String(byteArray);

        // Create an IFormFile from the byte array with the provided content type
        var stream = new MemoryStream(byteArray);

        FormFile = new FormFile(stream, 0, byteArray.Length, "image", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType.ToString() // Dynamic content type from the caller
        };
    }
}
