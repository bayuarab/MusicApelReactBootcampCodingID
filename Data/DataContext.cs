using System.Security.Cryptography;
namespace SecondV.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            using (var hmac = new HMACSHA512()){
            builder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    nama = "admin",
                    email = "admin",
                    roles = "admin",
                    passwordSalt = hmac.Key,
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("admin"))
                }
            );
            }
        }
        
        public DbSet<MasterInvoice> MasterInvoices { get; set; }

        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<CourseCategory> CourseCategories { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<User> Users { get; set; }
        
        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<UserCourse> UserCourses { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }
    }
}