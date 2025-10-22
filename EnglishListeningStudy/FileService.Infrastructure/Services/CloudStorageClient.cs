using EnglishListeningStudy.FileService.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;

namespace EnglishListeningStudy.FileService.Infrastructure.Services {
    public class CloudStorageClient : IStorageClient {
        public EStorageType StorageType => EStorageType.Public;
        private readonly IWebHostEnvironment hostEnv;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CloudStorageClient( IWebHostEnvironment hostEnv, IHttpContextAccessor httpContextAccessor) {
            this.hostEnv = hostEnv;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<Uri> SaveAsync(string key, Stream content, CancellationToken cancellationToken = default) {
            // 避免拼接时出现双斜杠
            if (key.StartsWith('/') ) {
                throw new ArgumentException("Key不能以/开头", nameof(key));
            }
            string workingDir = Path.Combine(hostEnv.ContentRootPath, "wwwroot");
            string fullPath = Path.Combine(workingDir, key);
            string? fullDir = Path.GetDirectoryName(fullPath);

            if (!Directory.Exists(fullDir)) {
                Directory.CreateDirectory(fullDir!);
            }
            if (File.Exists(fullPath)) {
                File.Delete(fullPath);
            }
            using Stream outStream = File.OpenWrite(fullPath);
            await content.CopyToAsync(outStream, cancellationToken);
            var request = httpContextAccessor.HttpContext?.Request;
            string url = $"{request?.Scheme}://{request?.Host}/{key.Replace("\\", "/")}";
            return new Uri(url);
        }
    }
}
