using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Application.DTO.RunBook
{
    [XmlRoot(ElementName = "root")]
    public class RunbookEntity
    {
        [XmlElement(ElementName = "runbookid")]
        public string runbookId { get; set; }
        [XmlElement(ElementName = "mxCell")]
        public List<Cell> mxCell { get; set; }
        [XmlElement(ElementName = "Start")]
        public Start start { get; set; }
        [XmlElement(ElementName = "Task")]
        public List<Task> tasks { get; set; }
        [XmlElement(ElementName = "Precondition")]
        public List<Precondition> preConditions { get; set; }
        [XmlElement(ElementName = "Subprocess")]
        public List<SubRunbook> subRunbook { get; set; }
        [XmlElement(ElementName = "End")]
        public List<End> ends { get; set; }
        [XmlElement(ElementName = "Edge")]
        public List<Connector> connectors { get; set; }
        [XmlElement(ElementName = "Container")]
        public List<Container> containers { get; set; }
        [XmlElement(ElementName = "Event")]
        public List<Event> events { get; set; }
        [XmlElement(ElementName = "Text")]
        public List<Text> texts { get; set; }

    }
}
