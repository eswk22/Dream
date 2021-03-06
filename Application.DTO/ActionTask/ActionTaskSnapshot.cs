﻿using Application.Common;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using Application.DTO.Common;

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
        public IList<RolePermissionSnapshot> PermittedRoles { get; set; }
        public IList<ParameterSnapshot> Parameters { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
    }
}
