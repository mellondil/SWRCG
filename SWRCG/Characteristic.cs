using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SWRCG
{
    public class Characteristic
    {
        private int brawn;
        private int agility;
        private int intellect;
        private int cunning;
        private int willpower;
        private int presence;
        private List<String> characteristicNames;

        public Characteristic()
        {
            BRAWN = 0;
            AGILITY = 0;
            INTELLECT = 0;
            CUNNING = 0;
            WILLPOWER = 0;
            PRESENCE = 0;

            characteristicNames = new List<string>
            {
                "Brawn",
                "Agility",
                "Intellect",
                "Cunning",
                "Willpower",
                "Presence"
            };
        }

        public int BRAWN
        {
            get => brawn;
            set => brawn = value;
        }

        public int AGILITY
        {
            get => agility;
            set => agility = value;
        }

        public int INTELLECT
        {
            get => intellect;
            set => intellect = value;
        }

        public int CUNNING
        {
            get => cunning;
            set => cunning = value;
        }

        public int WILLPOWER
        {
            get => willpower;
            set => willpower = value;
        }

        public int PRESENCE
        {
            get => presence;
            set => presence = value;
        }

        public List<string> GetNames()
        {
            return characteristicNames;
        }

        public Dictionary<string, int> GetCharacteristics()
        {
            Dictionary<string, int> chars = new Dictionary<string, int>();

            chars.Add(characteristicNames[0], BRAWN);
            chars.Add(characteristicNames[1], AGILITY);
            chars.Add(characteristicNames[2], INTELLECT);
            chars.Add(characteristicNames[3], CUNNING);
            chars.Add(characteristicNames[4], WILLPOWER);
            chars.Add(characteristicNames[5], PRESENCE);

            return chars;
        }

        public void SetCharacteristics(Dictionary<string, int> characteristics)
        {
            BRAWN = characteristics[characteristicNames[0]];
            AGILITY = characteristics[characteristicNames[1]];
            INTELLECT = characteristics[characteristicNames[2]];
            CUNNING = characteristics[characteristicNames[3]];
            WILLPOWER= characteristics[characteristicNames[4]];
            PRESENCE = characteristics[characteristicNames[5]];
        }

        public List<string> GetOrderedCharecteristics()
        {
            List<string> orderedChars = new List<string>();

            for (int i = 1; i <= characteristicNames.Count; i++)
            {
                if (i == BRAWN)
                {
                    orderedChars.Add(characteristicNames[0]);
                }
                if (i == AGILITY)
                {
                    orderedChars.Add(characteristicNames[1]);
                }
                if (i == INTELLECT)
                {
                    orderedChars.Add(characteristicNames[2]);
                }
                if (i == CUNNING)
                {
                    orderedChars.Add(characteristicNames[3]);
                }
                if (i == WILLPOWER)
                {
                    orderedChars.Add(characteristicNames[4]);
                }
                if (i == PRESENCE)
                {
                    orderedChars.Add(characteristicNames[5]);
                }
            }

            return orderedChars;
        }


    }
}
