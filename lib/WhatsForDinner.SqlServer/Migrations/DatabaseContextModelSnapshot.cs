﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WhatsForDinner.SqlServer;

#nullable disable

namespace WhatsForDinner.SqlServer.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WhatsForDinner.SqlServer.Entities.Effort", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.HasKey("Id");

                    b.ToTable("Effort");

                    b.HasData(
                        new
                        {
                            Id = "LOW",
                            Name = "Niski"
                        },
                        new
                        {
                            Id = "MEDIUM",
                            Name = "Średni"
                        },
                        new
                        {
                            Id = "HIGH",
                            Name = "Wysoki"
                        });
                });

            modelBuilder.Entity("WhatsForDinner.SqlServer.Entities.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.ToTable("Group");
                });

            modelBuilder.Entity("WhatsForDinner.SqlServer.Entities.Ingredient", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Ingredient");

                    b.HasData(
                        new
                        {
                            Id = "MILK",
                            Name = "Mleko"
                        },
                        new
                        {
                            Id = "FLOUR",
                            Name = "Mąka"
                        });
                });

            modelBuilder.Entity("WhatsForDinner.SqlServer.Entities.Meal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EffortId")
                        .IsRequired()
                        .HasColumnType("nvarchar(6)");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Links")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("ModifiedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecipeSteps")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.HasIndex("CreatedById");

                    b.HasIndex("EffortId");

                    b.HasIndex("GroupId");

                    b.HasIndex("ModifiedById");

                    b.ToTable("Meal");
                });

            modelBuilder.Entity("WhatsForDinner.SqlServer.Entities.Membership", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "GroupId");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("UserId", "GroupId"), false);

                    b.HasIndex("GroupId");

                    b.ToTable("Membership");
                });

            modelBuilder.Entity("WhatsForDinner.SqlServer.Entities.Unit", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.HasKey("Id");

                    b.ToTable("Unit");

                    b.HasData(
                        new
                        {
                            Id = "GRAM",
                            Name = "g"
                        },
                        new
                        {
                            Id = "MILILITER",
                            Name = "ml"
                        },
                        new
                        {
                            Id = "TABLE_SPOON",
                            Name = "łyżka stołowa"
                        },
                        new
                        {
                            Id = "SPOON",
                            Name = "łyżeczka"
                        });
                });

            modelBuilder.Entity("WhatsForDinner.SqlServer.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(201)
                        .HasColumnType("nvarchar(201)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"), false);

                    b.ToTable("User");
                });

            modelBuilder.Entity("WhatsForDinner.SqlServer.Entities.Meal", b =>
                {
                    b.HasOne("WhatsForDinner.SqlServer.Entities.User", "CreatedBy")
                        .WithMany("CreatedMeals")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WhatsForDinner.SqlServer.Entities.Effort", "Effort")
                        .WithMany("Meals")
                        .HasForeignKey("EffortId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WhatsForDinner.SqlServer.Entities.Group", "Group")
                        .WithMany("Meals")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WhatsForDinner.SqlServer.Entities.User", "ModifiedBy")
                        .WithMany("ModifiedMeals")
                        .HasForeignKey("ModifiedById");

                    b.OwnsMany("WhatsForDinner.SqlServer.Entities.MealIngredient", "Ingredients", b1 =>
                        {
                            b1.Property<Guid>("MealId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            b1.Property<int>("Amount")
                                .HasColumnType("int");

                            b1.Property<string>("IngredientId")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("IngredientName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("UnitId")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("UnitName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("MealId", "Id");

                            b1.ToTable("Meal");

                            b1.ToJson("Ingredients");

                            b1.WithOwner()
                                .HasForeignKey("MealId");
                        });

                    b.Navigation("CreatedBy");

                    b.Navigation("Effort");

                    b.Navigation("Group");

                    b.Navigation("Ingredients");

                    b.Navigation("ModifiedBy");
                });

            modelBuilder.Entity("WhatsForDinner.SqlServer.Entities.Membership", b =>
                {
                    b.HasOne("WhatsForDinner.SqlServer.Entities.Group", "Group")
                        .WithMany("Memberships")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WhatsForDinner.SqlServer.Entities.User", "User")
                        .WithMany("Memberships")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WhatsForDinner.SqlServer.Entities.Effort", b =>
                {
                    b.Navigation("Meals");
                });

            modelBuilder.Entity("WhatsForDinner.SqlServer.Entities.Group", b =>
                {
                    b.Navigation("Meals");

                    b.Navigation("Memberships");
                });

            modelBuilder.Entity("WhatsForDinner.SqlServer.Entities.User", b =>
                {
                    b.Navigation("CreatedMeals");

                    b.Navigation("Memberships");

                    b.Navigation("ModifiedMeals");
                });
#pragma warning restore 612, 618
        }
    }
}
