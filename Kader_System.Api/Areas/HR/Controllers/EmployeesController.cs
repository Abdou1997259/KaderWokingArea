namespace Kader_System.Api.Areas.HR.Controllers
{
    [Area(Modules.HR)]
    [ApiExplorerSettings(GroupName = Modules.HR)]
    [ApiController]
    [Authorize(Permissions.HR.View)]
    [Route("api/v1/")]
    public class EmployeesController(IEmployeeService employeeService) : ControllerBase
    {
        #region Get

        [HttpGet(ApiRoutes.Employee.ListOfEmployees)]
        public async Task<IActionResult> ListOfEmployeesAsync() =>
            Ok(await employeeService.ListOfEmployeesAsync(GetCurrentRequestLanguage()));



        [HttpGet(ApiRoutes.Employee.GetAllEmployees)]
        public async Task<IActionResult> GetAllEmployeesAsync([FromQuery]GetAllEmployeesFilterRequest request) =>
            Ok(await employeeService.GetAllEmployeesAsync(GetCurrentRequestLanguage(), request));


        [HttpGet(ApiRoutes.Employee.GetEmployeeById)]

        public async Task<IActionResult> GetEmployeeByIdAsync(int id)
        {
            var response = await employeeService.GetEmployeeByIdAsync(id);
            if (response.Check)
                return Ok(response);
            else if (!response.Check)
                return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
            return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
        }


        #endregion


        #region Post
        [HttpPost(ApiRoutes.Employee.CreateEmployee)]
        public async Task<IActionResult> CreateEmployeeAsync([FromForm] CreateEmployeeRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await employeeService.CreateEmployeeAsync(request);
                if (response.Check)
                    return Ok(response);
                else if (!response.Check)
                    return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
                return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
            }
            return StatusCode(statusCode: StatusCodes.Status400BadRequest, request);
        }


        #endregion


        #region Put

        [HttpPut(ApiRoutes.Employee.UpdateEmployee)]
        public async Task<IActionResult> UpdateEmployeeAsyncTask([FromRoute]int id,[FromForm] CreateEmployeeRequest request)
        {
            var response = await employeeService.UpdateEmployeeAsync(id, request);
            if (response.Check)
                return Ok(response);
            else if (!response.Check)
                return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
            return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
        }


        #endregion


        #region Delete

        [HttpDelete(ApiRoutes.Employee.DeleteEmployee)]
        public async Task<IActionResult> DeleteEmployeeAsyncTask(int id)
        {
            var response = await employeeService.DeleteEmployeeAsync(id);
            if (response.Check)
                return Ok(response);
            else if (!response.Check)
                return StatusCode(statusCode: StatusCodes.Status400BadRequest, response);
            return StatusCode(statusCode: StatusCodes.Status500InternalServerError, response);
        }

        #endregion


        #region Helper
        private string GetCurrentRequestLanguage() =>
            Request.Headers.AcceptLanguage.ToString().Split(',').First();
        #endregion

    }
}
