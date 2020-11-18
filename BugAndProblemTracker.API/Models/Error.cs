using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugAndProblemTracker.API.Models
{
    public class Error
    {
        public string Message { get; set; }

        public string Type { get; set; }

        public string Id { get; set; }
    }
}
