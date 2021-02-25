﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PluralVideos.Data.Persistence;

namespace PluralVideos.Data.Migrations
{
    [DbContext(typeof(PluralVideosContext))]
    partial class PluralVideosContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.3");

            modelBuilder.Entity("PluralVideos.Data.Models.Clip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClipIndex")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DurationInMilliseconds")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ModuleId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("SupportsStandard")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SupportsWidescreen")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ModuleId");

                    b.ToTable("Clip");
                });

            modelBuilder.Entity("PluralVideos.Data.Models.Course", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("AuthorsFullnames")
                        .HasColumnType("TEXT");

                    b.Property<string>("CachedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("DefaultImageUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("DurationInMilliseconds")
                        .HasColumnType("INTEGER");

                    b.Property<int>("HasTranscript")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("TEXT");

                    b.Property<int?>("IsStale")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Level")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReleaseDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("ShortDescription")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdatedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("UrlSlug")
                        .HasColumnType("TEXT");

                    b.HasKey("Name");

                    b.ToTable("Course");
                });

            modelBuilder.Entity("PluralVideos.Data.Models.Module", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AuthorHandle")
                        .HasColumnType("TEXT");

                    b.Property<string>("CourseName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("DurationInMilliseconds")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ModuleIndex")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CourseName");

                    b.ToTable("Module");
                });

            modelBuilder.Entity("PluralVideos.Data.Models.Transcript", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClipId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EndTime")
                        .HasColumnType("integer");

                    b.Property<int>("StartTime")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClipId");

                    b.HasIndex("StartTime")
                        .HasDatabaseName("index_ClipTranscriptStart");

                    b.ToTable("ClipTranscript");
                });

            modelBuilder.Entity("PluralVideos.Data.Models.User", b =>
                {
                    b.Property<string>("Jwt")
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceId")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("JwtExpiration")
                        .HasColumnType("TEXT");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserHandle")
                        .HasColumnType("TEXT");

                    b.HasKey("Jwt");

                    b.ToTable("User");
                });

            modelBuilder.Entity("PluralVideos.Data.Models.Clip", b =>
                {
                    b.HasOne("PluralVideos.Data.Models.Module", "Module")
                        .WithMany("Clip")
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Module");
                });

            modelBuilder.Entity("PluralVideos.Data.Models.Module", b =>
                {
                    b.HasOne("PluralVideos.Data.Models.Course", "Course")
                        .WithMany("Module")
                        .HasForeignKey("CourseName")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Course");
                });

            modelBuilder.Entity("PluralVideos.Data.Models.Transcript", b =>
                {
                    b.HasOne("PluralVideos.Data.Models.Clip", "Clip")
                        .WithMany("ClipTranscript")
                        .HasForeignKey("ClipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Clip");
                });

            modelBuilder.Entity("PluralVideos.Data.Models.Clip", b =>
                {
                    b.Navigation("ClipTranscript");
                });

            modelBuilder.Entity("PluralVideos.Data.Models.Course", b =>
                {
                    b.Navigation("Module");
                });

            modelBuilder.Entity("PluralVideos.Data.Models.Module", b =>
                {
                    b.Navigation("Clip");
                });
#pragma warning restore 612, 618
        }
    }
}