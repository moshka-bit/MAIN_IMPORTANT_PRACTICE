using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MAIN_IMPORTANT_PRACTICE.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Дисциплина> Дисциплинаs { get; set; }

    public virtual DbSet<ЗавКафедрой> ЗавКафедройs { get; set; }

    public virtual DbSet<Инженер> Инженерs { get; set; }

    public virtual DbSet<Кафедра> Кафедраs { get; set; }

    public virtual DbSet<Преподаватель> Преподавательs { get; set; }

    public virtual DbSet<Роли> Ролиs { get; set; }

    public virtual DbSet<Сотрудник> Сотрудникs { get; set; }

    public virtual DbSet<Специальность> Специальностьs { get; set; }

    public virtual DbSet<Студент> Студентs { get; set; }

    public virtual DbSet<Факультет> Факультетs { get; set; }

    public virtual DbSet<Экзамен> Экзаменs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Учебная;Username=postgres;Password=dasha98712345");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Дисциплина>(entity =>
        {
            entity.HasKey(e => e.Код).HasName("Дисциплина_pkey");

            entity.ToTable("Дисциплина");

            entity.Property(e => e.Код).ValueGeneratedNever();
            entity.Property(e => e.Исполнитель).HasMaxLength(10);
            entity.Property(e => e.Название).HasMaxLength(100);

            entity.HasOne(d => d.ИсполнительNavigation).WithMany(p => p.Дисциплинаs)
                .HasForeignKey(d => d.Исполнитель)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Дисциплина_Исполнитель_fkey");

            entity.HasMany(d => d.Номерs).WithMany(p => p.Кодs)
                .UsingEntity<Dictionary<string, object>>(
                    "Заявка",
                    r => r.HasOne<Специальность>().WithMany()
                        .HasForeignKey("Номер")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Заявка_Номер_fkey"),
                    l => l.HasOne<Дисциплина>().WithMany()
                        .HasForeignKey("Код")
                        .HasConstraintName("Заявка_Код_fkey"),
                    j =>
                    {
                        j.HasKey("Код", "Номер").HasName("Заявка_pkey");
                        j.ToTable("Заявка");
                        j.IndexerProperty<string>("Номер").HasMaxLength(20);
                    });
        });

        modelBuilder.Entity<ЗавКафедрой>(entity =>
        {
            entity.HasKey(e => e.ТабНомер).HasName("Зав_кафедрой_pkey");

            entity.ToTable("Зав_кафедрой");

            entity.Property(e => e.ТабНомер)
                .ValueGeneratedNever()
                .HasColumnName("Таб_номер");

            entity.HasOne(d => d.ТабНомерNavigation).WithOne(p => p.ЗавКафедрой)
                .HasForeignKey<ЗавКафедрой>(d => d.ТабНомер)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Зав_кафедрой_Таб_номер_fkey");
        });

        modelBuilder.Entity<Инженер>(entity =>
        {
            entity.HasKey(e => e.ТабНомер).HasName("Инженер_pkey");

            entity.ToTable("Инженер");

            entity.Property(e => e.ТабНомер)
                .ValueGeneratedNever()
                .HasColumnName("Таб_номер");
            entity.Property(e => e.Специальность).HasMaxLength(50);

            entity.HasOne(d => d.ТабНомерNavigation).WithOne(p => p.Инженер)
                .HasForeignKey<Инженер>(d => d.ТабНомер)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Инженер_Таб_номер_fkey");
        });

        modelBuilder.Entity<Кафедра>(entity =>
        {
            entity.HasKey(e => e.Шифр).HasName("Кафедра_pkey");

            entity.ToTable("Кафедра");

            entity.Property(e => e.Шифр).HasMaxLength(10);
            entity.Property(e => e.Название).HasMaxLength(100);
            entity.Property(e => e.Факультет).HasMaxLength(10);

            entity.HasOne(d => d.ФакультетNavigation).WithMany(p => p.Кафедраs)
                .HasForeignKey(d => d.Факультет)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Кафедра_Факультет_fkey");
        });

        modelBuilder.Entity<Преподаватель>(entity =>
        {
            entity.HasKey(e => e.ТабНомер).HasName("Преподаватель_pkey");

            entity.ToTable("Преподаватель");

            entity.Property(e => e.ТабНомер)
                .ValueGeneratedNever()
                .HasColumnName("Таб_номер");
            entity.Property(e => e.Звание).HasMaxLength(50);
            entity.Property(e => e.Степень).HasMaxLength(50);

            entity.HasOne(d => d.ТабНомерNavigation).WithOne(p => p.Преподаватель)
                .HasForeignKey<Преподаватель>(d => d.ТабНомер)
                .HasConstraintName("Преподаватель_Таб_номер_fkey");
        });

        modelBuilder.Entity<Роли>(entity =>
        {
            entity.HasKey(e => e.КодРоли).HasName("Роли_pkey");

            entity.ToTable("Роли");

            entity.Property(e => e.КодРоли).HasColumnName("код_роли");
            entity.Property(e => e.НазваниеРоли).HasColumnName("название_роли");
        });

        modelBuilder.Entity<Сотрудник>(entity =>
        {
            entity.HasKey(e => e.ТабНомер).HasName("Сотрудник_pkey");

            entity.ToTable("Сотрудник");

            entity.Property(e => e.ТабНомер)
                .ValueGeneratedNever()
                .HasColumnName("Таб_номер");
            entity.Property(e => e.Зарплата).HasPrecision(10, 2);
            entity.Property(e => e.КодРоли).HasColumnName("Код_роли");
            entity.Property(e => e.Логин).HasMaxLength(50);
            entity.Property(e => e.Номер).HasMaxLength(20);
            entity.Property(e => e.Пароль).HasMaxLength(50);
            entity.Property(e => e.Фамилия).HasMaxLength(100);
            entity.Property(e => e.Шифр).HasMaxLength(10);

            entity.HasOne(d => d.КодРолиNavigation).WithMany(p => p.Сотрудникs)
                .HasForeignKey(d => d.КодРоли)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Сотрудник_Роль_fkey");

            entity.HasOne(d => d.ТабНомерNavigation).WithOne(p => p.Сотрудник)
                .HasForeignKey<Сотрудник>(d => d.ТабНомер)
                .HasConstraintName("Сотрудник_РегНомер");

            entity.HasOne(d => d.ШефNavigation).WithMany(p => p.InverseШефNavigation)
                .HasForeignKey(d => d.Шеф)
                .HasConstraintName("Сотрудник_Шеф_fkey");

            entity.HasOne(d => d.ШифрNavigation).WithMany(p => p.Сотрудникs)
                .HasForeignKey(d => d.Шифр)
                .HasConstraintName("Сотрудник_Шифр_fkey");
        });

        modelBuilder.Entity<Специальность>(entity =>
        {
            entity.HasKey(e => e.Номер).HasName("Специальность_pkey");

            entity.ToTable("Специальность");

            entity.Property(e => e.Номер).HasMaxLength(20);
            entity.Property(e => e.Направление).HasMaxLength(200);
            entity.Property(e => e.Шифр).HasMaxLength(10);

            entity.HasOne(d => d.ШифрNavigation).WithMany(p => p.Специальностьs)
                .HasForeignKey(d => d.Шифр)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Специальность_Шифр_fkey");
        });

        modelBuilder.Entity<Студент>(entity =>
        {
            entity.HasKey(e => e.РегНомер).HasName("Студент_pkey");

            entity.ToTable("Студент");

            entity.Property(e => e.РегНомер)
                .ValueGeneratedNever()
                .HasColumnName("Рег_номер");
            entity.Property(e => e.Номер).HasMaxLength(20);

            entity.HasOne(d => d.НомерNavigation).WithMany(p => p.Студентs)
                .HasForeignKey(d => d.Номер)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Студент_Номер_fkey");
        });

        modelBuilder.Entity<Факультет>(entity =>
        {
            entity.HasKey(e => e.Аббревиатура).HasName("Факультет_pkey");

            entity.ToTable("Факультет");

            entity.Property(e => e.Аббревиатура).HasMaxLength(10);
            entity.Property(e => e.Название).HasMaxLength(100);
        });

        modelBuilder.Entity<Экзамен>(entity =>
        {
            entity.HasKey(e => e.КодЭкзамена).HasName("Экзамен_pkey");

            entity.ToTable("Экзамен");

            entity.Property(e => e.КодЭкзамена).HasColumnName("Код_Экзамена");
            entity.Property(e => e.Аудитория).HasMaxLength(20);
            entity.Property(e => e.РегНомер).HasColumnName("Рег_номер");
            entity.Property(e => e.ТабНомер).HasColumnName("Таб_номер");

            entity.HasOne(d => d.КодNavigation).WithMany(p => p.Экзаменs)
                .HasForeignKey(d => d.Код)
                .HasConstraintName("Экзамен_Код_fkey");

            entity.HasOne(d => d.РегНомерNavigation).WithMany(p => p.ЭкзаменРегНомерNavigations)
                .HasForeignKey(d => d.РегНомер)
                .HasConstraintName("Экзамен_РегНомер_fkey");

            entity.HasOne(d => d.ТабНомерNavigation).WithMany(p => p.ЭкзаменТабНомерNavigations)
                .HasForeignKey(d => d.ТабНомер)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("Экзамен_Таб_номер_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
