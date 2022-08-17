namespace SecondV.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Not valid data")]
        public string? Method { get; set; }
        public string? Icon { get; set; }
    }
}