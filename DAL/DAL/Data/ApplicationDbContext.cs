using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    //inherit from DbContext from EntityFrameworkCore
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext()
        {


        }
        //constructor created with initialized parameter DbContext
        //then, DbContext made into variable options passed into base class
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(LocalDb)\\mssqllocaldb;Database=OnlineExaminationDb;Trusted_Connection=true;MultipleActiveResultSets=true");
            }
        }

        public DbSet<ExamResults> ExamResults { get; set; }
        public DbSet<Exams> Exams { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<QnAs> QnAs { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<Users> Users { get; set; }

        //creating modelbuilder
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //setting validations
            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<Students>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Contact).HasMaxLength(50);
                entity.Property(e => e.CVFileName).HasMaxLength(250);
                entity.Property(e => e.PictureFileName).HasMaxLength(250);
                //students have multiple classes or groups
                entity.HasOne(d => d.Groups).WithMany(p => p.Students).HasForeignKey(d => d.GroupsId);
            });

            modelBuilder.Entity<QnAs>(entity =>
            {
                entity.Property(e => e.Question).IsRequired();
                entity.Property(e => e.Option1).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Option2).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Option3).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Option4).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Answer).IsRequired();
                //questions belongs to multiple exams
                //the deletebehaviour handles the deletion of whole row based on the foreing key
                entity.HasOne(d => d.Exams).WithMany(p => p.QnAs).HasForeignKey(d => d.ExamsId).OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Groups>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(250);                
                entity.HasOne(d => d.Users).WithMany(p => p.Groups).HasForeignKey(d => d.UsersId).OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Exams>(entity =>
            {
                entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(250);
                entity.HasOne(d => d.Groups).WithMany(p => p.Exams).HasForeignKey(d => d.GroupsId).OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ExamResults>(entity =>
            {
                entity.HasOne(d => d.Exams).WithMany(p => p.ExamResults).HasForeignKey(d => d.ExamsId);
                entity.HasOne(d => d.QnAs).WithMany(p => p.ExamResults).HasForeignKey(d => d.QnAsId).OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(d => d.Students).WithMany(p => p.ExamResults).HasForeignKey(d => d.StudentsId).OnDelete(DeleteBehavior.ClientSetNull);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
