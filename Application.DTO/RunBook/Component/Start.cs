﻿using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Application.DTO.RunBook
{
    [XmlRoot(ElementName = "Start")]
    public class Start
    {
        [XmlElement(ElementName = "params")]
        public Params Params { get; set; }
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "mxCell")]
        public Cell cell { get; set; }
        [XmlAttribute(AttributeName = "label")]
        public string Label { get; set; }
        [XmlAttribute(AttributeName = "description")]
        public string Description { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }
}