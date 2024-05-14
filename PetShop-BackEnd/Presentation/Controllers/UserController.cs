using Business;
using Microsoft.AspNetCore.Mvc;
using Persistence.DTO;
using Persistence.DTO.User;

namespace PetShop_BackEnd.Controllers;

[ApiController]
[Route("/api/users")]
public class UserController: ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    public BillUserDto? Test()
    {
        return new Service1().Test();
    }
}