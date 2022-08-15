namespace SecondV.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        public DbSet<MasterInvoice> MasterInvoices { get; set; }

        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<CourseCategory> CourseCategories { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<User> Users { get; set; }
        
        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<UserCourse> UserCourses { get; set; }
    }
}