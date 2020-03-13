using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TableTennis
{
    public partial class TableTennisDBContext : DbContext
    {
        public TableTennisDBContext()
        {
        }

        public TableTennisDBContext(DbContextOptions<TableTennisDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Blade> Blade { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Factory> Factory { get; set; }
        public virtual DbSet<Game> Game { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<PlayerRackets> PlayerRackets { get; set; }
        public virtual DbSet<Racket> Racket { get; set; }
        public virtual DbSet<Rubber> Rubber { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-CAVVMKM\\SQLEXPRESS; Database=TableTennisDB; Trusted_Connection=True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blade>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BladeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FactoryId).HasColumnName("FactoryID");

                entity.HasOne(d => d.Factory)
                    .WithMany(p => p.Blade)
                    .HasForeignKey(d => d.FactoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Blade_Factory");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Factory>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.FactoryName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Factory)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Factory_Country");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.GameDate).HasColumnType("date");

                entity.Property(e => e.Player1Id).HasColumnName("Player1ID");

                entity.Property(e => e.Player2Id).HasColumnName("Player2ID");

                entity.Property(e => e.Racket1Id).HasColumnName("Racket1ID");

                entity.Property(e => e.Racket2Id).HasColumnName("Racket2ID");

                entity.Property(e => e.Score)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Player1)
                    .WithMany(p => p.GamePlayer1)
                    .HasForeignKey(d => d.Player1Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Game_Player1");

                entity.HasOne(d => d.Player2)
                    .WithMany(p => p.GamePlayer2)
                    .HasForeignKey(d => d.Player2Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Game_Player2");

                entity.HasOne(d => d.Racket1)
                    .WithMany(p => p.GameRacket1)
                    .HasForeignKey(d => d.Racket1Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Game_PlayerRackets1");

                entity.HasOne(d => d.Racket2)
                    .WithMany(p => p.GameRacket2)
                    .HasForeignKey(d => d.Racket2Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Game_PlayerRackets2");
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Player)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Player_Country");
            });

            modelBuilder.Entity<PlayerRackets>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.PlayerId).HasColumnName("PlayerID");

                entity.Property(e => e.RacketId).HasColumnName("RacketID");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.PlayerRackets)
                    .HasForeignKey<PlayerRackets>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlayerRackets_Racket");

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.PlayerRackets)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlayerRackets_Player");
            });

            modelBuilder.Entity<Racket>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BhrubberId).HasColumnName("BHRubberID");

                entity.Property(e => e.BladeId).HasColumnName("BladeID");

                entity.Property(e => e.FhrubberId).HasColumnName("FHRubberID");

                entity.HasOne(d => d.Bhrubber)
                    .WithMany(p => p.RacketBhrubber)
                    .HasForeignKey(d => d.BhrubberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Racket_BHRubber");

                entity.HasOne(d => d.Blade)
                    .WithMany(p => p.Racket)
                    .HasForeignKey(d => d.BladeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Racket_Blade");

                entity.HasOne(d => d.Fhrubber)
                    .WithMany(p => p.RacketFhrubber)
                    .HasForeignKey(d => d.FhrubberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Racket_FHRubber");
            });

            modelBuilder.Entity<Rubber>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FactoryId).HasColumnName("FactoryID");

                entity.Property(e => e.Pimples)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RubberName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Factory)
                    .WithMany(p => p.Rubber)
                    .HasForeignKey(d => d.FactoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rubber_Factory");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
