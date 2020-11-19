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
    public class Framework
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [SwaggerIgnore]
        public string Id { get; set; }

        [BsonRequired]
        [StringLength(20,ErrorMessage ="Maximum 20 characters for framework name")]
        [Required(ErrorMessage ="Name field cannot be blank")]
        public string Name { get; set; }

        [BsonRequired]
        [Required(ErrorMessage ="Language Id cannot be blank")]
        public string LanguageId { get; set; }
    }
}
