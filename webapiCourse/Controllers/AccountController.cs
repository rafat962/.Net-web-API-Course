using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using webapiCourse.models;
using webapiCourse.models.DTOs;

namespace webapiCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            UserManager = userManager;
            _configuration = configuration;
        }

        public UserManager<ApplicationUser> UserManager { get; }
        public IConfiguration _configuration { get; }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {

            if (ModelState.IsValid)
            {
                // create

                ApplicationUser user = new ApplicationUser() { 
                
                UserName = registerDTO.UserName,
                PasswordHash = registerDTO.Password,
                Email = registerDTO.Email,

                };

                IdentityResult result = await  UserManager.CreateAsync(user, registerDTO.Password);

                if (result.Succeeded)
                {

                    return Ok(result);
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

            }
                return BadRequest(ModelState);

        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO userFromRequest)
        {

            if (ModelState.IsValid)
            {
                // check valid
                ApplicationUser appUser = await UserManager.FindByNameAsync(userFromRequest.UserName);
                // generate token
                if (appUser != null)
                {
                    // check password
                    bool found = await UserManager.CheckPasswordAsync(appUser, userFromRequest.Password);

                    if (found)
                    {
                        List<Claim> extraClaim = new List<Claim>();

                        extraClaim.Add(new Claim(ClaimTypes.Name, appUser.UserName));
                        extraClaim.Add(new Claim(ClaimTypes.NameIdentifier, appUser.Id));
                        extraClaim.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        var roles = await UserManager.GetRolesAsync(appUser);

                        foreach (var item in roles)
                        {
                            extraClaim.Add(new Claim(ClaimTypes.Role, item));
                        }

                        // 2/ define signtureCredintials


                        string key = _configuration["JwtSettings:SecretKey"];
                        string audience = _configuration["JwtSettings:Audience"];
  
                        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(key));

                        SigningCredentials mySinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                        // design token
                        JwtSecurityToken token = new JwtSecurityToken(
                            audience: audience,
                            expires: DateTime.Now.AddHours(1),
                            claims: extraClaim,
                            signingCredentials: mySinCredentials
                        );

                        // generate compact token

                        return Ok(new { 
                            
                            expired = DateTime.Now.AddHours(1),
                            token = new JwtSecurityTokenHandler().WriteToken(token)


                        });


                        // You may want to return the token string here
                        //return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                    }

                }
                ModelState.AddModelError("", "Invalid account");
            }

            return BadRequest(ModelState);

        }
    }
}
