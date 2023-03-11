using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Serialization;

namespace TaskScheduler.Domain
{
    public class SCHEDULER 
    {
        [Key]
        [ForeignKey("TASK")]
        public long ID { get; set; }
        public int INTERVALINMINUTES { get; set; }
        public string STARTINGDAILYAT { get; set; }
        public string ENDINGDAILYAT { get; set; }
        public string ONDAYSOFTHEWEEK { get; set; }
        public TASK TASK { get; set; }
    }
}
