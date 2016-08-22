using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application1.Models
{
    public class LuisResult
    {
        public string query { get; set; }
        public List<IntentItem> intents { get; set; }
        public List<EntityItem> entities { get; set; }


        public class ValueItem
        {
            public String entity { get; set; }
            public String type { get; set; }
            public decimal score { get; set; }
        }

        public class ParameterItem
        {
            public String name { get; set; }
            public bool required { get; set; }
            public List<ValueItem> value { get; set; }
        }

        public class Action
        {
            public bool triggered { get; set; }
            public String name { get; set; }
            public List<ParameterItem> parameters { get; set; }
        }

        public class IntentItem
        {
            public String intent { get; set; }
            public decimal score { get; set; }
            public List<Action> actions { get; set; }
        }

        public class EntityItem
        {
            public String entity { get; set; }
            public String type { get; set; }
            public int startIndex { get; set; }
            public int endIndex { get; set; }
            public decimal score { get; set; }
        }


    }

}