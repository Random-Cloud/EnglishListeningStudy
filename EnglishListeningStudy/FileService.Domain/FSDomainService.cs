using EnglishListeningStudy.FileService.Domain.Entities;
using Zack.Commons;

namespace EnglishListeningStudy.FileService.Domain {
    public class FSDomainService {
        private readonly IFSRepository repository;
        private readonly IStorageClient backupStorage;
        private readonly IStorageClient remoteStorage;

        //依赖注入与关键资源初始化
        public FSDomainService(IFSRepository repository, IEnumerable<IStorageClient> storageClients) {
            this.repository = repository;
            this.backupStorage = storageClients.First(client => client.StorageType == EStorageType.Backup);
            this.remoteStorage = storageClients.First(client => client.StorageType == EStorageType.Public);
        }
        public async Task<UploadedItem> UploadAsync(Stream stream, string fileName,
        CancellationToken cancellationToken) {
            string hash = HashHelper.ComputeSha256Hash(stream);
            long fileSize = stream.Length;
            DateTime today = DateTime.Today;
            string key = $"{today.Year}/{today.Month}/{today.Day}/{hash}/{fileName}";
            var oldUploadItem = await repository.FindFileAsync(fileSize, hash);
            if (oldUploadItem != null) {
                return oldUploadItem;
            }
            // 重置流位置，以便后续存储操作可以从头开始读取流内容
            // 为什么需要重置流位置？因为计算哈希值时，流的位置已经移动到了末尾，必须重置为开头才能正确读取内容进行存储。
            stream.Position = 0;
            Uri backupUrl = await backupStorage.SaveAsync(key, stream, cancellationToken);
            stream.Position = 0;
            Uri remoteUrl = await remoteStorage.SaveAsync(key, stream, cancellationToken);
            stream.Position = 0;
            Guid id = Guid.NewGuid();
            return UploadedItem.Create(
                id,
                fileSize,
                fileName,
                hash,
                backupUrl,
                remoteUrl
            );
        }
    }
    /*
    1. 文件唯一性标识生成
	2. 存储路径（Key）生成
	3. 文件重复校验（防重复上传）
	4. 双存储备份
	5. 生成领域实体（UploadedItem）
     */
}
