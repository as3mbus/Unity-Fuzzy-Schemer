using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace as3mbus.Open_Fuzzy_Scenario.Scripts.Objects
{
    public class Linguistic_Variable 
    {
        private string linguistic_Name;
        public Dictionary<string,float> linguistics_Value =
            new Dictionary<string, float>();
        public Dictionary<string,string> linguistics_membership_function = 
            new Dictionary<string, string>();
        public void check_method(){
            
        }
        public Linguistic_Variable(string name)
        {
            this.Name = name;
        }
        public Linguistic_Variable()
        {
            this.Name = "Unknown";
        }
        
        public string Name
        {
            get { return linguistic_Name;}
            set { linguistic_Name = value;}
        }
        
    }
}
