using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyWebApp.Models.Configurations;

public class BodyGoalConfiguration : IEntityTypeConfiguration<BodyGoal>
{
    public void Configure(EntityTypeBuilder<BodyGoal> builder)
    {
        builder.ToTable("BodyGoal");
        
        builder.HasKey(bg => bg.Id);
        
        builder.Property(gt => gt.GoalType)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(tv => tv.TargetValue)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(u => u.Unit)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(ia => ia.IsAchieved)
            .IsRequired();
        
        builder.HasOne(bg => bg.User)
            .WithMany(u => u.BodyGoals)
            .HasForeignKey(bg => bg.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}