using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnAndPlayWithKinect.Cloud
{
    public class Datum
    {
        public string score { get; set; }
        public string user { get; set; }
        public string creation_time { get; set; }
    }

    public class RootObject
    {
        public bool result { get; set; }
        public object info { get; set; }
        public List<Datum> data { get; set; }
    }
}
