namespace EnglishListeningStudy.FileService.Domain.Entities {
   // UploadedItem上传项，每次用户上传一个文件时，都会创建一个UploadedItem实体。
    public class UploadedItem {
        public Guid Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public long FileSizeInBytes { get; private set; }
        public string FileName { get; private set; }
        public string FileSHA256Hash { get; private set; }
        public Uri BackupUrl { get; private set; }
        public Uri RemoteUrl { get; private set; }
        public static UploadedItem Create(
            Guid id,
            long fileSizeInBytes,
            string fileName,
            string fileSHA256Hash,
            Uri backupUrl,
            Uri remoteUrl
        ) {
            return new UploadedItem {
                Id = id,
                CreatedAt = DateTime.Now,
                FileSizeInBytes = fileSizeInBytes,
                FileName = fileName,
                FileSHA256Hash = fileSHA256Hash,
                BackupUrl = backupUrl,
                RemoteUrl = remoteUrl
            };
        }
    }
}

/*
 * Guid 主键
 * DateTime 创建时间
 * long FileSizeInBytes 文件大小(单位：字节)
 * string FileName 上传的原始文件名
 * string FileSHA256Hash 文件的SHA256哈希值,只要hash和size相同,就认为是同一个文件
 * Uri BackupUrl 备份文件的访问地址
 * Uri RemoteUrl 文件的外部访问地址
 */
