using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SWRCG
{
    public class Career
    {
        private string name;
        private Dictionary<string, Spec> specs = new Dictionary<string, Spec>();
        private bool hasForce = false;

        public Career()
        {

        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public bool HasForce
        {
            get => hasForce;
            set => hasForce = value;
        }

        public void SetSpec(string specKey, Spec spec = null)
        {
            if (!specs.ContainsKey(specKey))
            {
                specs.Add(specKey, spec);
            }

            specs[specKey] = spec;
        }

        public Spec GetSpec(string specKey)
        {
            if (specs.ContainsKey(specKey))
            {
                return specs[specKey];
            }

            return null;
        }

        public Dictionary<string, Spec>.KeyCollection GetSpecKeys()
        {
            return specs.Keys;
        }
    }
}
