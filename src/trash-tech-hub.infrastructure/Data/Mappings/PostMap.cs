using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrashTechHub.Core.Models;

namespace TrashTechHub.Infrastructure.Data.Mappings;

public class PostMap : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Body)
            .IsRequired();

        builder.Property(p => p.Created)
            .IsRequired();

        builder.Property(p => p.Updated);

        builder.Property(p => p.IsFeatured)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.Slug)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Excerpt)
            .IsRequired()
            .HasMaxLength(300);

        builder.HasIndex(p => p.Slug)
            .IsUnique();
    }
}