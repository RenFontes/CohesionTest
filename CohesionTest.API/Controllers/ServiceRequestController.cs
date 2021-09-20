using CohesionTest.Models;
using CohesionTest.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CohesionTest.API.Controllers
{
    // Hardcoding 1 to make testing easier when using swagger. It should be changed to {version:apiVersion} when we add v2 methods.
    [Route("api/v1/[controller]")]
    [ApiVersion("1")]
    [ApiController]
    public class ServiceRequestController : ControllerBase
    {
        private readonly IServiceRequestService serviceRequestService;

        public ServiceRequestController(IServiceRequestService serviceRequestService)
        {
            this.serviceRequestService = serviceRequestService ?? throw new ArgumentNullException(nameof(serviceRequestService));
        }

        // GET: api/v1/<ServiceRequestController>
        [HttpGet]
        public ActionResult<ServiceRequest[]> Get()
        {
            var serviceRequests = this.serviceRequestService.GetAllServiceRequests();

            if (serviceRequests.Length > 0)
            {
                return serviceRequests;
            }
            else
            {
                return new NoContentResult();
            }
        }

        // GET api/v1/<ServiceRequestController>/727b376b-79ae-498e-9cff-a9f51b848ea4
        [HttpGet("{id}")]
        public ActionResult<ServiceRequest> Get(Guid id)
        {
            // In hindsight I should have used the "TryGet".net pattern.
            var serviceRequest = this.serviceRequestService.GetServiceRequest(id);

            if (serviceRequest != null)
            {
                return serviceRequest;
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/v1/<ServiceRequestController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ServiceRequest serviceRequest)
        {
            // bad request is returned automatically by model validation
            var id = await this.serviceRequestService.CreateServiceRequest(serviceRequest);

            return CreatedAtAction(nameof(Post), serviceRequest with { Id = id });
        }

        // PUT api/v1/<ServiceRequestController>/727b376b-79ae-498e-9cff-a9f51b848ea4
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceRequest>> Put(Guid id, [FromBody] UpdateServiceRequest updateServiceRequest)
        {
            // bad request is returned automatically by model validation
            var updatedServiceRequest = await this.serviceRequestService.UpdateServiceRequestAsync(id, updateServiceRequest);
            if (updateServiceRequest != null)
            {
                return Ok(updateServiceRequest);
            }
            return NotFound();
        }

        // DELETE api/v1/<ServiceRequestController>/727b376b-79ae-498e-9cff-a9f51b848ea4
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await this.serviceRequestService.DeleteServiceRequestAsync(id))
            {
                // Just noticed documentation said 201, but I'm pretty sure this wasn't created.
                return Ok();
            }
            return NotFound();
        }
    }
}
