using Microsoft.AspNetCore.Mvc;
using PomeloAPI.Services;
using PomeloAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PomeloAPI.Controllers
{
    [Route("api/[controller]")]
    public class PomeloController : Controller
    {
        private readonly IServicePomeloAPI servicePomeloAPI;

        public PomeloController(IServicePomeloAPI servicePomelo)
        {
            servicePomeloAPI = servicePomelo;
        }


        [HttpGet("/users")]
        public Task<List<UserData>> Get()
        {
            return servicePomeloAPI.GetUsers();
        }

        [HttpPost("/users/create")]
        public Task<UserData> CreateUser([FromBody] CreateUserDTO user)
        { 
            return servicePomeloAPI.CreateUser(user);
        }

        [HttpGet("/users/{id}")]
        public Task<UserData> Get(string id)
        {
            return servicePomeloAPI.GetUser(id);
        }

    
    }
}

