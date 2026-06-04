using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrashTechHub.Core.Models;

namespace TrashTechHub.Infrastructure.Data.Mappings;

public class ProjectMap : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Title)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.Property(p => p.Description)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(p => p.Summary)
            .HasMaxLength(600)
            .IsRequired();
        
        builder.Property(p => p.ImageUrl)
            .HasMaxLength(2000)
            .IsRequired(false);
        
        builder.Property(p => p.RepoLink)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(p => p.Slug)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(p => p.Slug)
            .IsUnique();
    }
}