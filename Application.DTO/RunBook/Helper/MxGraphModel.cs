using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Application.DTO.RunBook.Helper
{
    [XmlRoot(ElementName = "mxGraphModel")]
    public class MxGraphModel
    {
        [XmlElement(ElementName = "root")]
        public RunbookEntity Root { get; set; }
    }
}
