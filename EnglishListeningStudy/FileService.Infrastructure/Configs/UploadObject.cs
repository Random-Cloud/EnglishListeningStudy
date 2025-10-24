using COSXML;
using COSXML.Auth;
using EnglishListeningStudy.FileService.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EnglishListeningStudy.FileService.Infrastructure.Configs {
    public class UploadObject {
        private static readonly Lazy<UploadObject> _instance = new Lazy<UploadObject>(() => new UploadObject());
        public static UploadObject Instance => _instance.Value;

        public CosXml cosXml { get; private set;}
        private TencentCosOptions _options;
        public TencentCosOptions Options => _options;
        private UploadObject() {
            string json = File.ReadAllText("Configs/CosSettings.json");
            _options = JsonConvert.DeserializeObject<TencentCosOptions>(json);
            InitCosXml();
        }

        
        private void InitCosXml() {
            
            string region = Environment.GetEnvironmentVariable(_options.Region);
            CosXmlConfig config = new CosXmlConfig.Builder()
                .SetRegion(region) // 设置默认的地域, COS 地域的简称请参照 https://cloud.tencent.com/document/product/436/6224
                .Build();
            string secretId = Environment.GetEnvironmentVariable(_options.AccessKeyId); // 云 API 密钥 SecretId, 获取 API 密钥请参照 https://console.cloud.tencent.com/cam/capi
            string secretKey = Environment.GetEnvironmentVariable(_options.SecretAccessKey); // 云 API 密钥 SecretKey, 获取 API 密钥请参照 https://console.cloud.tencent.com/cam/capi
            long durationSecond = 600; //每次请求签名有效时长，单位为秒
            QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(secretId, secretKey, durationSecond);
            this.cosXml = new CosXmlServer(config, qCloudCredentialProvider);
        }
    }
}
