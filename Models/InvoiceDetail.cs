namespace SecondV.Models
{
  public class InvoiceDetail
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Not valid data")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Not valid data")]
        // [RegularExpression(".*[a-zA-Z]+.*", ErrorMessage = "Not valid data")]
        public string? NoInvoice { get; set; }

        [Required(ErrorMessage = "Not valid data")]
        public string? Course { get; set; }

        [Required(ErrorMessage = "Not valid data")]
        public string? CourseCategory { get; set; }

        [Required(ErrorMessage = "Not valid data")]
        public string? Schedule { get; set; }

        [Required(ErrorMessage = "Not valid data")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Not valid data")]
        public int MasterInvoiceId { get; set; }
        public MasterInvoice? MasterInvoice { get; set; }
    }
}