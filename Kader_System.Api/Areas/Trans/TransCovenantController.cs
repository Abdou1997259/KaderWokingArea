using Kader_System.Services.IServices.Trans;

namespace Kader_System.Api.Areas.Trans
{
    [Area(Modules.Trans)]
    [Authorize(Permissions.Transaction.View)]
    [ApiExplorerSettings(GroupName = Modules.Trans)]
    [ApiController]
    [Route("api/v1/")]
    public class TransCovenantController(ITransCovenantService service) : ControllerBase
    {
        [HttpGet(ApiRoutes.TransCovenant.ListOfTransCovenants)]
        public async Task<IActionResult> ListOfTransCovenants() =>
            Ok(await service.ListOfTransCovenantsAsync(GetCurrentRequestLanguage()));

        [HttpGet(ApiRoutes.TransCovenant.GetTransCovenants)]
        public async Task<IActionResult> GetAllTransCovenants([FromQuery] GetAllFilterationForTransCovenant request) =>
            Ok(await service.GetAllTransCovenantsAsync(GetCurrentRequestLanguage(),request));

        [HttpPost(ApiRoutes.TransCovenant.CreateTransCovenant)]
        public async Task<IActionResult> CreateTransCovenant([FromBody] CreateTransCovenantRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await service.CreateTransCovenantAsync(request);
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

        [HttpPut(ApiRoutes.TransCovenant.UpdateTransCovenant)]
        public async Task<IActionResult> UpdateTransCovenant([FromRoute] int id, [FromBody] CreateTransCovenantRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await service.UpdateTransCovenantAsync(id,request);
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

        [HttpDelete(ApiRoutes.TransCovenant.DeleteTransCovenant)]
        public async Task<IActionResult> DeleteTransCovenant(int id)
        {
            var response = await service.DeleteTransCovenantAsync(id);
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
}
