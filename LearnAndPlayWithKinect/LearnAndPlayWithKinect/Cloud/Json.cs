using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace LearnAndPlayWithKinect.Cloud
{
    class Json
    {
        public static RootObject Deserialize(string json)
        {
            //System.Web.Script.Serialization.JavaScriptSerializer jsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            RootObject root = new RootObject();

            try
            {
                root = JsonConvert.DeserializeObject<RootObject>(json);
            }
            catch (Exception)
            {

            }

            return root;
        }
    }
}
