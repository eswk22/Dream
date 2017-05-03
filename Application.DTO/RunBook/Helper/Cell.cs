using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Application.DTO.RunBook
{
    [XmlRoot(ElementName = "mxCell")]
    public class Cell
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "parent")]
        public string Parent { get; set; }
        [XmlElement(ElementName = "mxGeometry")]
        public Geometry geometry { get; set; }
        [XmlAttribute(AttributeName = "style")]
        public string Style { get; set; }
        [XmlAttribute(AttributeName = "vertex")]
        public string Vertex { get; set; }
        [XmlAttribute(AttributeName = "source")]
        public string Source { get; set; }
        [XmlAttribute(AttributeName = "target")]
        public string Target { get; set; }
        [XmlAttribute(AttributeName = "edge")]
        public string Edge { get; set; }
    }
}
