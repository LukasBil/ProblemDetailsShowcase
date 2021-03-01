using System;
using System.Collections.Generic;

namespace ProblemDetailsShowcase
{
    public static class Persistence
    {
        private static Dictionary<Guid?, Service> _services = new Dictionary<Guid?, Service>();
        public static Dictionary<Guid?, Service> Services { get { return _services; } }
    }
}
