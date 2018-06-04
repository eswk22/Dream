using Application.Common;
using Application.DTO.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
      public class ActionTaskDTO
    {
        public string ActionTaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Namespace { get; set; }
        public string FolderPath { get; set; }
        public string Type { get; set; }
        public string RemoteLanguage { get; set; }
        public string LocalLanguage { get; set; }
        //    [JsonProperty(PropertyName = "RemoteCode")]
        public string RemoteCode { get; set; }
        public string LocalCode { get; set; }
        public string Queuename { get; set; }
        public bool IsActive { get; set; }
        public int Timeout { get; set; }
        //   [JsonProperty(PropertyName = "Version")]
        public string Version { get; set; }
        //     [JsonProperty(PropertyName = "IsLatestVersion")]
        public RolePermissionDTO[] PermittedRoles { get; set; }
        public ParameterDTO[] Parameters { get; set; }

        public string Status { get; set; }
    }


}
