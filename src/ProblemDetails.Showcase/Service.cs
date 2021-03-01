using System;
using System.ComponentModel.DataAnnotations;

namespace ProblemDetailsShowcase
{
    public class Service
    {
        public Guid? Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}