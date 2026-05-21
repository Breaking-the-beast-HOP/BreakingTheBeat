using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicCatalogContext.Domain.Entities;

namespace MusicCatalogContext.Infrastructure.Configurations
{
    public class LabelConfiguration : IEntityTypeConfiguration<Label>
    {
        public void Configure(EntityTypeBuilder<Label> entity)
        {
            entity.ToTable("Label");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedNever();

            entity.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.LabelCode)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Website);

            entity.Property(x => x.InstagramUrl);

            entity.Property(x => x.TikTokUrl);

            entity.Property(x => x.YouTubeUrl);

            entity.Property(x => x.FacebookUrl);

            entity.Property(x => x.CountryId)
                .IsRequired();

            entity.Property(x => x.PrimaryLanguageId);

            entity.Property(x => x.LogoUrl);

            entity.HasOne(x => x.Country)
                .WithMany()
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.PrimaryLanguage)
                .WithMany()
                .HasForeignKey(x => x.PrimaryLanguageId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.OwnsOne(x => x.ContactPerson, contact =>
            {
                contact.Property(x => x.FullName)
                    .HasColumnName("ContactPerson_FullName")
                    .HasMaxLength(200);

                contact.Property(x => x.Email)
                    .HasColumnName("ContactPerson_Email")
                    .HasMaxLength(200);

                contact.Property(x => x.PhoneNumber)
                    .HasColumnName("ContactPerson_PhoneNumber")
                    .HasMaxLength(50);

                contact.Property(x => x.Role)
                    .HasColumnName("ContactPerson_Role")
                    .HasMaxLength(200);

                contact.Property(x => x.PreferredLanguageId)
                    .HasColumnName("ContactPerson_PreferredLanguageId");

                contact.HasOne(x => x.PreferredLanguage)
                    .WithMany()
                    .HasForeignKey(x => x.PreferredLanguageId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            entity.Navigation(x => x.ContactPerson)
                .IsRequired();
        }
    }
}