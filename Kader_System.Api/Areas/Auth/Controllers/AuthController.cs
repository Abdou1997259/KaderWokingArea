namespace Kader_System.Api.Areas.Auth.Controllers;

[Area(Modules.Auth)]
[ApiController]
[ApiExplorerSettings(GroupName = Modules.Auth)]
[Authorize(Permissions.Setting.View)]
//[Authorize("Superadmin")]
[Route("Api/[controller]")]
public class AuthController(IAuthService service) : ControllerBase
{
    private readonly IAuthService _service = service;

    [AllowAnonymous]
    [HttpPost(ApiRoutes.User.LoginUser)]
    public async Task<IActionResult> LoginUserAsync(AuthLoginUserRequest model)
    {
        var response = await _service.LoginUserAsync(model);
        if (response.Check)
        {
            //if (!string.IsNullOrEmpty(response.Data.RefreshToken))
            //    SetRefreshTokenInCookie(response.Data.RefreshToken, response.Data.RefreshTokenExpiration);
            return Ok(response);
        }
        else if (!response.Check)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [AllowAnonymous]
    [HttpDelete(ApiRoutes.User.LogOutUser)]
    public async Task<IActionResult> LogOutUserAsync()
    {
        var response = await _service.LogOutUserAsync();
        if (response.Check)
            return Ok(response);
        else if (!response.Check)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut(ApiRoutes.User.UpdateUser)]
    public async Task<IActionResult> UpdateUserAsync([FromRoute] string id, AuthUpdateUserRequest model)
    {
        var response = await _service.UpdateUserAsync(id, model);
        if (response.Check)
            return Ok(response);
        else if (!response.Check)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpGet(ApiRoutes.User.ShowPasswordToSpecificUser)]
    public async Task<IActionResult> ShowPasswordToSpecificUserAsync([FromRoute] string id)
    {
        var response = await _service.ShowPasswordToSpecificUserAsync(id);
        if (response.Check)
            return Ok(response);
        else if (!response.Check)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut(ApiRoutes.User.SetNewPasswordToSpecificUser)]
    public async Task<IActionResult> SetNewPasswordToSpecificUserAsync(AuthSetNewPasswordRequest model)
    {
        var response = await _service.SetNewPasswordToSpecificUserAsync(model);
        if (response.Check)
            return Ok(response);
        else if (!response.Check)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    [HttpPut(ApiRoutes.User.SetNewPasswordToSuperAdmin)]
    public async Task<IActionResult> SetNewPasswordToSuperAdminAsync([FromRoute] string newPassword)
    {
        var response = await _service.SetNewPasswordToSuperAdminAsync(newPassword);
        if (response.Check)
            return Ok(response);
        else if (!response.Check)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    //private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
    //{
    //    var cookieOptions = new CookieOptions
    //    {
    //        HttpOnly = true,
    //        Expires = expires.ToLocalTime(),
    //        Secure = true,
    //        IsEssential = true,
    //        SameSite = SameSiteMode.None
    //    };

    //    Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    //}
}
