using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyWebApp.Models.Configurations;

public class WorkoutConfiguration : IEntityTypeConfiguration<Workout>
{
    public void Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.ToTable("Workout");
        
        builder.HasKey(w => w.Id);
        
        builder.Property(w => w.DateOfTraining)
            .IsRequired();

        builder.Property(w => w.Notes)
            .HasMaxLength(1000);
        
        builder.HasOne(w => w.User)
            .WithMany(u => u.Workouts)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(w => w.WorkoutExercises)
            .WithOne(we => we.Workout)
            .HasForeignKey(we => we.WorkoutId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}