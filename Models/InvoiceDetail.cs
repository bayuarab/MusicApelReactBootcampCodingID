namespace SecondV.Models
{
  public class InvoiceDetail
    {
        public int Id { get; set; }

        public string? Course { get; set; }

        public string? CourseCategory { get; set; }

        public string? Schedule { get; set; }

        public int Price { get; set; }

        public string? NoInvoice { get; set; }

    }
}