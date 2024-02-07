using Kader_System.Services.IServices.Trans;

namespace Kader_System.Api.Areas.Trans
{
    [Area(Modules.Trans)]
    [Authorize(Permissions.Transaction.View)]
    [ApiExplorerSettings(GroupName = Modules.Trans)]
    [ApiController]
    [Route("api/v1/")]
    public class TransAllowanceController(ITransAllowanceService service) : ControllerBase
    {
        #region Get
        [HttpGet(ApiRoutes.TransAllowance.ListOfTransAllowances)]
        public async Task<IActionResult> ListOfTransAllowance() =>
            Ok(await service.ListOfTransAllowancesAsync(GetCurrentRequestLanguage()));

        [HttpGet(ApiRoutes.TransAllowance.GetAllTransAllowances)]
        public async Task<IActionResult> GetAllTransAllowances([FromQuery] GetAllFilterationAllowanceRequest model) =>
            Ok(await service.GetAllTransAllowancesAsync(GetCurrentRequestLanguage(),model, GetCurrentHost()));

        [HttpGet(ApiRoutes.TransAllowance.GetTransAllowanceById)]
        public async Task<IActionResult> GetTransAllowanceById([FromRoute]int id)
        {
            var response = await service.GetTransAllowanceByIdAsync(id);
            if (response.Check)
                return Ok(response);
            else if (!response.Check)
                return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
            return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
        }
        #endregion

        #region Post

        [HttpPost(ApiRoutes.TransAllowance.CreateTransAllowance)]
        public async Task<IActionResult> CreateTransAllowance([FromBody] CreateTransAllowanceRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await service.CreateTransAllowanceAsync(request);
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


        #endregion

        #region Put
        [HttpPut(ApiRoutes.TransAllowance.UpdateTransAllowance)]
        public async Task<IActionResult> UpdateTransAllowance([FromRoute]int id,[FromBody]CreateTransAllowanceRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await service.UpdateTransAllowanceAsync(id,request);
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


        #endregion

        #region Delete

        [HttpDelete(ApiRoutes.TransAllowance.DeleteTransAllowance)]
        public async Task<IActionResult> DeleteTransAllowance(int id)
        {
            var response = await service.DeleteTransAllowanceAsync(id);
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
