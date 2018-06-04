using Application.Common;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.DTO.ActionTask
{
	public class ActionTaskMessage
	{
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string module { get; set; }
        public string menupath { get; set; }
        public string Actiontype { get; set; }
        public string RemoteCodelanguage { get; set; }
        public string LocalCodelanguage { get; set; }
        public string RemoteCode { get; set; }
        public string AccessCode { get; set; }
        public string Queue { get; set; }
        public bool IsActive { get; set; }
        public int TimeOut { get; set; }
        public string Version { get; set; }
        public DictionaryWithDefault<string, dynamic> Inputs { get; set; }
		public DictionaryWithDefault<string, dynamic> Outputs { get; set; }
		public DictionaryWithDefault<string, dynamic> Results { get; set; }
		public DictionaryWithDefault<string, dynamic> MockInputs { get; set; }
	}
}
