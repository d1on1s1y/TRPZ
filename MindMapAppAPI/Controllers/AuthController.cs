using Microsoft.AspNetCore.Mvc;
using MindMapApp.Server.Helpers;

namespace MindMapApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                string login = CryptoHelper.Decrypt(request.EncryptedLogin);
                string password = CryptoHelper.Decrypt(request.EncryptedPassword);
                // перевірка обл. даних, хардкод
                if (login == "admin" && password == "12345")
                {
                    return Ok(new { Token = Guid.NewGuid().ToString(), Message = "Вітаємо!" });
                }
                return Unauthorized("Невірний логін або пароль");
            }
            catch
            {
                return BadRequest("Помилка розшифрування");
            }
        }
    }
    public class LoginRequest
    {
        public string EncryptedLogin { get; set; }
        public string EncryptedPassword { get; set; }
    }
}