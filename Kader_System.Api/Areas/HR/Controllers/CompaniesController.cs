﻿namespace Kader_System.Api.Areas.Setting.Controllers;

[Area(Modules.HR)]
[ApiExplorerSettings(GroupName = Modules.HR)]
[ApiController]
[Route("api/v1/")]
[AllowAnonymous]
public class CompaniesController(ICompanyService service) : ControllerBase
{
    #region Retreive

    [HttpGet(ApiRoutes.Company.ListOfCompanies)]
    public async Task<IActionResult> ListOfCompaniesAsync() =>
        Ok(await service.ListOfCompaniesAsync(GetCurrentRequestLanguage()));

    [HttpGet(ApiRoutes.Company.GetAllCompanies)]
    public async Task<IActionResult> GetAllCompaniesAsync([FromQuery] HrGetAllFiltrationsForCompaniesRequest model) =>
        Ok(await service.GetAllCompaniesAsync(GetCurrentRequestLanguage(), model));

    [HttpGet(ApiRoutes.Company.GetCompanyById)]
    public async Task<IActionResult> GetCompanyByIdAsync(int id)
    {
        var response = await service.GetCompanyByIdAsync(id);
        if (response.Check)
            return Ok(response);
        else if (!response.Check)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    #endregion


    #region Insert

    [HttpPost(ApiRoutes.Company.CreateCompany)]
    public async Task<IActionResult> CreateCompanyAsync([FromForm] HrCreateCompanyRequest model)
    {
        var response = await service.CreateCompanyAsync(model);
        if (response.Check)
            return Ok(response);
        else if (!response.Check)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    #endregion

    #region Update

    [HttpPut(ApiRoutes.Company.UpdateCompany)]
    public async Task<IActionResult> UpdateCompanyAsync([FromRoute] int id, [FromForm] HrUpdateCompanyRequest model)
    {
        var response = await service.UpdateCompanyAsync(id, model);
        if (response.Check)
            return Ok(response);
        else if (!response.Check)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }

    #endregion


    #region Delete
    [HttpDelete(ApiRoutes.Company.DeleteCompany)]
    public async Task<IActionResult> DeleteCompanyAsync(int id)
    {
        var response = await service.DeleteCompanyAsync(id);
        if (response.Check)
            return Ok(response);
        else if (!response.Check)
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
        return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
    }



    #endregion

    #region Helpers

    private string GetCurrentRequestLanguage() =>
        Request.Headers.AcceptLanguage.ToString().Split(',').First();
    #endregion

}
