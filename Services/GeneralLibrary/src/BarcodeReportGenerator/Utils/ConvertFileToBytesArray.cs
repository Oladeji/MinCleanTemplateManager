using Microsoft.AspNetCore.Http;


namespace ReportGeneratorLib.Utils
{
    public static class ConvertFileToBytesArray
    {
        public static async Task<byte[]> ConvertToByteArrayAsync(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
