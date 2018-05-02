using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace as3mbus.Open_Fuzzy_Scenario.Scripts.Object
{
    public class linguistic_Variable 
    {
        public string linguistic_Name;
        public Dictionary<string,float> linguistics_Value =
            new Dictionary<string, float>();
        public Dictionary<string,string> linguistics_membership_function = 
            new Dictionary<string, string>();
        public void check_method(){
            
        }
        
    }
}
