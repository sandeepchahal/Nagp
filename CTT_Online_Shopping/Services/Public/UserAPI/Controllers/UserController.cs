using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FullName = model.FullName
        };

        var result = await userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            return Ok(new { message = "User created successfully" });
        }

        return BadRequest(result.Errors);
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

        var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);
        if (result.Succeeded)
        {
            return Ok(new { message = "Login successful" });
        }

        return Unauthorized(new { message = "Invalid credentials" });
    }
    
    [HttpPost("validate-social-login")]
    public async Task<IActionResult> ValidateSocialLogin([FromBody] SocialLoginRequest request)
    {
        if (request.Provider == "Google")
        {
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
            return Ok(new { token });
        }
        return BadRequest("Unsupported provider");
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