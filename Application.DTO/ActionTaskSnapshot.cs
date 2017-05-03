using Application.Common;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Snapshot
{
	[CollectionName("CustomersTest")]
	public class ActionTaskSnapshot : Entity
	{
		[BsonElement("Id")]
		public override string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Summary { get; set; }
		public string CreatedBy { get; set; }
		public string UpdatedBy { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime UpdatedOn { get; set; }
		public string module { get; set; }
		public string menupath { get; set; }
		public string Actiontype { get; set; }
		public string Codelanguage { get; set; }
		public string RemoteCode { get; set; }
		public string AccessCode { get; set; }
		public int TimeOut { get; set; }
		public DictionaryWithDefault<string, string> Inputs { get; set; }
		public DictionaryWithDefault<string, string> Outputs { get; set; }
		public DictionaryWithDefault<string, string> Results { get; set; }
		public DictionaryWithDefault<string, string> MockInputs { get; set; }
		public bool IsActive { get; set; }
	}
}
