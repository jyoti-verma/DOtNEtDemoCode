﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartHospital.Letters.Context;

#nullable disable

namespace SmartHospital.Letters.Context.Migrations
{
    [DbContext(typeof(LetterDbContext))]
    [Migration("20230828070551_AdditionalProperty")]
    partial class AdditionalProperty
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("SmartHospital.Letters.Domain.LetterUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Salutation")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.BaseClass", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable((string)null);

                    b.UseTpcMappingStrategy();
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.KeyValue", b =>
                {
                    b.HasBaseType("SmartHospital.Letters.Entities.BaseClass");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("KeyType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SnippetId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ValueType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasIndex("SnippetId");

                    b.ToTable("Values");
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.Letter", b =>
                {
                    b.HasBaseType("SmartHospital.Letters.Entities.BaseClass");

                    b.Property<int>("AdmissionType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExternalCaseNumber")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("TEXT");

                    b.Property<string>("ExternalPatientId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("LetterTypeId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasIndex("LetterTypeId");

                    b.ToTable("Letters");
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.LetterTemplate", b =>
                {
                    b.HasBaseType("SmartHospital.Letters.Entities.BaseClass");

                    b.Property<Guid>("LetterTypeId")
                        .HasColumnType("TEXT");

                    b.HasIndex("LetterTypeId");

                    b.ToTable("LetterTemplates");
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.LetterTemplateSectionTemplate", b =>
                {
                    b.HasBaseType("SmartHospital.Letters.Entities.BaseClass");

                    b.Property<Guid>("LetterTemplateId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SectionTemplateId")
                        .HasColumnType("TEXT");

                    b.Property<int>("SortOrder")
                        .HasColumnType("INTEGER");

                    b.HasIndex("LetterTemplateId");

                    b.HasIndex("SectionTemplateId");

                    b.ToTable("LetterTemplatesSectionTemplates");
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.LetterType", b =>
                {
                    b.HasBaseType("SmartHospital.Letters.Entities.BaseClass");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.ToTable("LetterTypes");
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.Section", b =>
                {
                    b.HasBaseType("SmartHospital.Letters.Entities.BaseClass");

                    b.Property<Guid>("LetterId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SectionTypeId")
                        .HasColumnType("TEXT");

                    b.Property<int>("SortOrder")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasIndex("LetterId");

                    b.HasIndex("SectionTypeId");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.SectionTemplate", b =>
                {
                    b.HasBaseType("SmartHospital.Letters.Entities.BaseClass");

                    b.Property<Guid>("SectionTypeId")
                        .HasColumnType("TEXT");

                    b.HasIndex("SectionTypeId");

                    b.ToTable("SectionTemplates");
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.SectionType", b =>
                {
                    b.HasBaseType("SmartHospital.Letters.Entities.BaseClass");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.ToTable("SectionTypes");
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.Snippet", b =>
                {
                    b.HasBaseType("SmartHospital.Letters.Entities.BaseClass");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SectionId")
                        .HasColumnType("TEXT");

                    b.HasIndex("SectionId");

                    b.ToTable("Snippets");
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.SnippetTemplate", b =>
                {
                    b.HasBaseType("SmartHospital.Letters.Entities.BaseClass");

                    b.Property<string>("DataType")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SectionTemplateId")
                        .HasColumnType("TEXT");

                    b.HasIndex("SectionTemplateId");

                    b.ToTable("SnippetTemplates");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("SmartHospital.Letters.Domain.LetterUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("SmartHospital.Letters.Domain.LetterUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHospital.Letters.Domain.LetterUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("SmartHospital.Letters.Domain.LetterUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.KeyValue", b =>
                {
                    b.HasOne("SmartHospital.Letters.Entities.Snippet", "Snippet")
                        .WithMany("KeyValues")
                        .HasForeignKey("SnippetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Snippet");
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.Letter", b =>
                {
                    b.HasOne("SmartHospital.Letters.Entities.LetterType", "LetterType")
                        .WithMany()
                        .HasForeignKey("LetterTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LetterType");
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.LetterTemplate", b =>
                {
                    b.HasOne("SmartHospital.Letters.Entities.LetterType", "LetterType")
                        .WithMany()
                        .HasForeignKey("LetterTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LetterType");
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.LetterTemplateSectionTemplate", b =>
                {
                    b.HasOne("SmartHospital.Letters.Entities.LetterTemplate", "LetterTemplate")
                        .WithMany("LetterTemplatesSectionTemplates")
                        .HasForeignKey("LetterTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHospital.Letters.Entities.SectionTemplate", "SectionTemplate")
                        .WithMany("LetterTemplateSectionTemplates")
                        .HasForeignKey("SectionTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LetterTemplate");

                    b.Navigation("SectionTemplate");
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.Section", b =>
                {
                    b.HasOne("SmartHospital.Letters.Entities.Letter", "Letter")
                        .WithMany("Sections")
                        .HasForeignKey("LetterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartHospital.Letters.Entities.SectionType", "SectionType")
                        .WithMany()
                        .HasForeignKey("SectionTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Letter");

                    b.Navigation("SectionType");
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.SectionTemplate", b =>
                {
                    b.HasOne("SmartHospital.Letters.Entities.SectionType", "SectionType")
                        .WithMany()
                        .HasForeignKey("SectionTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SectionType");
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.Snippet", b =>
                {
                    b.HasOne("SmartHospital.Letters.Entities.Section", "Section")
                        .WithMany("Snippets")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Section");
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.SnippetTemplate", b =>
                {
                    b.HasOne("SmartHospital.Letters.Entities.SectionTemplate", "SectionTemplate")
                        .WithMany("SnippetTemplates")
                        .HasForeignKey("SectionTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SectionTemplate");
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.Letter", b =>
                {
                    b.Navigation("Sections");
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.LetterTemplate", b =>
                {
                    b.Navigation("LetterTemplatesSectionTemplates");
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.Section", b =>
                {
                    b.Navigation("Snippets");
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.SectionTemplate", b =>
                {
                    b.Navigation("LetterTemplateSectionTemplates");

                    b.Navigation("SnippetTemplates");
                });

            modelBuilder.Entity("SmartHospital.Letters.Entities.Snippet", b =>
                {
                    b.Navigation("KeyValues");
                });
#pragma warning restore 612, 618
        }
    }
}
