using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebApp.Constants;

namespace MyWebApp.Models.Configurations;

public class BodyGoalConfiguration : IEntityTypeConfiguration<BodyGoal>
{
    public void Configure(EntityTypeBuilder<BodyGoal> builder)
    {
        builder.HasKey(bg => bg.Id);
        
        builder.Property(gt => gt.GoalType)
            .IsRequired()
            .HasMaxLength(FieldLengths.DefaultText);
        
        builder.Property(tv => tv.TargetValue)
            .IsRequired()
            .HasMaxLength(FieldLengths.DefaultText);
        
        builder.Property(u => u.Unit)
            .IsRequired()
            .HasMaxLength(FieldLengths.ShortText);

        builder.Property(ia => ia.IsAchieved)
            .IsRequired();
        
        builder.HasOne(bg => bg.User)
            .WithMany(u => u.BodyGoals)
            .HasForeignKey(bg => bg.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}