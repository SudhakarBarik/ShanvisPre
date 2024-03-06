using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccuFintech.Models
{
    public class PublishExamModel
    {
        public string StudentID { get; set; }
        public string QuestionSet { get; set; }
        public string Examdate { get; set; }
        public List<ConfigList> ConfigLists { get; set; }
    }

    public class ConfigList
    {
        public string ConfigField { get; set; }
        public string ConfigValue { get; set; }
    }
}