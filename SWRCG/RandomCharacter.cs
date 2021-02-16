using System;
using System.Collections.Generic;
using System.Text;

namespace SWRCG
{
    public class RandomCharacter
    {
        private Species species;
        private Career career;
        private Spec spec;
        private int obligationXP;
        private int availableXP;

        private Characteristic newCharateristic;

        private readonly int MAX_CHAR_NUM = 4;
        private readonly int DROID_ALT_CHAR_NUM = 3;

        public RandomCharacter(Species sc, Career cr, Spec sp, int obXP = 0)
        {
            species = sc;
            career = cr;
            spec = sp;
            obligationXP = obXP;
            availableXP = species.StartingXP + obligationXP;

            newCharateristic = CalulateChararcteristics();
        }

        private Characteristic CalulateChararcteristics()
        {
            Characteristic newChar = new Characteristic();

            Dictionary<string, int> speciesChar = species.GetCharacteristic().GetCharacteristics();
            
            List<string> orderedSpecCharPref = spec.GetCharPref().GetOrderedCharecteristics();

            // Check for Droid and randomize characteristic preference.
            bool isDroid = false;
            if (species.Key == "DROID" && Randomizer.Instance.GetOneOrZero() == 0)
            {
                isDroid = true;
            }

            // Apply XP to the spec's prefered characteristics.
            for (int i = 0; i < orderedSpecCharPref.Count; i++)
            {
                int charPrefNum = speciesChar[orderedSpecCharPref[i]];

                speciesChar[orderedSpecCharPref[i]] = CalcIndividualChar(charPrefNum, isDroid ? DroidPref(i) : MAX_CHAR_NUM);
            }

            // Randomly assign extra XP.
            AssignRemainingXPRandomly(newChar, speciesChar, isDroid ? DROID_ALT_CHAR_NUM : MAX_CHAR_NUM);

            newChar.SetCharacteristics(speciesChar);

            return newChar;
        }

        private int DroidPref(int offset)
        {
            int adjustment = MAX_CHAR_NUM - offset;
            return adjustment >= DROID_ALT_CHAR_NUM ? adjustment : DROID_ALT_CHAR_NUM;
        }

        private void AssignRemainingXPRandomly(Characteristic newChar, Dictionary<string, int> speciesChar, int maxCharNum)
        {
            List<string> charNames = new List<string>();

            int charNumberToLookFor = (availableXP / 10) - 1;

            while (charNumberToLookFor > 0)
            {
                foreach (string charName in newChar.GetNames())
                {
                    int adjustedCharNumToLookFor = charNumberToLookFor >= maxCharNum ? (maxCharNum - 1) : charNumberToLookFor;
                    if (speciesChar[charName] == adjustedCharNumToLookFor)
                    {
                        charNames.Add(charName);
                    }
                }

                bool DoAvailableCharsExist = charNames.Count > 0;
                if (DoAvailableCharsExist)
                {
                    string randomCharName = Randomizer.Instance.GetRandomItem(charNames);

                    charNames.Clear();

                    int charNumber = speciesChar[randomCharName];

                    speciesChar[randomCharName] = CalcIndividualChar(charNumber, maxCharNum);

                    charNumberToLookFor = (availableXP / 10) - 1;
                }
                else
                {
                    charNumberToLookFor += -1;
                }
            }
        }

        private int CalcIndividualChar(int charPrefNum, int MAX_CHAR_NUM)
        {
            while (charPrefNum != MAX_CHAR_NUM)
            {
                bool isAllowed = (availableXP - ((charPrefNum + 1) * 10)) >= 0;

                if (isAllowed)
                {
                    charPrefNum += 1;
                    availableXP -= (charPrefNum * 10);
                }
                else
                    break;
            }
            return charPrefNum;
        }

        public string Print()
        {
            string output = "";

            string speciesName = species.Name;
            string careerName = career.Name;
            string specName = spec.Name;
            string sAvailableXP = availableXP.ToString();

            Dictionary<string, int> characteristics = newCharateristic.GetCharacteristics();
            List<string> charNameList = newCharateristic.GetNames();

            output += "Species..............: " + speciesName + "\n";
            output += "Career...............: " + careerName + "\n";
            output += "Specilization........: " + specName + "\n";

            //if (obligationXP > 0)
            //{
            output += "XP from Obligation...: " + obligationXP.ToString() + "\n";
            //}

            output += "Remaining XP.........: " + sAvailableXP + "\n";
            output += "\n";

            foreach (string charName in charNameList)
            {
                output += charName.PadRight(12);
            }

            output += "\n";

            foreach (string charName in charNameList)
            {
                output += characteristics[charName].ToString().PadRight(12);
            }

            output += "\n";
            output += "\n";

            return output;
        }
    }
}
