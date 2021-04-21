using System;
using Enfo.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Enfo.Infrastructure.Migrations
{
    [DbContext(typeof(EnfoDbContext))]
    partial class EnfoDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EnforcementOrders.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("PostalCode")
                        .HasMaxLength(10);

                    b.Property<string>("State")
                        .HasMaxLength(2);

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Street2")
                        .HasMaxLength(100);

                    b.Property<Guid?>("UpdatedById");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("EnforcementOrders.Models.County", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CountyName")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("Counties");
                });

            modelBuilder.Entity("EnforcementOrders.Models.EnforcementOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Cause")
                        .HasMaxLength(3990);

                    b.Property<int?>("CommentContactId");

                    b.Property<DateTime?>("CommentPeriodClosesDate");

                    b.Property<string>("County")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<bool>("Deleted");

                    b.Property<DateTime?>("ExecutedDate");

                    b.Property<DateTime?>("ExecutedOrderPostedDate");

                    b.Property<string>("FacilityName")
                        .IsRequired()
                        .HasMaxLength(205);

                    b.Property<DateTime?>("HearingCommentPeriodClosesDate");

                    b.Property<int?>("HearingContactId");

                    b.Property<DateTime?>("HearingDate");

                    b.Property<string>("HearingLocation")
                        .HasMaxLength(3990);

                    b.Property<bool>("IsExecutedOrder");

                    b.Property<bool>("IsHearingScheduled");

                    b.Property<bool>("IsProposedOrder");

                    b.Property<int?>("LegalAuthorityId")
                        .IsRequired();

                    b.Property<string>("OrderNumber")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ProposedOrderPostedDate");

                    b.Property<int>("PublicationStatus");

                    b.Property<string>("Requirements")
                        .HasMaxLength(3990);

                    b.Property<decimal?>("SettlementAmount")
                        .HasColumnType("money");

                    b.Property<Guid?>("UpdatedById");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("CommentContactId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("HearingContactId");

                    b.HasIndex("LegalAuthorityId");

                    b.HasIndex("UpdatedById");

                    b.ToTable("EnforcementOrders");
                });

            modelBuilder.Entity("EnforcementOrders.Models.EnfoUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime?>("LastLoggedInDate");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime?>("LastPasswordChangedDate");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(100);

                    b.Property<bool>("RequirePasswordChange");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("EnfoUsers");
                });

            modelBuilder.Entity("EnforcementOrders.Models.EpdContact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int?>("AddressId")
                        .IsRequired();

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Email")
                        .HasMaxLength(100);

                    b.Property<string>("Organization")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Telephone")
                        .HasMaxLength(50);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<Guid?>("UpdatedById");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("EpdContacts");
                });

            modelBuilder.Entity("EnforcementOrders.Models.LegalAuthority", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("AuthorityName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("OrderNumberTemplate")
                        .HasMaxLength(40);

                    b.Property<Guid?>("UpdatedById");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("LegalAuthorities");
                });

            modelBuilder.Entity("EnforcementOrders.Models.Address", b =>
                {
                    b.HasOne("EnforcementOrders.Models.EnfoUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("EnforcementOrders.Models.EnfoUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });

            modelBuilder.Entity("EnforcementOrders.Models.EnforcementOrder", b =>
                {
                    b.HasOne("EnforcementOrders.Models.EpdContact", "CommentContact")
                        .WithMany()
                        .HasForeignKey("CommentContactId");

                    b.HasOne("EnforcementOrders.Models.EnfoUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("EnforcementOrders.Models.EpdContact", "HearingContact")
                        .WithMany()
                        .HasForeignKey("HearingContactId");

                    b.HasOne("EnforcementOrders.Models.LegalAuthority", "LegalAuthority")
                        .WithMany()
                        .HasForeignKey("LegalAuthorityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EnforcementOrders.Models.EnfoUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });

            modelBuilder.Entity("EnforcementOrders.Models.EpdContact", b =>
                {
                    b.HasOne("EnforcementOrders.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EnforcementOrders.Models.EnfoUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("EnforcementOrders.Models.EnfoUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });

            modelBuilder.Entity("EnforcementOrders.Models.LegalAuthority", b =>
                {
                    b.HasOne("EnforcementOrders.Models.EnfoUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("EnforcementOrders.Models.EnfoUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });
        }
    }
}
