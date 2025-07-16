using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using personal_blog.core.Models;

namespace personal_blog.Api.Data.Mappings;

public class CategoryMap : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Title)
            .HasColumnType("NVARCHAR")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Slug)
            .HasColumnType("NVARCHAR")
            .IsRequired()
            .HasMaxLength(100);
        
        builder.HasMany(c => c.Posts)
            .WithOne(p => p.Category)
            .OnDelete(DeleteBehavior.Cascade);
    }   
}