using System.IdentityModel.Tokens.Jwt;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.Configuration;
using UserAPI.DbContext;
using UserAPI.Helpers;
using UserAPI.Models;

namespace UserAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(
    UserManager<ApplicationUser> userManager, 
    SignInManager<ApplicationUser> signInManager,
    IJwtTokenGeneration jwtTokenGeneration,
    UserDbContext userDbContext,
    IConfiguration configuration)
    : ControllerBase
{
    // Register User
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
            };

            await userDbContext.Users.AddAsync(user);
            userDbContext.Entry(user).State = EntityState.Added;
            var result = await userDbContext.SaveChangesAsync();
            var token = jwtTokenGeneration.GenerateToken(user);
                return Ok(new { Token=token });
           
        }
        catch (Exception e)
        {
            return BadRequest("An error has occurred");
        }
    }

    [HttpGet("detail")]
    public async Task<IActionResult> UserDetails()
    {
        try
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized("Missing or invalid token");
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Invalid token");
            }

            var user = await userDbContext.Users
                .Where(u => u.Email == email)
                .Select(u => new UserDetail
                {
                    UserId = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.PhoneNumber,
                    Address = u.Address,
                    City = u.City,
                    Zip = u.Zip,
                    Country = u.Country
                }).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest("An error has occurred");
        }
    }

    // Login User
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        var result = await signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
        if (result.Succeeded)
        {
            var token = jwtTokenGeneration.GenerateToken(user);
            return Ok(new { Token=token });
        }

        return Unauthorized(new { message = "Invalid credentials" });
    }
    
    [HttpPost("validate-social-login")]
    public async Task<IActionResult> ValidateSocialLogin([FromBody] SocialLoginRequest request)
    {
        if (request.Provider != "Google") return BadRequest("Unsupported provider");
        var payload = await ValidateGoogleToken(request.IdToken);
        if (payload == null) return Unauthorized("Invalid Google Token");

        // Check if user exists in DB
        var user = await userManager.FindByEmailAsync(payload.Email);
        if (user == null)
        {
            // Create new user
            user = new ApplicationUser
            {
                UserName = payload.Email,
                Email = payload.Email,
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                Provider = "Google",
                ProviderId = payload.Subject,
                FullName = $"{payload.GivenName} {payload.FamilyName}"
            };
            await userManager.CreateAsync(user);
        }

        // Generate JWT Token
        var token = jwtTokenGeneration.GenerateToken(user);
        return Ok(new { Token=token });
    }
    
    private async Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string idToken)
    {
        try
        {
            var googleClientId = configuration["GoogleAuth:ClientId"];
            if (string.IsNullOrEmpty(googleClientId))
                throw new InvalidConfigurationException("Google Client id not found");
            
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { googleClientId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            return payload;
        }
        catch
        {
            throw new InvalidConfigurationException("Invalid Token");
        }
    }


}