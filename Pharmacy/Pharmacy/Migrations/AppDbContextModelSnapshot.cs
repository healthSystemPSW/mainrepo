﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pharmacy.EfStructures;

namespace Pharmacy.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("MedicineMedicinePotentialDanger", b =>
                {
                    b.Property<int>("MedicinePotentialDangersId")
                        .HasColumnType("integer");

                    b.Property<int>("MedicinesId")
                        .HasColumnType("integer");

                    b.HasKey("MedicinePotentialDangersId", "MedicinesId");

                    b.HasIndex("MedicinesId");

                    b.ToTable("MedicineMedicinePotentialDanger");
                });

            modelBuilder.Entity("MedicinePrecaution", b =>
                {
                    b.Property<int>("MedicinesId")
                        .HasColumnType("integer");

                    b.Property<int>("PrecautionsId")
                        .HasColumnType("integer");

                    b.HasKey("MedicinesId", "PrecautionsId");

                    b.HasIndex("PrecautionsId");

                    b.ToTable("MedicinePrecaution");
                });

            modelBuilder.Entity("MedicineReaction", b =>
                {
                    b.Property<int>("MedicinesId")
                        .HasColumnType("integer");

                    b.Property<int>("ReactionsId")
                        .HasColumnType("integer");

                    b.HasKey("MedicinesId", "ReactionsId");

                    b.HasIndex("ReactionsId");

                    b.ToTable("MedicineReaction");
                });

            modelBuilder.Entity("MedicineSideEffect", b =>
                {
                    b.Property<int>("MedicinesId")
                        .HasColumnType("integer");

                    b.Property<int>("SideEffectsId")
                        .HasColumnType("integer");

                    b.HasKey("MedicinesId", "SideEffectsId");

                    b.HasIndex("SideEffectsId");

                    b.ToTable("MedicineSideEffect");
                });

            modelBuilder.Entity("MedicineSubstance", b =>
                {
                    b.Property<int>("MedicinesId")
                        .HasColumnType("integer");

                    b.Property<int>("SubstancesId")
                        .HasColumnType("integer");

                    b.HasKey("MedicinesId", "SubstancesId");

                    b.HasIndex("SubstancesId");

                    b.ToTable("MedicineSubstance");
                });

            modelBuilder.Entity("Pharmacy.Model.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("CountryId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("PostalCode")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("Pharmacy.Model.Complaint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("HospitalId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("HospitalId");

                    b.ToTable("Complaints");
                });

            modelBuilder.Entity("Pharmacy.Model.ComplaintResponse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ComplaintId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ComplaintId");

                    b.ToTable("ComplaintResponses");
                });

            modelBuilder.Entity("Pharmacy.Model.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Pharmacy.Model.Hospital", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<Guid>("ApiKey")
                        .HasColumnType("uuid");

                    b.Property<string>("BaseUrl")
                        .HasColumnType("text");

                    b.Property<int>("CityId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("StreetName")
                        .HasColumnType("text");

                    b.Property<string>("StreetNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Hospitals");
                });

            modelBuilder.Entity("Pharmacy.Model.Manufacturer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Manufacturers");
                });

            modelBuilder.Entity("Pharmacy.Model.Medicine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ManufacturerId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");


                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.Property<string>("Usage")
                        .HasColumnType("text");

                    b.Property<double>("WeightInMilligrams")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("ManufacturerId");

                    b.ToTable("Medicines");
                });

            modelBuilder.Entity("Pharmacy.Model.MedicineCombination", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("FirstMedicineId")
                        .HasColumnType("integer");

                    b.Property<int>("SecondMedicineId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FirstMedicineId");

                    b.HasIndex("SecondMedicineId");

                    b.ToTable("MedicineCombinations");
                });

            modelBuilder.Entity("Pharmacy.Model.MedicinePotentialDanger", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("MedicinePotentialDangers");
                });

            modelBuilder.Entity("Pharmacy.Model.Precaution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Precautions");
                });

            modelBuilder.Entity("Pharmacy.Model.Reaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Reactions");
                });

            modelBuilder.Entity("Pharmacy.Model.SideEffect", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SideEffects");
                });

            modelBuilder.Entity("Pharmacy.Model.Substance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Substances");
                });

            modelBuilder.Entity("MedicineMedicinePotentialDanger", b =>
                {
                    b.HasOne("Pharmacy.Model.MedicinePotentialDanger", null)
                        .WithMany()
                        .HasForeignKey("MedicinePotentialDangersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pharmacy.Model.Medicine", null)
                        .WithMany()
                        .HasForeignKey("MedicinesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicinePrecaution", b =>
                {
                    b.HasOne("Pharmacy.Model.Medicine", null)
                        .WithMany()
                        .HasForeignKey("MedicinesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pharmacy.Model.Precaution", null)
                        .WithMany()
                        .HasForeignKey("PrecautionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicineReaction", b =>
                {
                    b.HasOne("Pharmacy.Model.Medicine", null)
                        .WithMany()
                        .HasForeignKey("MedicinesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pharmacy.Model.Reaction", null)
                        .WithMany()
                        .HasForeignKey("ReactionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicineSideEffect", b =>
                {
                    b.HasOne("Pharmacy.Model.Medicine", null)
                        .WithMany()
                        .HasForeignKey("MedicinesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pharmacy.Model.SideEffect", null)
                        .WithMany()
                        .HasForeignKey("SideEffectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicineSubstance", b =>
                {
                    b.HasOne("Pharmacy.Model.Medicine", null)
                        .WithMany()
                        .HasForeignKey("MedicinesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pharmacy.Model.Substance", null)
                        .WithMany()
                        .HasForeignKey("SubstancesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Pharmacy.Model.City", b =>
                {
                    b.HasOne("Pharmacy.Model.Country", "Country")
                        .WithMany("Cities")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Pharmacy.Model.Complaint", b =>
                {
                    b.HasOne("Pharmacy.Model.Hospital", "Hospital")
                        .WithMany("Complaints")
                        .HasForeignKey("HospitalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hospital");
                });

            modelBuilder.Entity("Pharmacy.Model.ComplaintResponse", b =>
                {
                    b.HasOne("Pharmacy.Model.Complaint", "Complaint")
                        .WithMany()
                        .HasForeignKey("ComplaintId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Complaint");
                });

            modelBuilder.Entity("Pharmacy.Model.Hospital", b =>
                {
                    b.HasOne("Pharmacy.Model.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("Pharmacy.Model.Medicine", b =>
                {
                    b.HasOne("Pharmacy.Model.Manufacturer", "Manufacturer")
                        .WithMany()
                        .HasForeignKey("ManufacturerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Manufacturer");
                });

            modelBuilder.Entity("Pharmacy.Model.MedicineCombination", b =>
                {
                    b.HasOne("Pharmacy.Model.Medicine", "FirstMedicine")
                        .WithMany()
                        .HasForeignKey("FirstMedicineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pharmacy.Model.Medicine", "SecondMedicine")
                        .WithMany()
                        .HasForeignKey("SecondMedicineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FirstMedicine");

                    b.Navigation("SecondMedicine");
                });

            modelBuilder.Entity("Pharmacy.Model.Country", b =>
                {
                    b.Navigation("Cities");
                });

            modelBuilder.Entity("Pharmacy.Model.Hospital", b =>
                {
                    b.Navigation("Complaints");
                });
#pragma warning restore 612, 618
        }
    }
}
