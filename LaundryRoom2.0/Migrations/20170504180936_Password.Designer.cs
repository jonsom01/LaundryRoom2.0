using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using LaundryRoom20.Models;

namespace LaundryRoom20.Migrations
{
    [DbContext(typeof(LaundryRoomContext))]
    [Migration("20170504180936_Password")]
    partial class Password
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LaundryRoom20.Models.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BookerId");

                    b.Property<string>("Time");

                    b.HasKey("Id");

                    b.HasIndex("BookerId")
                        .IsUnique();

                    b.ToTable("Booking");
                });

            modelBuilder.Entity("LaundryRoom20.Models.User", b =>
                {
                    b.Property<string>("BookerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .HasMaxLength(50);

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("Salt");

                    b.Property<string>("ShortAddress")
                        .HasMaxLength(10);

                    b.HasKey("BookerId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("LaundryRoom20.Models.Booking", b =>
                {
                    b.HasOne("LaundryRoom20.Models.User", "User")
                        .WithOne("Booking")
                        .HasForeignKey("LaundryRoom20.Models.Booking", "BookerId");
                });
        }
    }
}
