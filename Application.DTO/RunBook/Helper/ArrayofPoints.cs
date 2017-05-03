using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Application.DTO.RunBook
{
    [XmlRoot(ElementName = "Array")]
    public class ArrayofPoints
    {
        [XmlElement(ElementName = "mxPoint")]
        public Point MxPoint { get; set; }
        [XmlAttribute(AttributeName = "as")]
        public string As { get; set; }
    }
}
