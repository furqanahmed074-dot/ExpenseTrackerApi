using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]

public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public IActionResult Register(UserRegistrationDto dto)
    {
        User user = new User();
        user.Email = dto.Email;
        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(null, dto.Password);
        _context.Users.Add(user);
        _context.SaveChanges();
        return Ok("Your registration is successfull.");
    }

    [HttpPost("login")]
    public IActionResult Login(UserLoginDto dto)
    {
        var user = _context.Users.FirstOrDefault(u=>u.Email == dto.Email);
        if(user == null)
        {
            return Unauthorized("Invalid email or password");
        }
        else if(user is not null)
        {
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if(result != PasswordVerificationResult.Success)
            {
                return Unauthorized("Invalid email or password.");
            }
        }
        var token = GenerateToken(user);
        return Ok(token);
    }

        private string GenerateToken(User user)
{
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Email, user.Email)
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsMySuperSecretKeyForJwtSigning12345"));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: "ExpenseTrackerApi",
        claims: claims,
        expires: DateTime.Now.AddHours(1),
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}
}