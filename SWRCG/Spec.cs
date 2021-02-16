using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SWRCG
{
    public class Spec
    {
        private string key;
        private string name;
        private List<Career> careers;
        private Characteristic charPreference;

        public Spec()
        {
            careers = new List<Career>();
            charPreference = new Characteristic();
        }

        public string Key
        {
            get => key;
            set => key = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public List<Career> GetCareers()
        {
            return careers;
        }

        public void AddCareer(Career career)
        {
            careers.Add(career);
        }

        public void SetCharPref(Characteristic characteristic)
        {
            charPreference = characteristic;
        }

        public Characteristic GetCharPref()
        {
            return charPreference;
        }
    }
}
