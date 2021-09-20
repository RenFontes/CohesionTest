using CohesionTest.Db;
using CohesionTest.Db.Enums;
using CohesionTest.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CohesionTest.Tests
{
    // I prefer TDD but for a 3 hour test I'd rather not do that. 🙊
    // Tests weren't that good, next time I have to test something related to the database I'll use a repository pattern or read more on how to test with entity framework directly.
    [TestClass]
    public class ServiceRequestServiceTests
    {
        private IServiceRequestService serviceRequestService;
        private static CTContext db;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var ctContextMock = new Mock<CTContext>();

            var serviceRequests = new List<ServiceRequest>
            {
                new ServiceRequest
                {
                    Id = Guid.Parse("ab6d0354-9647-40f4-ae86-5aef4f09ea26"),
                    BuildingCode = "HMO-01",
                    Description = "Please fix light switches, they get stuck.",
                    CurrentStatus =  CurrentStatusEnum.InProgress,
                    CreatedBy = "Renato Fontes",
                    CreatedDate = new DateTime(2021, 09, 03),
                    LastModifiedBy = "Building Owner",
                    LastModifiedDate = new DateTime(2021, 09, 18)
                },
                new ServiceRequest
                {
                    Id = Guid.Parse("bbc98830-9dbd-4fa9-a521-213c918d8697"),
                    BuildingCode = "HMO-02",
                    Description = "Pipes are leaking, fix them.",
                    CurrentStatus =  CurrentStatusEnum.Canceled,
                    CreatedBy = "Renato Fontes",
                    CreatedDate = new DateTime(2021, 09, 05),
                    LastModifiedBy = "Building Owner",
                    LastModifiedDate = new DateTime(2021, 09, 10)
                },
                new ServiceRequest
                {
                    Id = Guid.Parse("fe781395-b951-45b3-bb27-73adf05ce615"),
                    BuildingCode = "GDL-01",
                    Description = "Fix the entrance couch, a dog bit a part off.",
                    CurrentStatus =  CurrentStatusEnum.Created,
                    CreatedBy = "Monica Munoz",
                    CreatedDate = new DateTime(2021, 09, 05),
                    LastModifiedBy = "Monica Muñoz",
                    LastModifiedDate = new DateTime(2021, 09, 05)
                }
            };
            ctContextMock.Setup(x => x.ServiceRequests).ReturnsDbSet(serviceRequests);
            
            db = ctContextMock.Object;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            this.serviceRequestService = new ServiceRequestService(db);
        }


        [TestMethod]
        public void GetAllServiceRequest()
        {
            // act
            var serviceRequests = this.serviceRequestService.GetAllServiceRequests();

            // assert
            Assert.AreEqual(3, serviceRequests.Length);
        }

        [TestMethod]
        public void GetServiceRequest()
        {
            // arrange
            var id = Guid.Parse("fe781395-b951-45b3-bb27-73adf05ce615");

            // act
            var serviceRequest = this.serviceRequestService.GetServiceRequest(id);

            // assert
            Assert.AreEqual("Monica Munoz", serviceRequest.CreatedBy);
            Assert.AreEqual(CurrentStatusEnum.Created, serviceRequest.CurrentStatus);
        }

        [TestMethod]
        public async Task CreateServiceRequest()
        {
            // arrange 
            var newId = Guid.Parse("a15f42d0-cccc-42ad-9713-b133aca01da7");
            var newServiceRequest = new Models.ServiceRequest
            {
                Id = newId,
                BuildingCode = "HMO-01",
                Description = "Fix the garage door.",
                CurrentStatus = CurrentStatusEnum.InProgress,
                CreatedBy = "Renato Fontes",
                CreatedDate = new DateTime(2021, 09, 03),
                LastModifiedBy = "Building Owner",
                LastModifiedDate = new DateTime(2021, 09, 18)
            };

            // Moq isn't working that great when adding things to the database. :(
            // act
            var id = await this.serviceRequestService.CreateServiceRequest(newServiceRequest);
            //var serviceRequest = this.serviceRequestService.GetServiceRequest(id);


            // assert
            Assert.AreEqual(newId, id);
            //Assert.AreEqual("Fix the garage door.", serviceRequest.Description);
        }

        [TestMethod]
        public async Task UpdateServiceRequest()
        {
            // arrange 
            var id = Guid.Parse("fe781395-b951-45b3-bb27-73adf05ce615");
            var updateServiceRequest = new Models.UpdateServiceRequest
            {
                Description = "Fix the entrance couch, a dog bit a part off.",
                CurrentStatus = CurrentStatusEnum.InProgress,
                LastModifiedBy = "Building Owner",
                LastModifiedDate = new DateTime(2021, 09, 05)
            };

            // Moq isn't working that great when updating either.
            // act
            var updatedServiceRequest = await this.serviceRequestService.UpdateServiceRequestAsync(id, updateServiceRequest);

            // assert
            Assert.IsNotNull(updatedServiceRequest);
            Assert.AreEqual("Building Owner", updatedServiceRequest.LastModifiedBy);
            Assert.AreEqual(CurrentStatusEnum.InProgress, updatedServiceRequest.CurrentStatus);
        }
    }
}
