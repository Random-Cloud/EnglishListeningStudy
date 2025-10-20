namespace EnglishListeningStudy.FileService.Domain {

    public interface IStorageClient {
        EStorageType StorageType { get;
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="key">文件路径</param>
        /// <param name="content">文件内容</param>
        /// <param name="cancellationToken">检查是否中途取消</param>
        /// <returns>可以被访问的URL</returns>

        Task<Uri> SaveAsync(string key, Stream content, CancellationToken cancellationToken);
    }
}
