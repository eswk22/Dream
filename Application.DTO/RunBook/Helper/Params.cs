using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Application.DTO.RunBook
{
    [XmlRoot(ElementName = "params")]
    public class Params
    {
        [XmlElement(ElementName = "inputs")]
        public string Inputs { get; set; }
        [XmlElement(ElementName = "outputs")]
        public string Outputs { get; set; }

        
    }
}
