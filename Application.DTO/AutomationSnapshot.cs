using Application.Common;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using Application.DTO.RunBook;

namespace Application.DTO
{
    [CollectionName("CustomersTest")]
    public class AutomationEntity : Entity
    {
        [BsonElement("Id")]
        public override string Id { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string summary { get; set; }
        public RunbookEntity runbookContent { get; set; }
        public RunbookEntity runbookException { get; set; }
        public int version { get; set; }
        public bool isLatestVersion { get; set; }
        public bool isActive { get; set; }
        public string createdBy { get; set; }
        public string modifiedBy { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime modifiedOn { get; set; }

    }
}
