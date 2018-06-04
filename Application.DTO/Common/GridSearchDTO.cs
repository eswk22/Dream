using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.DTO.Common
{
    public class GridSearchDTO
    {
        public int page { get; set; }
        public int size { get; set; }
        public string sort { get; set; }
        public string quickFilter { get; set; }
        public FilterDTO[] filterPerColumn { get; set; }
    }

    public class FilterDTO
    {
        public string column { get; set; }
        public string filter { get; set; }
        public string type { get; set; }
    }
}