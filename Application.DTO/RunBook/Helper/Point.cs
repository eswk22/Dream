using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Application.DTO.RunBook
{
    [XmlRoot(ElementName = "mxPoint")]
    public class Point
    {
        [XmlAttribute(AttributeName = "x")]
        public string X { get; set; }
        [XmlAttribute(AttributeName = "y")]
        public string Y { get; set; }
        [XmlAttribute(AttributeName = "as")]
        public string As { get; set; }
    }
}
