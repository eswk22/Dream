using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Application.DTO.RunBook
{
    [XmlRoot(ElementName = "Task")]
    public class Task
    {
        [XmlElement(ElementName = "params")]
        public Params Params { get; set; }
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "mxCell")]
        public Cell MxCell { get; set; }
        [XmlAttribute(AttributeName = "label")]
        public string Label { get; set; }
        [XmlAttribute(AttributeName = "description")]
        public string Description { get; set; }
        [XmlAttribute(AttributeName = "tooltip")]
        public string Tooltip { get; set; }
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "merge")]
        public string Merge { get; set; }
        [XmlAttribute(AttributeName = "actiontaskid")]
        public string ActionTaskId { get; set; }

    }
}
