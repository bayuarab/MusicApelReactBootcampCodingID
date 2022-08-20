using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace SecondV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAuthController : ControllerBase
    {
        private DataContext dataContext;

        private readonly IConfiguration configuration;

        public UserAuthController(DataContext dataContext, IConfiguration configuration)
        {
            this.dataContext = dataContext;
            this.configuration = configuration;
        }

        public class userChangePassword
        {
            public int Id { get; set; }
            public string? Email { get; set; }
            public string? Password { get; set; }
        }

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(UserAuthDto request)
        {
            if (request.Password == null)
                return BadRequest("invalid password");

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            
            this.dataContext.Users.Add(entity: new User {
                nama = request.Nama,
                email = request.Email,
                passwordHash = passwordHash,
                passwordSalt = passwordSalt,
                roles = request.Roles
            });
            
            await this.dataContext.SaveChangesAsync();

            return Ok("Success");
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(UserAuthDto request)
        {
            var validUser = await this.dataContext.Users.FirstOrDefaultAsync(data => data.email == request.Email);
            if (validUser == null || request.Password == null || validUser.passwordHash == null || validUser.passwordSalt == null)
                return BadRequest("Invalid user");

            var validPassword = VerifyPasswordHash(request.Password, validUser, validUser.passwordHash, validUser.passwordSalt);

            if (!validPassword)
                return BadRequest("Invalid user");

            string token = CreateToken(validUser);
            var userData = new User {
                Id = validUser.Id,
                email = validUser.email,
                nama = validUser.nama,
                roles = validUser.roles,
            };

            return Ok(new {token, userData});
        }

        [HttpPost("ChangePassword"), Authorize(Roles = "student")]
        public async Task<ActionResult<User>> ChangeUserPassword(userChangePassword request)
        {
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbContextTransaction = await this.dataContext.Database.BeginTransactionAsync();
            try
            {
                var validUser = await this.dataContext.Users.FindAsync(request.Id);
                if (validUser == null || request.Password == null || validUser.passwordHash == null || validUser.passwordSalt == null)
                    return BadRequest("Not valid data");

                if (validUser.email != request.Email)
                    return BadRequest("Not valid data");
                
                if (request.Password == null)
                    return BadRequest("invalid password");

                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                validUser.passwordHash = passwordHash;
                validUser.passwordSalt = passwordSalt;

                await this.dataContext.SaveChangesAsync();

                await dbContextTransaction.CommitAsync();

                return Ok("Success");
            }
            catch (System.Exception)
            {
                await dbContextTransaction.RollbackAsync();
                return StatusCode(500, "Unknown error occurred");
            }            
        }

        [HttpPost("PasswordValidation"), Authorize(Roles = "student")]
        public async Task<ActionResult<List<User>>> ValidationChangePassword(userChangePassword request)
        {
            try
            {
                var validUser = await this.dataContext.Users.FindAsync(request.Id);
                if (validUser == null || request.Password == null || validUser.passwordHash == null || validUser.passwordSalt == null)
                    return BadRequest("Invalid user");

                if (validUser.email != request.Email)
                    return BadRequest("Not valid data");

                var validPassword = VerifyPasswordHash(request.Password, validUser, validUser.passwordHash, validUser.passwordSalt);

                if (!validPassword)
                    return BadRequest("Invalid user");

                return Ok("Success");
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Unknown error occurred");
            }            
        }

        [HttpPost("ChangeName"), Authorize(Roles = "student")]
        public async Task<ActionResult<User>> ChangeUserName(User request)
        {
            Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction dbContextTransaction = await this.dataContext.Database.BeginTransactionAsync();
            try
            {
                var validUser = await this.dataContext.Users.FindAsync(request.Id);
                if (validUser == null)
                    return BadRequest("Not valid data");

                if (validUser.email != request.email)
                    return BadRequest("Not valid data");

                validUser.nama = request.nama;

                await this.dataContext.SaveChangesAsync();

                await dbContextTransaction.CommitAsync();

                string token = CreateToken(validUser);

                return Ok(token);
            }
            catch (System.Exception)
            {
                await dbContextTransaction.RollbackAsync();
                return StatusCode(500, "Unknown error occurred");
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, User validUser, byte[] passwordHash, byte[] passwordSalt)
        {
            if (validUser.passwordSalt == null)
                return (false);
            
            using (var hmac = new HMACSHA512(validUser.passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

         private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.nama),
                new Claim(ClaimTypes.Role, user.roles)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                configuration.GetSection("AppSettings:Token").Value
            ));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}