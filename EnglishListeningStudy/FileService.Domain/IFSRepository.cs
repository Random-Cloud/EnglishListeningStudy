using EnglishListeningStudy.FileService.Domain.Entities;

namespace EnglishListeningStudy.FileService.Domain {
    public interface IFSRepository {
        Task<UploadedItem?> FindFileAsync(long fileSizeInBytes, string fileSHA256Hash);
    }
}
