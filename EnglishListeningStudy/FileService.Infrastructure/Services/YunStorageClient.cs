using COSXML;
using COSXML.Transfer;
using EnglishListeningStudy.FileService.Domain;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Zack.Commons;
using EnglishListeningStudy.FileService.Infrastructure.Configs;

namespace EnglishListeningStudy.FileService.Infrastructure.Services {
    // 先用cloudstorageclient测试，跑通了再测这个
    public class YunStorageClient : IStorageClient {
        public EStorageType StorageType => EStorageType.Public;
        // private IOptionsSnapshot<FTPStorageOptions> options;
        private IOptionsSnapshot<TencentCosOptions> options;
        private IHttpClientFactory httpClientFactory;


        public YunStorageClient(IOptionsSnapshot<TencentCosOptions> options,
            IHttpClientFactory httpClientFactory) {
            this.options = options;
            this.httpClientFactory = httpClientFactory;
        }
        static string ConcatUrl(params string[] segments) {
            for (int i = 0; i < segments.Length; i++) {
                string s = segments[i];
                if(s.Contains("..")) {
                    throw new ArgumentException("Invalid URL segment.路径中不能含有'..'", nameof(segments));
                }
                segments[i] = s.Trim('/');
            }
            return string.Join("/", segments);
        }


        public async Task<Uri> SaveAsync(string key, Stream content, CancellationToken cancellationToken) {
            if(key.StartsWith("/")) {
                throw new ArgumentException("Key不能以'/'开头", nameof(key));
            }
            byte[] bytes = content.ToArray();
            if(bytes.Length <= 0) {
                throw new ArgumentException("Content不能为空", nameof(content));
            }

            string WorkingDir = options.Value.WorkingDir;
            string PublicDomain = options.Value.PublicDomain;

            string url = ConcatUrl( PublicDomain, WorkingDir, key);
            TransferUploadFile().Wait();
            return new Uri(url);


        }
        //TODO:累了，先直接写，后面写完了再优化
        public async Task TransferUploadFile() {
            TransferConfig transferConfig = new TransferConfig();
            // 手动设置开始分块上传的大小阈值为10MB，默认值为5MB
            transferConfig.DivisionForUpload = 10 * 1024 * 1024;
            // 手动设置分块上传中每个分块的大小为2MB，默认值为1MB
            transferConfig.SliceSizeForUpload = 2 * 1024 * 1024;

            CosXml cosXml = UploadObject.Instance.cosXml;
            // 初始化 TransferManager
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);
            // 存储桶名称，此处填入格式必须为 BucketName-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
            String bucket = "examplebucket-1250000000";
            String cosPath = "exampleobject"; //对象在存储桶中的位置标识符，即称对象键
            String srcPath = "temp-source-file";//本地文件绝对路径  
                                                // 上传对象
            COSXMLUploadTask uploadTask = new COSXMLUploadTask(bucket, cosPath);
            uploadTask.SetSrcPath(srcPath);
            uploadTask.progressCallback = delegate (long completed, long total)
            {
                Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };
            //开始上传
            try {
                COSXMLUploadTask.UploadTaskResult result = await transferManager.UploadAsync(uploadTask);
                Console.WriteLine(result.GetResultInfo());
            }
            catch (Exception e) {
                Console.WriteLine("CosException: " + e);
            }
        }

        
    }
}
