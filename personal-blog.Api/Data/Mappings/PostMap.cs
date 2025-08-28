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
            .HasColumnType("NVARCHAR")
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(p => p.Body)
            .HasColumnType("NVARCHAR")
            .IsRequired();

        builder.Property(p => p.Created)
            .IsRequired()
            .HasColumnType("DATETIME");
        
        builder.Property(p => p.Updated)
            .IsRequired()
            .HasColumnType("DATETIME");
        
        builder.Property(p => p.CategoryId)
            .HasColumnType("INTEGER")
            .IsRequired();
        
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Posts)
            .HasForeignKey(p => p.CategoryId);;
        
        builder.Property(p => p.UserId)
            .HasColumnType("BIGINT")
            .IsRequired();
        
        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId);
    }
}