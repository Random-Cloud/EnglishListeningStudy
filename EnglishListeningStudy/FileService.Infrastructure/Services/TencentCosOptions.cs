namespace EnglishListeningStudy.FileService.Infrastructure.Services {
    public class TencentCosOptions {
        public string AccessKeyId {
            get; set;
        }
        public string SecretAccessKey {
            get; set;
        }
        public string BucketName {
            get; set;
        }
        public string Region {
            get; set;
        }
        public string WorkingDir {
        get; set;}
        public string PublicDomain {
        get; set;}
    }
}
