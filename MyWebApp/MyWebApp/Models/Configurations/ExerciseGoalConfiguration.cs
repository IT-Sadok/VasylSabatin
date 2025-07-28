using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyWebApp.Models.Configurations;

public class ExerciseGoalConfiguration : IEntityTypeConfiguration<ExerciseGoal>
{
    public void Configure(EntityTypeBuilder<ExerciseGoal> builder)
    {
        builder.ToTable("ExerciseGoals");
        
        builder.HasKey(eg => eg.Id);
        
        builder.Property(tr => tr.TargetReps)
            .IsRequired();
        
        builder.Property(tw => tw.TargetWeight)
            .IsRequired();
        
        builder.Property(ia => ia.IsAchieved)
            .IsRequired();
        
        builder.HasOne(eg => eg.User)
            .WithMany(u => u.ExerciseGoals)
            .HasForeignKey(eg => eg.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(eg => eg.Exercise)
            .WithMany(e => e.ExerciseGoals)
            .HasForeignKey(eg => eg.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}