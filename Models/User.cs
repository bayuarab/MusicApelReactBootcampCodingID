namespace SecondV.Models
{
  public class User
    {
        public int Id { get; set; }
        public string? nama { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        // [Required(ErrorMessage = "Not valid data")]
        // public byte[]? passwordHash { get; set; }
        // [Required(ErrorMessage = "Not valid data")]
        // public byte[]? passwordSalt { get; set; }
        public string roles { get; set; } = "student";
    }
}