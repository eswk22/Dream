using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Application.DTO.RunBook
{
    [XmlRoot(ElementName = "Edge")]
    public class Connector
    {
        [XmlElement(ElementName = "mxCell")]
        public Cell cell { get; set; }
        [XmlAttribute(AttributeName = "label")]
        public string Label { get; set; }
        [XmlAttribute(AttributeName = "description")]
        public string Description { get; set; }
        [XmlAttribute(AttributeName = "order")]
        public string Order { get; set; }
        [XmlAttribute(AttributeName = "condition")]
        public string Condition { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }   
}
