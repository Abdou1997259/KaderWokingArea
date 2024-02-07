using Kader_System.Services.IServices.Trans;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kader_System.Api.Areas.Trans
{
    [Area(Modules.Trans)]
    [Authorize(Permissions.Transaction.View)]
    [ApiExplorerSettings(GroupName = Modules.Trans)]
    [ApiController]
    [Route("api/v1/")]
    public class TransBenefitsController(ITransBenefitService service) : ControllerBase
    {
        [HttpGet(ApiRoutes.TransBenefit.ListOfTransBenefits)]
        public async Task<IActionResult> ListOfTransBenefits() =>
            Ok(await service.ListOfTransBenefitsAsync(GetCurrentRequestLanguage()));

        [HttpGet(ApiRoutes.TransBenefit.GetTransBenefits)]
        public async Task<IActionResult> GetAllTransBenefits([FromQuery] GetAllFilterationForTransBenefitRequest request) =>
            Ok(await service.GetAllTransBenefitsAsync(GetCurrentRequestLanguage(),request, GetCurrentHost()));

        [HttpPost(ApiRoutes.TransBenefit.CreateTransBenefit)]
        public async Task<IActionResult> CreateTransBenefit([FromBody] CreateTransBenefitRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await service.CreateTransBenefitAsync(request);
                if (response.Check)
                    return Ok(response);
                else if (!response.Check)
                    return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
                return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut(ApiRoutes.TransBenefit.UpdateTransBenefit)]
        public async Task<IActionResult> UpdateTransBenefit([FromRoute] int id, [FromBody] CreateTransBenefitRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await service.UpdateTransBenefitAsync(id,request);
                if (response.Check)
                    return Ok(response);
                else if (!response.Check)
                    return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
                return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        #region Delete

        [HttpDelete(ApiRoutes.TransBenefit.DeleteTransBenefit)]
        public async Task<IActionResult> DeleteTransBenefit(int id)
        {
            var response = await service.DeleteTransBenefitAsync(id);
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
        private string GetCurrentHost() =>
            HttpContext.Request.Host.Value +
            HttpContext.Request.Path.Value;

        #endregion
    }
}
