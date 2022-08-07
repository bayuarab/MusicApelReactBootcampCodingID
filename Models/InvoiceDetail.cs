namespace SecondV.Models
{
  public class InvoiceDetail
    {
        public int Id { get; set; }
        public string? NoInvoice { get; set; }
        public int CourseId { get; set; }
        // public Course? Course { get; set; }
        public int MasterInvoiceId { get; set; }
        // public MasterInvoice? MasterInvoice { get; set; }
    }
}