using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SWRCG
{
    public sealed class DataManager
    {
        #region Singleton Code
        private static readonly DataManager instance = new DataManager();
        static DataManager() { }
        private DataManager()
        {
        }
        public static DataManager Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        private List<Species> speciesList = new List<Species>();
        private List<Career> careersList = new List<Career>();
        private List<Spec> specsList = new List<Spec>();

        private string DATA_DIRECTORY;
        private readonly string SPECIES_FOLDER_NAME = "\\Species";
        private readonly string CAREERS_FOLDER_NAME = "\\Careers";
        private readonly string SPECIALIZATION_FOLDER_NAME = "\\Specializations";

        private bool isDataLoaded = false;

        // Testing.
        private List<Species> otherSpeciesList = new List<Species>();

        public void LoadData(string Data_Directory)
        {
            DATA_DIRECTORY = Data_Directory;

            LoadSpecies();
            LoadCareers();
            LoadSpecs();
            UpdateCareerSpecConnection();

            LoadSpecPreferences();

            if (speciesList.Count > 0 && careersList.Count > 0 && specsList.Count > 0)
            {
                isDataLoaded = true;
            }
            else
            {
                isDataLoaded = false;
            }

            //PrintSpecKeys();
            //PrintSpeciesChar();
        }

        // For testing.
        private void PrintSpecKeys()
        {
            foreach (var spec in specsList)
            {
                Debug.WriteLine(spec.Key);
            }
        }

        // For testing.
        private void PrintSpeciesChar()
        {
            #region Get other species.
            try
            {
                string speciesPath = @"C:\Users\Jonathan\Google Drive\RPG\Star Wars\SWCharGen\Data\Species";

                var speciesFiles = from file in Directory.EnumerateFiles(speciesPath, "*.xml")
                                   select file;

                foreach (var f in speciesFiles)
                {
                    XElement element = XElement.Load(f);

                    Species species = new Species();

                    species.Key = element.Element("Key").Value;

                    species.Name = element.Element("Name").Value;

                    species.StartingXP = int.Parse(element.Element("StartingAttrs").Element("Experience").Value);

                    if (species.Key == "GAND")
                        species.StartingXP += 10;

                    Characteristic c = new Characteristic();

                    var StartingChar = element.Element("StartingChars").Elements();

                    foreach (var sc in StartingChar)
                    {
                        switch (sc.Name.LocalName)
                        {
                            case "Brawn":
                                c.BRAWN = int.Parse(sc.Value);
                                break;
                            case "Agility":
                                c.AGILITY = int.Parse(sc.Value);
                                break;
                            case "Intellect":
                                c.INTELLECT = int.Parse(sc.Value);
                                break;
                            case "Cunning":
                                c.CUNNING = int.Parse(sc.Value);
                                break;
                            case "Willpower":
                                c.WILLPOWER = int.Parse(sc.Value);
                                break;
                            case "Presence":
                                c.PRESENCE = int.Parse(sc.Value);
                                break;
                            default:
                                break;
                        }
                    }

                    species.SetCharacteristic(c);

                    otherSpeciesList.Add(species);
                }
            }
            catch (UnauthorizedAccessException uAEx)
            {
                Console.WriteLine(uAEx.Message);
            }
            catch (PathTooLongException pathEx)
            {
                Console.WriteLine(pathEx.Message);
            }
            #endregion

            foreach (var species in speciesList)
            {
                var otherSpecies = otherSpeciesList.Find( s => s.Key == species.Key);

                Characteristic sChar = species.GetCharacteristic();

                Debug.WriteLine(species.Name + ": " + sChar.BRAWN + " " + sChar.AGILITY + " " + sChar.INTELLECT + " " + sChar.CUNNING + " " + sChar.WILLPOWER + " " + sChar.PRESENCE + " | XP: " + species.StartingXP);

                if (otherSpecies != null)
                {
                    Characteristic osChar = otherSpecies.GetCharacteristic();

                    Debug.WriteLine(otherSpecies.Name + ": " + osChar.BRAWN + " " + osChar.AGILITY + " " + osChar.INTELLECT + " " + osChar.CUNNING + " " + osChar.WILLPOWER + " " + osChar.PRESENCE + " | XP: " + otherSpecies.StartingXP);
                }

                Debug.WriteLine("");
            }
        }

        public Species GetRandomSpecies()
        {
            return Randomizer.Instance.GetRandomItem(speciesList);
        }

        public Spec GetRandomSpec(Species species, bool isForceCareersEnabled)
        {
            if (species.Key == "DROID" || !isForceCareersEnabled)
            {
                List<Spec> nonForceSpecs = new List<Spec>();

                foreach (Spec s in specsList)
                {
                    if (!s.GetCareers().FirstOrDefault().HasForce)
                    {
                        nonForceSpecs.Add(s);
                    }
                }

                return Randomizer.Instance.GetRandomItem(nonForceSpecs);
            }

            return Randomizer.Instance.GetRandomItem(specsList);
        }

        public Career GetRandomCareer(Spec spec)
        {
            return Randomizer.Instance.GetRandomItem(spec.GetCareers());
        }

        public string GetSpeciesFolderName()
        {
            return SPECIES_FOLDER_NAME;
        }

        public string GetCareersFolderName()
        {
            return CAREERS_FOLDER_NAME;
        }

        public string GetSpecsFolderName()
        {
            return SPECIALIZATION_FOLDER_NAME;
        }

        public bool IsDataLoaded()
        {
            return isDataLoaded;
        }

        private void LoadSpecies()
        {
            try
            {
                string speciesPath = GetSpeciesPath();

                var speciesFiles = from file in Directory.EnumerateFiles(speciesPath, "*.xml")
                                   select file;

                // Clear list to load new data.
                speciesList.Clear();

                foreach (var f in speciesFiles)
                {
                    XElement element = XElement.Load(f);

                    Species species = new Species
                    {
                        Key = element.Element("Key").Value,

                        Name = element.Element("Name").Value,

                        StartingXP = int.Parse(element.Element("StartingAttrs").Element("Experience").Value)
                    };

                    if (species.Key == "GAND")
                        species.StartingXP += 10;

                    Characteristic c = new Characteristic();

                    var StartingChar = element.Element("StartingChars").Elements();

                    foreach (var sc in StartingChar)
                    {
                        switch (sc.Name.LocalName)
                        {
                            case "Brawn":
                                c.BRAWN = int.Parse(sc.Value);
                                break;
                            case "Agility":
                                c.AGILITY = int.Parse(sc.Value);
                                break;
                            case "Intellect":
                                c.INTELLECT = int.Parse(sc.Value);
                                break;
                            case "Cunning":
                                c.CUNNING = int.Parse(sc.Value);
                                break;
                            case "Willpower":
                                c.WILLPOWER = int.Parse(sc.Value);
                                break;
                            case "Presence":
                                c.PRESENCE = int.Parse(sc.Value);
                                break;
                            default:
                                break;
                        }
                    }

                    species.SetCharacteristic(c);

                    speciesList.Add(species);
                }
            }
            catch (UnauthorizedAccessException uAEx)
            {
                Console.WriteLine(uAEx.Message);
            }
            catch (PathTooLongException pathEx)
            {
                Console.WriteLine(pathEx.Message);
            }
        }

        private void LoadCareers()
        {
            try
            {
                string careerPath = GetCareerPath();

                var careerFiles = from file in Directory.EnumerateFiles(careerPath, "*.xml")
                                   select file;

                // Clear list to loaded new data.
                careersList.Clear();

                foreach (var f in careerFiles)
                {
                    XElement element = XElement.Load(f);

                    Career career = new Career
                    {
                        Name = element.Element("Name").Value
                    };

                    var specializations = element.Element("Specializations").Elements();

                    foreach (var s in specializations)
                    {
                        if (s.Value != "JEDIKNIGHT" && s.Value != "JEDIGEN" && s.Value != "JEDIMASTER")
                            career.SetSpec(s.Value);

                        // Addressing issue with some datasets. Converting "STEELHANDADEPT" to "STEELHAND" to aline with Spec.
                        if (s.Value == "STEELHANDADEPT")
                        {
                            career.ReplaceKey("STEELHANDADEPT", "STEELHAND");
                        }

                    }

                    // Add force rating to career.
                    var forceRatingAttribute = element.Elements("Attributes");
                    if (forceRatingAttribute.Count() > 0)
                    {
                        var forceRatingElement = forceRatingAttribute.Elements("ForceRating");

                        if (forceRatingElement.Count() > 0)
                        {
                            career.HasForce = int.Parse(forceRatingElement.FirstOrDefault().Value) == 1;
                        }
                    }

                    careersList.Add(career);
                }
            }
            catch (UnauthorizedAccessException uAEx)
            {
                Console.WriteLine(uAEx.Message);
            }
            catch (PathTooLongException pathEx)
            {
                Console.WriteLine(pathEx.Message);
            }
        }

        private void LoadSpecs()
        {
            try
            {
                string specsPath = GetSpecPath();

                var specFiles = from file in Directory.EnumerateFiles(specsPath, "*.xml")
                                  select file;

                // Clear list to loaded new data.
                specsList.Clear();

                foreach (var f in specFiles)
                {
                    XElement element = XElement.Load(f);

                    Spec spec = new Spec
                    {
                        Key = element.Element("Key").Value,

                        Name = element.Element("Name").Value
                    };

                    // Addressing issue with some datasets. Converting "STEELHANDADEPT" to "STEELHAND" to aline with career.
                    if (spec.Key == "STEELHANDADEPT")
                    {
                        spec.Key = "STEELHAND";
                    }

                    if (spec.Key != "JEDIKNIGHT" && spec.Key != "JEDIGEN" && spec.Key != "JEDIMASTER")
                        specsList.Add(spec);
                }
            }
            catch (UnauthorizedAccessException uAEx)
            {
                Console.WriteLine(uAEx.Message);
            }
            catch (PathTooLongException pathEx)
            {
                Console.WriteLine(pathEx.Message);
            }
        }

        private void LoadSpecPreferences()
        {
            try
            {
                string specPrefPath = GetSpecPrefPath();

                var specPrefFiles = from file in Directory.EnumerateFiles(specPrefPath, "*.csv")
                                    select file;

                string CharPrefCSVFilePath = specPrefFiles.First().ToString();

                using (TextFieldParser csvParser = new TextFieldParser(CharPrefCSVFilePath))
                {
                    csvParser.SetDelimiters(new string[] { "," });

                    while (!csvParser.EndOfData)
                    {
                        string[] fields = csvParser.ReadFields();

                        Spec spec = specsList.Find(s => s.Key == fields[0]);

                        if (spec != null)
                        {
                            Characteristic charPref = spec.GetCharPref();

                            for (int i = 1; i < fields.Length; i++)
                            {
                                switch (fields[i])
                                {
                                    case "Brawn":
                                        charPref.BRAWN = i;
                                        break;
                                    case "Agility":
                                        charPref.AGILITY = i;
                                        break;
                                    case "Intellect":
                                        charPref.INTELLECT = i;
                                        break;
                                    case "Cunning":
                                        charPref.CUNNING = i;
                                        break;
                                    case "Willpower":
                                        charPref.WILLPOWER = i;
                                        break;
                                    case "Presence":
                                        charPref.PRESENCE = i;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException uAEx)
            {
                Console.WriteLine(uAEx.Message);
            }
            catch (PathTooLongException pathEx)
            {
                Console.WriteLine(pathEx.Message);
            }
        }

        private void UpdateCareerSpecConnection()
        {
            if (careersList.Count > 0 && specsList.Count > 0)
            {
                try
                {
                    foreach (var career in careersList.ToList())
                    {
                        var keys = career.GetSpecKeys();

                        foreach (var key in keys.ToList())
                        {
                            var specsFound = from s in specsList.ToList()
                                             where s.Key == key
                                             select s;

                            career.SetSpec(key, specsFound.First());

                            specsFound.First().AddCareer(career);
                        }
                    }

                    // Remove universal specs not in a career.
                    var universalSpecs = from s in specsList.ToList()
                                         where s.GetCareers().Count() == 0
                                         select s;

                    for (int i = 0; i < universalSpecs.Count(); i++)
                    {
                        specsList.Remove(universalSpecs.ElementAt(i));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private string GetSpeciesPath()
        {
            return DATA_DIRECTORY + SPECIES_FOLDER_NAME;
        }

        private string GetCareerPath()
        {
            return DATA_DIRECTORY + CAREERS_FOLDER_NAME;
        }

        private string GetSpecPath()
        {
            return DATA_DIRECTORY + SPECIALIZATION_FOLDER_NAME;
        }

        private string GetSpecPrefPath()
        {
            return Environment.CurrentDirectory;
        }
    }
}
