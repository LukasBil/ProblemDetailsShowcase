using System;

namespace ProblemDetailsShowcase
{
    public interface IServiceService
    {
        public Service Get(Guid serviceId);
        public Service Add(Service service);
    }
}