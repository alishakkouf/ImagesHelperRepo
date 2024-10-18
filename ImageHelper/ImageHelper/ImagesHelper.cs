using System.Net.Mime;
using System.Net.NetworkInformation;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImageHelper
{
    public static class ImagesHelper
    {
        /// <summary>
        /// Resize image by providing width and height.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async static Task<ImageResult> Resize(string imageUrl, int width, int height, string fileName, ImageFormat contentType = ImageFormat.Png)
        {
            // Download the image from the provided URL
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(imageUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to download image from URL: {imageUrl}");
            }

            // Read the image data as a stream
            using var imageStream = await response.Content.ReadAsStreamAsync();

            // Load the image into memory for processing using ImageSharp
            using var image = Image.Load(imageStream);

            using var memoryStream = new MemoryStream();

            image.Mutate(x => x.Resize(width, height));

            switch (contentType)
            {
                case ImageFormat.Png:
                    image.SaveAsPng(memoryStream);
                    break;
                case ImageFormat.Jpeg:
                    image.SaveAsJpeg(memoryStream);
                    break;
                case ImageFormat.Bmp:
                    image.SaveAsBmp(memoryStream);
                    break;
                default:
                    throw new NotSupportedException("Unsupported contentType.");
            }

            byte[] imageBytes = memoryStream.ToArray();

            return new ImageResult(imageBytes, GetUniqueFileName(fileName, $".{contentType}"), contentType);
        }

        /// <summary>
        /// Resize image By Percentage.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="percentage"></param>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public async static Task<ImageResult> ResizeByPercentage(string imageUrl, float percentage, string fileName, ImageFormat contentType = ImageFormat.Png)
        {
            // Download the image from the provided URL
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(imageUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to download image from URL: {imageUrl}");
            }

            // Read the image data as a stream
            using var imageStream = await response.Content.ReadAsStreamAsync();

            // Load the image into memory for processing using ImageSharp
            using var image = Image.Load(imageStream);

            using (var memoryStream = new MemoryStream())
            {
                var newWidth = (int)(image.Width * percentage);
                var newHeight = (int)(image.Height * percentage);
                image.Mutate(ctx => ctx.Resize(newWidth, newHeight));

                switch (contentType)
                {
                    case ImageFormat.Png:
                        image.SaveAsPng(memoryStream);
                        break;
                    case ImageFormat.Jpeg:
                        image.SaveAsJpeg(memoryStream);
                        break;
                    case ImageFormat.Bmp:
                        image.SaveAsBmp(memoryStream);
                        break;
                    default:
                        throw new NotSupportedException("Unsupported contentType.");
                }

                byte[] imageBytes = memoryStream.ToArray();
                return new ImageResult(imageBytes, GetUniqueFileName(fileName, $".{contentType}"), contentType);
            }
        }

        /// <summary>
        /// Converting Image contentTypes (JPEG, PNG, etc.)
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="contentType"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public async static Task<ImageResult> ChangeFormat(string imageUrl, string fileName, ImageFormat contentType = ImageFormat.Png)
        {
            // Download the image from the provided URL
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(imageUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to download image from URL: {imageUrl}");
            }

            // Read the image data as a stream
            using var imageStream = await response.Content.ReadAsStreamAsync();

            // Load the image into memory for processing using ImageSharp
            using var image = Image.Load(imageStream);

            using var memoryStream = new MemoryStream();
            switch (contentType)
            {
                case ImageFormat.Png:
                    image.SaveAsPng(memoryStream);
                    break;
                case ImageFormat.Jpeg:
                    image.SaveAsJpeg(memoryStream);
                    break;
                case ImageFormat.Bmp:
                    image.SaveAsBmp(memoryStream);
                    break;
                default:
                    throw new NotSupportedException("Unsupported contentType.");
            }

            byte[] imageBytes = memoryStream.ToArray();

            return new ImageResult(imageBytes, GetUniqueFileName(fileName, $".{contentType}"), contentType);
        }

        /// <summary>
        /// Crop image.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public async static Task<ImageResult> Crop(string imageUrl, int x, int y, int width, int height, string fileName, ImageFormat contentType = ImageFormat.Png)
        {
            // Download the image from the provided URL
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(imageUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to download image from URL: {imageUrl}");
            }

            // Read the image data as a stream
            using var imageStream = await response.Content.ReadAsStreamAsync();

            // Load the image into memory for processing using ImageSharp
            using var image = Image.Load(imageStream);

            using (var memoryStream = new MemoryStream())
            {
                image.Mutate(q =>
                {
                    q.Crop(new Rectangle(x, y, width, height));
                });

                switch (contentType)
                {
                    case ImageFormat.Png:
                        image.SaveAsPng(memoryStream);
                        break;
                    case ImageFormat.Jpeg:
                        image.SaveAsJpeg(memoryStream);
                        break;
                    case ImageFormat.Bmp:
                        image.SaveAsBmp(memoryStream);
                        break;
                    default:
                        throw new NotSupportedException("Unsupported contentType.");
                }

                byte[] imageBytes = memoryStream.ToArray();

                return new ImageResult(imageBytes, GetUniqueFileName(fileName, $".{contentType}"), contentType);
            }
        }

        /// <summary>
        /// Rotate image.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="angle"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async static Task<ImageResult> Rotate(string imageUrl, float angle, string fileName, ImageFormat contentType = ImageFormat.Png)
        {
            // Download the image from the provided URL
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(imageUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to download image from URL: {imageUrl}");
            }

            // Read the image data as a stream
            using var imageStream = await response.Content.ReadAsStreamAsync();

            // Load the image into memory for processing using ImageSharp
            using var image = Image.Load(imageStream);

            using var memoryStream = new MemoryStream();

            image.Mutate(x => x.Rotate(angle));

            switch (contentType)
            {
                case ImageFormat.Png:
                    image.SaveAsPng(memoryStream);
                    break;
                case ImageFormat.Jpeg:
                    image.SaveAsJpeg(memoryStream);
                    break;
                case ImageFormat.Bmp:
                    image.SaveAsBmp(memoryStream);
                    break;
                default:
                    throw new NotSupportedException("Unsupported contentType.");
            }

            byte[] imageBytes = memoryStream.ToArray();

            return new ImageResult(imageBytes, GetUniqueFileName(fileName, $".{contentType}"), contentType);
        }

        /// <summary>
        /// Adding text Watermark.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="watermarkText"></param>
        /// <param name="contentType"></param>
        /// <param name="FontName"></param>
        /// <param name="FontSize"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public async static Task<ImageResult> AddWatermark(string imageUrl, string watermarkText, string fileName, ImageFormat contentType = ImageFormat.Png, string FontName = "Arial", int FontSize = 24)
        {
            // Download the image from the provided URL
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(imageUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to download image from URL: {imageUrl}");
            }

            // Read the image data as a stream
            using var imageStream = await response.Content.ReadAsStreamAsync();

            // Load the image into memory for processing using ImageSharp
            using var image = Image.Load(imageStream);

            using var memoryStream = new MemoryStream();

            Font font = SystemFonts.CreateFont(FontName, FontSize);

            image.Mutate(ctx => ctx.DrawText(watermarkText, font, Color.White, new PointF(10, 10)));

            switch (contentType)
            {
                case ImageFormat.Png:
                    image.SaveAsPng(memoryStream);
                    break;
                case ImageFormat.Jpeg:
                    image.SaveAsJpeg(memoryStream);
                    break;
                case ImageFormat.Bmp:
                    image.SaveAsBmp(memoryStream);
                    break;
                default:
                    throw new NotSupportedException("Unsupported contentType.");
            }

            byte[] imageBytes = memoryStream.ToArray();

            return new ImageResult(imageBytes, GetUniqueFileName(fileName, $".{contentType}"), contentType);
        }

        /// <summary>
        /// Convert image To Grayscale
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="fileName"></param>
        /// <param name="contentType"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public async static Task<ImageResult> ConvertToGrayscale(string imageUrl, string fileName, ImageFormat contentType = ImageFormat.Png)
        {
            // Download the image from the provided URL
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(imageUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to download image from URL: {imageUrl}");
            }

            // Read the image data as a stream
            using var imageStream = await response.Content.ReadAsStreamAsync();

            // Load the image into memory for processing using ImageSharp
            using var image = Image.Load(imageStream);

            using (var memoryStream = new MemoryStream())
            {
                image.Mutate(ctx => ctx.Grayscale());

                switch (contentType)
                {
                    case ImageFormat.Png:
                        image.SaveAsPng(memoryStream);
                        break;
                    case ImageFormat.Jpeg:
                        image.SaveAsJpeg(memoryStream);
                        break;
                    case ImageFormat.Bmp:
                        image.SaveAsBmp(memoryStream);
                        break;
                    default:
                        throw new NotSupportedException("Unsupported contentType.");
                }

                byte[] imageBytes = memoryStream.ToArray();
                return new ImageResult(imageBytes, GetUniqueFileName(fileName, $".{contentType}"), contentType);
            }
        }

        /// <summary>
        /// Flip images.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="flipMode"></param>
        /// <param name="contentType"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async static Task<ImageResult> Flip(string imageUrl, FlipMode flipMode, string fileName, ImageFormat contentType = ImageFormat.Png)
        {
            // Download the image from the provided URL
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(imageUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to download image from URL: {imageUrl}");
            }

            // Read the image data as a stream
            using var imageStream = await response.Content.ReadAsStreamAsync();

            // Load the image into memory for processing using ImageSharp
            using var image = Image.Load(imageStream);

            using var memoryStream = new MemoryStream();

            image.Mutate(ctx => ctx.Flip(flipMode));

            switch (contentType)
            {
                case ImageFormat.Png:
                    image.SaveAsPng(memoryStream);
                    break;
                case ImageFormat.Jpeg:
                    image.SaveAsJpeg(memoryStream);
                    break;
                case ImageFormat.Bmp:
                    image.SaveAsBmp(memoryStream);
                    break;
                default:
                    throw new NotSupportedException("Unsupported contentType.");
            }

            byte[] imageBytes = memoryStream.ToArray();

            return new ImageResult(imageBytes, GetUniqueFileName(fileName, $".{contentType}"), contentType);
        }

        /// <summary>
        /// Sharpen image.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="amount"></param>
        /// <param name="contentType"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async static Task<ImageResult> Sharpen(string imageUrl, float amount, string fileName, ImageFormat contentType = ImageFormat.Png)
        {
            // Download the image from the provided URL
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(imageUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to download image from URL: {imageUrl}");
            }

            // Read the image data as a stream
            using var imageStream = await response.Content.ReadAsStreamAsync();

            // Load the image into memory for processing using ImageSharp
            using var image = Image.Load(imageStream);

            using var memoryStream = new MemoryStream();

            image.Mutate(ctx => ctx.GaussianSharpen(amount));

            switch (contentType)
            {
                case ImageFormat.Png:
                    image.SaveAsPng(memoryStream);
                    break;
                case ImageFormat.Jpeg:
                    image.SaveAsJpeg(memoryStream);
                    break;
                case ImageFormat.Bmp:
                    image.SaveAsBmp(memoryStream);
                    break;
                default:
                    throw new NotSupportedException("Unsupported contentType.");
            }

            byte[] imageBytes = memoryStream.ToArray();
            return new ImageResult(imageBytes, GetUniqueFileName(fileName, $".{contentType}"), contentType);
        }

        /// <summary>
        /// Generate Thumbnail
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="thumbnailSize"></param>
        /// <param name="contentType"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async static Task<ImageResult> GenerateThumbnail(string imageUrl, int thumbnailSize, string fileName, ImageFormat contentType = ImageFormat.Png)
        {
            // Download the image from the provided URL
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(imageUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to download image from URL: {imageUrl}");
            }

            // Read the image data as a stream
            using var imageStream = await response.Content.ReadAsStreamAsync();

            // Load the image into memory for processing using ImageSharp
            using var image = Image.Load(imageStream);

            using var memoryStream = new MemoryStream();

            var size = new ResizeOptions
            {
                Size = new Size(thumbnailSize, thumbnailSize),
                Mode = ResizeMode.Max // Maintains aspect ratio
            };

            image.Mutate(ctx => ctx.Resize(size));

            switch (contentType)
            {
                case ImageFormat.Png:
                    image.SaveAsPng(memoryStream);
                    break;
                case ImageFormat.Jpeg:
                    image.SaveAsJpeg(memoryStream);
                    break;
                case ImageFormat.Bmp:
                    image.SaveAsBmp(memoryStream);
                    break;
                default:
                    throw new NotSupportedException("Unsupported contentType.");
            }

            byte[] imageBytes = memoryStream.ToArray();
            return new ImageResult(imageBytes, GetUniqueFileName(fileName, $".{contentType}"), contentType);
        }

        /// <summary>
        /// Generate QR code.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="size"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public async static Task<ImageResult> GenerateQrCode(string text, int size, string fileName, ImageFormat contentType = ImageFormat.Png)
        {
            var qrCodeData = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = size,
                    Width = size
                }
            }.Write(text);

            using var image = Image.LoadPixelData<Rgba32>(qrCodeData.Pixels, qrCodeData.Width, qrCodeData.Height);

            using var memoryStream = new MemoryStream();

            switch (contentType)
            {
                case ImageFormat.Png:
                    image.SaveAsPng(memoryStream);
                    break;
                case ImageFormat.Jpeg:
                    image.SaveAsJpeg(memoryStream);
                    break;
                case ImageFormat.Bmp:
                    image.SaveAsBmp(memoryStream);
                    break;
                default:
                    throw new NotSupportedException("Unsupported contentType.");
            }

            byte[] imageBytes = memoryStream.ToArray();
            return new ImageResult(imageBytes, GetUniqueFileName(fileName, $".{contentType}"), contentType);
        }

        public static string GetUniqueFileName(string fileName, string extension = ".png")
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = Guid.NewGuid().ToString();

            // Append the extension (defaults to .png if not provided)
            return $"{fileName}{extension}";
        }
    }

}
