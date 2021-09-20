using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CohesionTest.Services
{
    public interface IServiceRequestService
    {
        Task<Guid> CreateServiceRequest(Models.ServiceRequest serviceRequest);
        Task<bool> DeleteServiceRequestAsync(Guid id);
        Models.ServiceRequest[] GetAllServiceRequests();
        Models.ServiceRequest GetServiceRequest(Guid id);
        Task<bool> UpdateServiceRequestAsync(Guid id, Models.ServiceRequest serviceRequest);
    }
}
