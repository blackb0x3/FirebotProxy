﻿// <auto-generated />
using System;
using FirebotProxy.Data.Access;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FirebotProxy.Data.Access.Migrations
{
    [DbContext(typeof(FirebotProxyContext))]
    [Migration("20220812111417_Add_ChatPlot_Entity")]
    partial class Add_ChatPlot_Entity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.7");

            modelBuilder.Entity("FirebotProxy.Data.Entities.ChatMessage", b =>
                {
                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<string>("SenderUsername")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.HasIndex("SenderUsername");

                    b.HasIndex("Timestamp");

                    b.ToTable("ChatMessages", (string)null);
                });

            modelBuilder.Entity("FirebotProxy.Data.Entities.ChatPlot", b =>
                {
                    b.Property<string>("ViewerUsername")
                        .HasColumnType("TEXT");

                    b.Property<string>("ChartUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.HasKey("ViewerUsername");

                    b.ToTable("ChatPlots", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
