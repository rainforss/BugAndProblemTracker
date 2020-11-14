using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugAndProblemTracker.API.Models
{
    public class MongoDBSettings:IMongoDBSettings
    {
        public string ConnectionString { get; set; }
    }

    public interface IMongoDBSettings
    {
        string ConnectionString { get; set; }
    }
}
