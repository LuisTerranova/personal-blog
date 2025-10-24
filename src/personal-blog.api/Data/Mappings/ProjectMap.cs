using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using personal_blog.core.Models;

namespace personal_blog.Api.Data.Mappings;

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
            .HasMaxLength(200)
            .IsRequired();
        
        builder.Property(p => p.ImageUrl)
            .HasMaxLength(2000)
            .IsRequired(false);
        
        builder.Property(p => p.RepoLink)
            .HasMaxLength(2000)
            .IsRequired();
        
        builder.Property(p => p.UserId)
            .IsRequired();
    }
}