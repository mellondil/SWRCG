using System;
using System.Collections.Generic;
using System.Text;

namespace SWRCG
{
    public class Randomizer
    {
        #region Singleton Code
        private static readonly Randomizer instance = new Randomizer();
        static Randomizer() { }
        private Randomizer()
        {
        }
        public static Randomizer Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        private static readonly Random getRandom = new Random();

        private static int GetRandomNumber(int min, int max)
        {
            lock (getRandom)
            {
                return getRandom.Next(min, max);
            }
        }

        public Species GetRandomItem(List<Species> itemsList)
        {
            int result = GetRandomNumber(0, itemsList.Count);

            return itemsList[result];
        }

        public Career GetRandomItem(List<Career> itemsList)
        {
            int result = GetRandomNumber(0, itemsList.Count);

            return itemsList[result];
        }

        public Spec GetRandomItem(List<Spec> itemsList)
        {
            int result = GetRandomNumber(0, itemsList.Count);

            return itemsList[result];
        }

        public string GetRandomItem(List<string> itemsList)
        {
            int result = GetRandomNumber(0, itemsList.Count);

            return itemsList[result];
        }

        public int GetRandomXP()
        {
            int result = GetRandomNumber(0, 4);
            return result * 5;
        }

        public int GetOneOrZero()
        {
            return GetRandomNumber(0, 2);
        }
    }
}
