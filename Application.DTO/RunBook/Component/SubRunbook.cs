using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Application.DTO.RunBook
{
    [XmlRoot(ElementName = "Subprocess")]
    public class SubRunbook
    {
        [XmlElement(ElementName = "params")]
        public Params Params { get; set; }
        [XmlElement(ElementName = "mxCell")]
        public Cell MxCell { get; set; }
        [XmlAttribute(AttributeName = "label")]
        public string Label { get; set; }
        [XmlAttribute(AttributeName = "description")]
        public string Description { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "subautomationid")]
        public string SubAutomationId { get; set; }
    }
}
