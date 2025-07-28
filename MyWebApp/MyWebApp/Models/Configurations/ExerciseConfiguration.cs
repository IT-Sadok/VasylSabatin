using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebApp.Constants;

namespace MyWebApp.Models.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(FieldLengths.DefaultText);
        
        builder.Property(c => c.Category)
            .IsRequired()
            .HasMaxLength(FieldLengths.DefaultText);
        
        builder.Property(e => e.Description)
            .HasMaxLength(FieldLengths.LongText);
        
        builder.HasMany(e => e.ExerciseGoals)
            .WithOne(g => g.Exercise)
            .HasForeignKey(g => g.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}