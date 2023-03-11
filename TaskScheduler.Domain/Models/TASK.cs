using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TaskScheduler.Domain
{
    public class TASK 
    {
        public long ID { get; set; }
        public bool ENABLE { get; set; }
        public string NAME { get; set; }
        //URL WebService
        public string JOB { get; set; }
        public SCHEDULER SCHEDULER { get; set; }

    }
}
