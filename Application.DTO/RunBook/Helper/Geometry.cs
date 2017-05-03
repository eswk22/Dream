using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Application.DTO.RunBook
{
    [XmlRoot(ElementName = "mxGeometry")]
    public class Geometry
    {
        [XmlAttribute(AttributeName = "x")]
        public string X { get; set; }
        [XmlAttribute(AttributeName = "y")]
        public string Y { get; set; }
        [XmlAttribute(AttributeName = "width")]
        public string Width { get; set; }
        [XmlAttribute(AttributeName = "height")]
        public string Height { get; set; }
        [XmlAttribute(AttributeName = "as")]
        public string As { get; set; }
        [XmlAttribute(AttributeName = "relative")]
        public string Relative { get; set; }
        [XmlElement(ElementName = "mxPoint")]
        public List<Point> MxPoint { get; set; }
        [XmlElement(ElementName = "Array")]
        public ArrayofPoints Array { get; set; }
    }
}
