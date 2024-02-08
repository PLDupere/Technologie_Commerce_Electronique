﻿// <auto-generated />
using System;
using Boutique_en_ligne;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Boutique_en_ligne.Migrations
{
    [DbContext(typeof(BoutiqueJeuDbContext))]
    [Migration("20240208145705_bd")]
    partial class bd
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Boutique_en_ligne.Models.CarteCredit", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<DateTime?>("date")
                        .HasColumnType("datetime2");

                    b.Property<string>("detenteur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("numero")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("numero_secret")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CarteCredits");
                });

            modelBuilder.Entity("Boutique_en_ligne.Models.Facture", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<int?>("ClientId")
                        .HasColumnType("int");

                    b.Property<float?>("montant_depense")
                        .HasColumnType("real");

                    b.Property<int?>("nombre_article")
                        .HasColumnType("int");

                    b.Property<int?>("nombre_facture")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Factures");
                });

            modelBuilder.Entity("Boutique_en_ligne.Models.JeuVideo", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<string>("annee_sortie")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("capture_ecran")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("console")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("editeur")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("genre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pochette_jeu")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("prix_vente")
                        .HasColumnType("real");

                    b.Property<string>("titre")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("JeuVideos");
                });

            modelBuilder.Entity("Boutique_en_ligne.Models.Utilisateur", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("adresse_courriel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("date_naissance")
                        .HasColumnType("datetime2");

                    b.Property<string>("mot_de_passe")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("prenom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("profil")
                        .HasColumnType("int");

                    b.Property<string>("ville")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Utilisateurs");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Utilisateur");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("JeuVideoUtilisateur", b =>
                {
                    b.Property<int>("JeuxVideosId")
                        .HasColumnType("int");

                    b.Property<int>("UtilisateursId")
                        .HasColumnType("int");

                    b.HasKey("JeuxVideosId", "UtilisateursId");

                    b.HasIndex("UtilisateursId");

                    b.ToTable("JeuVideoUtilisateur");
                });

            modelBuilder.Entity("Boutique_en_ligne.Models.Client", b =>
                {
                    b.HasBaseType("Boutique_en_ligne.Models.Utilisateur");

                    b.Property<int?>("CarteId")
                        .HasColumnType("int");

                    b.Property<int?>("carteCreditId")
                        .HasColumnType("int");

                    b.Property<float?>("solde")
                        .HasColumnType("real");

                    b.HasIndex("carteCreditId");

                    b.HasDiscriminator().HasValue("Client");
                });

            modelBuilder.Entity("Boutique_en_ligne.Models.Vendeur", b =>
                {
                    b.HasBaseType("Boutique_en_ligne.Models.Utilisateur");

                    b.HasDiscriminator().HasValue("Vendeur");
                });

            modelBuilder.Entity("Boutique_en_ligne.Models.Facture", b =>
                {
                    b.HasOne("Boutique_en_ligne.Models.Client", "Client")
                        .WithMany("Factures")
                        .HasForeignKey("ClientId");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("JeuVideoUtilisateur", b =>
                {
                    b.HasOne("Boutique_en_ligne.Models.JeuVideo", null)
                        .WithMany()
                        .HasForeignKey("JeuxVideosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Boutique_en_ligne.Models.Utilisateur", null)
                        .WithMany()
                        .HasForeignKey("UtilisateursId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Boutique_en_ligne.Models.Client", b =>
                {
                    b.HasOne("Boutique_en_ligne.Models.CarteCredit", "carteCredit")
                        .WithMany()
                        .HasForeignKey("carteCreditId");

                    b.Navigation("carteCredit");
                });

            modelBuilder.Entity("Boutique_en_ligne.Models.Client", b =>
                {
                    b.Navigation("Factures");
                });
#pragma warning restore 612, 618
        }
    }
}
