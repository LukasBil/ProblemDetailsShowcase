using Microsoft.AspNetCore.Mvc;
using System;

namespace ProblemDetailsShowcase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<Service> GetService(Guid serviceId = new Guid())
        {
            var result = _serviceService.Get(serviceId);
            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public ActionResult<Service> AddService(Service service)
        {
            var result = _serviceService.Add(service);
            return Ok(result);
        }
    }
}
