using BugAndProblemTracker.API.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BugAndProblemTracker.API.Models
{
    public class Language
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        [SwaggerIgnore]
        public string Id { get; set; }

        [BsonRequired]
        [StringLength(maximumLength:20,ErrorMessage ="Maximum name length is 20 characters")]
        [Required(ErrorMessage ="Name field cannot be blank")]
        public string Name { get; set; }



    }
}
