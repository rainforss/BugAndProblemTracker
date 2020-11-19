using BugAndProblemTracker.API.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugAndProblemTracker.API.Models
{
    public class Library
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [SwaggerIgnore]
        public string Id { get; set; }

        [BsonRequired]
        [StringLength(20)]
        public string Name { get; set; }
        public string LanguageId { get; set; }
    }
}
