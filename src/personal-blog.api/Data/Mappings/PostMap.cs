using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using personal_blog.core.Models;

namespace personal_blog.Api.Data.Mappings;

public class PostMap : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Title)
            .IsRequired()
            .HasColumnType("NVARCHAR")
            .HasMaxLength(200);

        builder.Property(p => p.Body)
            .IsRequired()
            .HasColumnType("NVARCHAR(max)");
        
        builder.Property(p => p.Created)
            .IsRequired()
            .HasColumnType("DATETIME");
        
        builder.Property(p => p.Updated)
            .IsRequired()
            .HasColumnType("DATETIME");

        builder.Property(p => p.UserId)
            .IsRequired()
            .HasColumnType("BIGINT");
    }
}