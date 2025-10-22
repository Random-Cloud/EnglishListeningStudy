using EnglishListeningStudy.FileService.Domain;
using Microsoft.Extensions.Options;

namespace EnglishListeningStudy.FileService.Infrastructure.Services {
    /// <summary>
    /// 用其他磁盘模拟备份服务器
    /// </summary>
    public class SMBStorageClient : IStorageClient {
        private IOptionsSnapshot<SMBStorageOptions> options;
        public SMBStorageClient(IOptionsSnapshot<SMBStorageOptions> options) {
            this.options = options;
        }
        public EStorageType StorageType => EStorageType.Backup;

        public async Task<Uri> SaveAsync(string key, Stream content, CancellationToken cancellationToken = default) {
            if(key.StartsWith('/')) {
                throw new ArgumentException("Key不能以/开头", nameof(key));
            }
            string workingDir = options.Value.WorkingDir;
            string fullPath = Path.Combine(workingDir, key);
            string? fullDir = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(fullDir))
            {
                Directory.CreateDirectory(fullDir);
            }
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            using Stream outStream = File.OpenWrite(fullPath);
            await content.CopyToAsync(outStream, cancellationToken);
            return new Uri(fullPath);
        }
    }
}
