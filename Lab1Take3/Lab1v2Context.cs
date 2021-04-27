using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Lab1Take3
{
    public partial class Lab1v2Context : DbContext
    {
        public Lab1v2Context()
        {
        }

        public Lab1v2Context(DbContextOptions<Lab1v2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Armor> Armors { get; set; }
        public virtual DbSet<Battle> Battles { get; set; }
        public virtual DbSet<BattleType> BattleTypes { get; set; }
        public virtual DbSet<Character> Characters { get; set; }
        public virtual DbSet<Race> Races { get; set; }
        public virtual DbSet<Side> Sides { get; set; }
        public virtual DbSet<Weapon> Weapons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-U0OE9SI7; Database=Lab1v2; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Armor>(entity =>
            {
                entity.ToTable("Armor");

                entity.Property(e => e.ArmorName).HasMaxLength(50);
            });

            modelBuilder.Entity<Battle>(entity =>
            {
                entity.ToTable("Battle");

                entity.Property(e => e.BattleName).HasMaxLength(50);
                entity.HasOne(d => d.BattleType)
                    .WithMany(p => p.Battles)
                    .HasForeignKey(d => d.BattleTypeId)
                    .HasConstraintName("FK_Battle_BattleType");
            });

            modelBuilder.Entity<BattleType>(entity =>
            {
                entity.ToTable("BattleType");

                entity.Property(e => e.BattleTypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<Character>(entity =>
            {
                entity.ToTable("Character");

                entity.Property(e => e.CharacterName).HasMaxLength(50);

                entity.HasOne(d => d.Armor)
                    .WithMany(p => p.Characters)
                    .HasForeignKey(d => d.ArmorId)
                    .HasConstraintName("FK_Character_Armor");

                entity.HasOne(d => d.Battle)
                    .WithMany(p => p.Characters)
                    .HasForeignKey(d => d.BattleId)
                    .HasConstraintName("FK_Character_Battle");

                entity.HasOne(d => d.Race)
                    .WithMany(p => p.Characters)
                    .HasForeignKey(d => d.RaceId)
                    .HasConstraintName("FK_Character_Race");

                entity.HasOne(d => d.Weapon)
                    .WithMany(p => p.Characters)
                    .HasForeignKey(d => d.WeaponId)
                    .HasConstraintName("FK_Character_Character");

                entity.HasOne(d => d.Side)
                    .WithMany(p => p.Characters)
                    .HasForeignKey(d => d.SideId)
                    .HasConstraintName("FK_Character_Side");
            });

            modelBuilder.Entity<Race>(entity =>
            {
                entity.ToTable("Race");

                entity.Property(e => e.RaceName).HasMaxLength(50);
            });

            modelBuilder.Entity<Side>(entity =>
            {
               // entity.HasNoKey();

                entity.ToTable("Side");

               // entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.SideName).HasMaxLength(50);
            });

            modelBuilder.Entity<Weapon>(entity =>
            {
                entity.ToTable("Weapon");

                entity.Property(e => e.WeaponName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
