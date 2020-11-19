using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using BugAndProblemTracker.API.Attributes;

namespace BugAndProblemTracker.API.Models
{
    public class Bug
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonIgnoreIfDefault]
        [SwaggerIgnore]
        public string Id { get; set; }

        [BsonElement("name")]
        [BsonRequired]
        [Required(ErrorMessage ="Name field cannot be blank")]
        public string Name { get; set; }

        [BsonRequired]
        [StringLength(4000,MinimumLength =20,ErrorMessage ="Minimum 20 characters in the description")]
        public string Description { get; set; }

        [BsonRequired]
        [StringLength(24,MinimumLength =24,ErrorMessage ="Language Id must be a 24 hex string")]
        [RegularExpression(@"^[a-fA-F0-9]+$",ErrorMessage = "Language Id must be a 24 hex string")]
        public string LanguageId { get; set; }

        [StringLength(24, MinimumLength = 24, ErrorMessage = "Framework Id must be a 24 hex string")]
        [RegularExpression(@"^[a-fA-F0-9]+$", ErrorMessage = "Framework Id must be a 24 hex string")]
        public string FrameworkId { get; set; }

        [StringLength(24, MinimumLength = 24, ErrorMessage = "Library Id must be a 24 hex string")]
        [RegularExpression(@"^[a-fA-F0-9]+$", ErrorMessage = "Library Id must be a 24 hex string")]
        public string LibraryId { get; set; }

        //public Bug()
        //{
        //    LanguageId = "";

        //    FrameworkId = "";

        //    LibraryId = "";
        //}


    }
}
