namespace SecondV.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        public DbSet<MasterInvoice> MasterInvoices { get; set; }

        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
    }
}