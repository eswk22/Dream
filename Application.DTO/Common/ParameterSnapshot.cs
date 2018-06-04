using Application.Common;
using Application.DTO.RunBook;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Common
{
    [CollectionName("parameters")]
    public class ParameterSnapshot : Entity
    {
        [BsonElement("Id")]
        public override string Id { get; set; }
        public string ParentType { get; set; }
        public string ParentId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string DefaultValue { get; set; }
        public string ParameterType { get; set; }
        public Boolean IsActive { get; set; }
    }
}
