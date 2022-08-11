namespace SecondV.Models
{
  public class MasterInvoice
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Not valid data")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Not valid data")]
        // [RegularExpression(".*[a-zA-Z]+.*", ErrorMessage = "Not valid data")]
        public string? NoInvoice { get; set; }
        [Required]
        public string? PurchaseDate { get; set; }
        [Required]
        public int Qty { get; set; }
        [Required]
        public int Cost { get; set; }
        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}