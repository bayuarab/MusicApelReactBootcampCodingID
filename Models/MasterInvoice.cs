namespace SecondV.Models
{
  public class MasterInvoice
    {
        public int Id { get; set; }
        public string? NoInvoice { get; set; }
        public string? PurchaseDate { get; set; }
        public int Qty { get; set; }
        public int Cost { get; set; }
        public int UserId { get; set; }
    }
}