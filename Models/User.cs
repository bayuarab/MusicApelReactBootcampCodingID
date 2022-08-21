namespace SecondV.Models
{
  public class User
    {
        public int Id { get; set; }
        public string? nama { get; set; }
        public string? email { get; set; }
        public byte[]? passwordHash { get; set; }
        public byte[]? passwordSalt { get; set; }
        public string roles { get; set; } = "student";
    }
}