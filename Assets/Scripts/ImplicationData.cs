using UnityEngine;
using System.Collections.Generic;
namespace as3mbus.OpenFuzzyScenario.Scripts.Objects
{
    public class ImplicationData
    {
        public double StartAxis = 0;
        public List<double> data = new List<double>();
        public double spacing;
        public List<double> chokePoint = new List<double>();
        public double maximum = 0;
        public List<double> MaxAxis = new List<double>();
        public double centerPoint;

    }
}
