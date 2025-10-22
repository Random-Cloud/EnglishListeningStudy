using EnglishListeningStudy.FileService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EnglishListeningStudy.FileService.Infrastructure.Configs {
    /// <summary>
    /// 配置UploadedItem实体类到数据库表的映射关系
    /// </summary>
    public class UploadedItemConfig : IEntityTypeConfiguration<UploadedItem>{
        public void Configure(EntityTypeBuilder<UploadedItem> builder) {
            builder.ToTable("T_FS_UploadedItems");
            // 取消GUID（主键）默认的聚集索引，减少对插入性能的影响
            builder.HasKey(e => e.Id).IsClustered(false);
            builder.Property(e => e.FileName).IsUnicode().HasMaxLength(1024);
            builder.Property(e => e.FileSHA256Hash).IsUnicode().HasMaxLength(64);
            // 通过文件哈希和文件大小（高频使用）创建复合索引，提高查询效率
            builder.HasIndex(e => new  { e.FileSHA256Hash, e.FileSizeInBytes});

        }
        /*
         *聚集索引特性：索引即数据。直接决定了数据的物理存储顺序。在索引发生变化时，数据的物理存储顺序也会随之变化，因此需要更多的维护开销。
         *普通索引在调整时仅需调整索引本身，而不影响数据的物理存储顺序，维护开销较小。
         */
    }
}
