using System;
using System.Collections.Generic;
using LibraryDomain.Model;
using Microsoft.EntityFrameworkCore;

namespace LibraryInfrastracture;

public partial class LibraryContext : DbContext
{
    public LibraryContext()
    {
    }

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookAuthor> BookAuthors { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Shelf> Shelves { get; set; }

    public virtual DbSet<ShelfBook> ShelfBooks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserLibrary> UserLibraries { get; set; }
    public virtual DbSet<Genre> Genre { get; set; } = default!;
    public virtual DbSet<BookGenre> BookGenres { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-HOLC7A0\\SQLEXPRESS; Database=Shop; Trusted_Connection=True; TrustServerCertificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Author__3213E83FDC72A0CE");

            entity.ToTable("Author");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Book__3213E83F5E32B59B");

            entity.ToTable("Book");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Isbn)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("ISBN");
            entity.Property(e => e.PublisherId).HasColumnName("PublisherID");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Publisher).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublisherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_book_publisher");
        });

        modelBuilder.Entity<BookAuthor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookAuth__3213E83F4D0F2CC4");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.BookId).HasColumnName("BookID");

            entity.HasOne(d => d.Author).WithMany(p => p.BookAuthors)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bookauthors_author");

            entity.HasOne(d => d.Book).WithMany(p => p.BookAuthors)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bookauthors_book");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Publishe__3213E83F46262A8B");

            entity.ToTable("Publisher");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PublisherName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Review__3213E83F92DE17F1");

            entity.ToTable("Review");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.Text)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Book).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_review_book");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_review_user");
        });

        modelBuilder.Entity<Shelf>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shelf__3213E83FCB52B29F");

            entity.ToTable("Shelf");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Shelves)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shelf_user");
        });

        modelBuilder.Entity<ShelfBook>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ShelfBoo__3213E83F682A7F29");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.ShelfId).HasColumnName("ShelfID");

            entity.HasOne(d => d.Book).WithMany(p => p.ShelfBooks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shelfbooks_book");

            entity.HasOne(d => d.Shelf).WithMany(p => p.ShelfBooks)
                .HasForeignKey(d => d.ShelfId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shelfbooks_shelf");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3213E83F232B3F4E");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserLibrary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserLibr__3213E83F4AD29D7A");

            entity.ToTable("UserLibrary");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Book).WithMany(p => p.UserLibraries)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_userlibrary_book");

            entity.HasOne(d => d.User).WithMany(p => p.UserLibraries)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_userlibrary_user");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Genre");

            entity.ToTable("Genre");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.GenreName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });
        modelBuilder.Entity<BookGenre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_BookGenres");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.genreid).HasColumnName("genreid");
            entity.Property(e => e.bookid).HasColumnName("bookid");

            entity.HasOne(d => d.Genre).WithMany(p => p.BookGenres)
                .HasForeignKey(d => d.genreid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookGenres_Genre");

            entity.HasOne(d => d.Book).WithMany(p => p.BookGenres)
                .HasForeignKey(d => d.bookid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookGenres_Book");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


}
