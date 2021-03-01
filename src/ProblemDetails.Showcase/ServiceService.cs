using ProblemDetailsShowcase.Exceptions;
using System;
using System.Linq;

namespace ProblemDetailsShowcase
{
    public class ServiceService : IServiceService
    {
        public Service Get(Guid serviceId)
        {
            if(Persistence.Services.TryGetValue(serviceId, out var service))
            {
                return service;
            }
            else
            {
                throw new NotFoundException($"Service with id {serviceId} wasn't found.");
            }
        }

        public Service Add(Service service)
        {
            if (!Persistence.Services.Values.Any(x => x.Name == service.Name))
            {
                service.Id = Guid.NewGuid();
                Persistence.Services.Add(service.Id, service);
                return service;
            }

            throw new CustomValidationException(nameof(service.Name), $"Service with name {service.Name} already exists");
        }

    }
}
