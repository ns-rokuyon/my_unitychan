using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyUnityChan {
    public class EnumLabelAttribute : PropertyAttribute {
        public List<string> labels { get; private set; }

        public EnumLabelAttribute(Type t) {
            labels = Enum.GetNames(t).ToList();
        }
    }
}
