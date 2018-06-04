using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.WebApi.Models
{
    public class GridModel
    {
        public int page { get; set; }
        public int size { get; set; }
        public string sort { get; set; }
        public string quickFilter { get; set; }
        public FilterModel[] filterPerColumn { get; set; }
    }

    public class FilterModel
    {
        public string column { get; set; }
        public  string filter { get; set; }
        public string type { get; set; }
    }

    public class GridResultModel
    {
        public dynamic data { get; set; }
        public int totalElements { get; set; }
    }
}