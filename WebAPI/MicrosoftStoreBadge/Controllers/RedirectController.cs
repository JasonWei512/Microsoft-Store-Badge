using Microsoft.AspNetCore.Mvc;

namespace MicrosoftStoreBadge.Controllers;

[ApiController]
[Route("")]
public class RedirectController : ControllerBase
{
    /// <summary>
    /// Redirect to GitHub project page.
    /// </summary>
    [HttpGet()]
    public IActionResult Redirect()
    {
        return Redirect("https://github.com/JasonWei512/Microsoft-Store-Badge");
    }
}
