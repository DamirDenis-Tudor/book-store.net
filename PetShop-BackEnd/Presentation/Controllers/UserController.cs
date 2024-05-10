using Microsoft.AspNetCore.Mvc;

namespace PetShop_BackEnd.Controllers;

[ApiController]
[Route("/api/users")]
public class UserController: ControllerBase
{
    [HttpGet]
    public string Test()
    {
        return "test";
    }
}