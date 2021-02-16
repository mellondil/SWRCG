using System;
using System.Collections.Generic;
using System.Text;

namespace SWRCG
{
    public class Species
    {
        private string key;
        private string name;
        private int startingXP;
        private Characteristic characteristic;

        public Species()
        {

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

        public int StartingXP
        {
            get => startingXP;
            set => startingXP = value;
        }

        public void SetCharacteristic(Characteristic c)
        {
            characteristic = c;
        }

        public Characteristic GetCharacteristic()
        {
            return characteristic;
        }
    }
}
