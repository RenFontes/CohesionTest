using CohesionTest.Db;
using CohesionTest.Db.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CohesionTest.Services
{
    // I normally name classes that "do things" Service. Kinda bad that this was already called *Service*Request. 😬
    public class ServiceRequestService : IServiceRequestService
    {
        private readonly CTContext db;

        public ServiceRequestService(CTContext db)
        {
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Gets ServiceRequest from db by Id
        /// </summary>
        /// <param name="id">Id of ServiceRequest</param>
        /// <returns>ServiceRequest or null if not found</returns>
        public Models.ServiceRequest GetServiceRequest(Guid id)
        {
            var dbServiceRequest = this.db.ServiceRequests.FirstOrDefault(sr => sr.Id == id);

            if (dbServiceRequest == null)
            {
                return null;
            }

            var serviceRequest = this.ConvertDbModelToModel(dbServiceRequest);

            return serviceRequest;
        }

        /// <summary>
        /// Gets all ServiceRequests from the database.
        /// </summary>
        /// <returns></returns>
        public Models.ServiceRequest[] GetAllServiceRequests()
        {
            var serviceRequests = this.db.ServiceRequests.ToArray().Select(this.ConvertDbModelToModel).ToArray();
            return serviceRequests;
        }

        /// <summary>
        /// Creates new ServiceRequest.
        /// </summary>
        /// <param name="serviceRequest">ServiceRequest</param>
        public async Task<Guid> CreateServiceRequest(Models.ServiceRequest serviceRequest)
        {
            var dbServiceRequest = this.ConvertModelToDbModel(serviceRequest);
            this.db.ServiceRequests.Add(dbServiceRequest);
            await this.db.SaveChangesAsync();

            return dbServiceRequest.Id;
        }

        /// <summary>
        /// Updates a ServiceRequest.
        /// </summary>
        /// <param name="id">ServiceRequest Id</param>
        /// <param name="updateServiceRequest">Updated ServiceRequest</param>
        /// <returns>Update service request or null if there was nothing to update.</returns>
        public async Task<Models.ServiceRequest> UpdateServiceRequestAsync(Guid id, Models.UpdateServiceRequest updateServiceRequest)
        {
            // Avoid attempt to update row if it doesn't exist.
            // Update would insert it if it didn't exist and we don't want that.
            var dbServiceRequest = this.db.ServiceRequests.FirstOrDefault(sr => sr.Id == id);
            if (dbServiceRequest == null)
            {
                return null;
            }

            if (updateServiceRequest.CurrentStatus == CurrentStatusEnum.Complete && ((CurrentStatusEnum)dbServiceRequest.CurrentStatus) != CurrentStatusEnum.Complete)
            {
                // TODO: Send email
                // My idea was to use firebase to send the email but that didn't work out.
                // It seems that the .net firebase admin doesn't have support for that feature (I had previously used that with node)
            }

            dbServiceRequest.Description = updateServiceRequest.Description;
            dbServiceRequest.CurrentStatus = updateServiceRequest.CurrentStatus;
            dbServiceRequest.LastModifiedBy = updateServiceRequest.LastModifiedBy;
            dbServiceRequest.LastModifiedDate = updateServiceRequest.LastModifiedDate;
            await this.db.SaveChangesAsync();

            var serviceRequest = this.ConvertDbModelToModel(dbServiceRequest);

            return serviceRequest;
        }

        /// <summary>
        /// Deletes a ServiceRequest.
        /// </summary>
        /// <param name="id">Id of ServiceRequest to delete</param>
        /// <returns>Bool value indicating if the ServiceRequest was deleted successfully</returns>
        public async Task<bool> DeleteServiceRequestAsync(Guid id)
        {

            // Avoid attempt to delete row if it doesn't exist.
            var dbServiceRequest = this.db.ServiceRequests.FirstOrDefault(sr => sr.Id == id);
            if (dbServiceRequest == null)
            {
                return false;
            }

            this.db.ServiceRequests.Remove(dbServiceRequest);
            return (await this.db.SaveChangesAsync() > 0);
        }

        private Models.ServiceRequest ConvertDbModelToModel(Db.ServiceRequest dbServiceRequest)
        {
            var serviceRequest = new Models.ServiceRequest
            {
                Id = dbServiceRequest.Id,
                BuildingCode = dbServiceRequest.BuildingCode,
                Description = dbServiceRequest.Description,
                CurrentStatus = dbServiceRequest.CurrentStatus,
                CreatedBy = dbServiceRequest.CreatedBy,
                CreatedDate = dbServiceRequest.CreatedDate,
                LastModifiedBy = dbServiceRequest.LastModifiedBy,
                LastModifiedDate = dbServiceRequest.LastModifiedDate
            };

            return serviceRequest;
        }

        private Db.ServiceRequest ConvertModelToDbModel(Models.ServiceRequest serviceRequest)
        {
            var dbServiceRequest = new Db.ServiceRequest
            {
                Id = serviceRequest.Id ?? Guid.NewGuid(), // Create new Guid if client didn't provide one.
                BuildingCode = serviceRequest.BuildingCode,
                Description = serviceRequest.Description,
                CurrentStatus = serviceRequest.CurrentStatus,
                CreatedBy = serviceRequest.CreatedBy,
                CreatedDate = serviceRequest.CreatedDate,
                LastModifiedBy = serviceRequest.LastModifiedBy,
                LastModifiedDate = serviceRequest.LastModifiedDate
            };

            return dbServiceRequest;
        }
    }
}
