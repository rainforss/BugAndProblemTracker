using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BugAndProblemTracker.API.Models
{
    public class Bug
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        public string Id { get; set; }

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; }

        [BsonRequired]
        [StringLength(4000,MinimumLength =20,ErrorMessage ="Minimum 20 characters in the description")]
        public string Description { get; set; }
        
        public string FrameworkId { get; set; }

        public string LibraryId { get; set; }

        public Bug()
        {
            FrameworkId = "";

            LibraryId = "";
        }


    }
}
