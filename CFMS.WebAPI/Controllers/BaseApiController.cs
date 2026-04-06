using Microsoft.AspNetCore.Mvc;

namespace CFMS.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
}