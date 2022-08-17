// using System.Security.Cryptography;
// using Microsoft.AspNetCore.Mvc;

// namespace SecondV.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class UserAuthController : ControllerBase
//     {
//         private DataContext dataContext;

//         public UserAuthController(DataContext dataContext)
//         {
//             this.dataContext = dataContext;
//         }

//         [HttpPost("Register")]
//         public async Task<ActionResult<User>> Register(UserAuthDto request)
//         {
//             if (request.Password == null)
//                 return BadRequest("invalid password");

//             CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            
//             this.dataContext.Users.Add(entity: new User {
//                 nama = request.Nama,
//                 email = request.Email,
//                 passwordHash = passwordHash,
//                 passwordSalt = passwordSalt,
//                 password = request.Password
//             });
            
//             await this.dataContext.SaveChangesAsync();

//             return Ok("Success");
//         }

//         [HttpPost("Login")]
//         public async Task<ActionResult<string>> Login(UserAuthDto request)
//         {
//             var validUser = await this.dataContext.Users.FirstOrDefaultAsync(data => data.email == request.Email);
//             if (validUser == null || request.Password == null || validUser.passwordHash == null || validUser.passwordSalt == null)
//                 return BadRequest("Invalid user");

//             var validPassword = VerifyPasswordHash(request.Password, validUser, validUser.passwordHash, validUser.passwordSalt);

//             if (!validPassword)
//                 return BadRequest("Invalid user");

//             return Ok("Login Success");
//         }

//         private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
//         {
//             using(var hmac = new HMACSHA512())
//             {
//                 passwordSalt = hmac.Key;
//                 passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
//             }
//         }

//         private bool VerifyPasswordHash(string password, User validUser, byte[] passwordHash, byte[] passwordSalt)
//         {
//             if (validUser.passwordSalt == null)
//                 return (false);
            
//             using (var hmac = new HMACSHA512(validUser.passwordSalt))
//             {
//                 var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
//                 return computeHash.SequenceEqual(passwordHash);
//             }
//         }
//     }
// }