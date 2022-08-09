namespace SecondV.Models
{
  public class InvoiceDetail
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Not valid data")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Not valid data")]
        [RegularExpression(".*[a-zA-Z]+.*", ErrorMessage = "Not valid data")]
        public string NoInvoice { get; set; }
        [Required]
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        [Required]
        public int MasterInvoiceId { get; set; }
        public MasterInvoice? MasterInvoice { get; set; }
    }
}