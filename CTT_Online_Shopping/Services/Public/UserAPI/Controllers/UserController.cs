using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserAPI.DbContext;
using UserAPI.Models;

namespace UserAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
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
}