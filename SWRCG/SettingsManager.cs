using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SWRCG
{
    public sealed class SettingsManager
    {
        #region Singleton Code
        private static readonly SettingsManager instance = new SettingsManager();
        static SettingsManager() { }
        private SettingsManager()
        {
        }
        public static SettingsManager Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        private bool isThereObligation = true;
        private bool isForceCareersEnabled = true;

        private readonly string SWCharGen_Full_Data_Default_Path_Segment = "\\SWCharGen\\DataCustom\\DataSet_Full_DataSet";

        private string dataFolderPath = "";

        public bool IsThereOblication
        {
            get => isThereObligation;
            set => isThereObligation = value;
        }

        public bool IsForceCareersEnabled
        {
            get => isForceCareersEnabled;
            set => isForceCareersEnabled = value;
        }

        public string GetDataFolderPath()
        {
            return dataFolderPath;
        }

        private void SetDataFolderPath(string path)
        {
            dataFolderPath = path;
        }

        public bool TryDefaultDataPath()
        {
            string potentialDefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + SWCharGen_Full_Data_Default_Path_Segment;

            if (IsDataPathGood(potentialDefaultPath))
            {
                dataFolderPath = potentialDefaultPath;
                return true;
            }

            return false;
        }

        private bool CheckForSpeciesCareersSpecsFolders(string path)
        {
            bool isThereSpeciesFolder = Directory.Exists(path + DataManager.Instance.GetSpeciesFolderName());
            bool isThereCareersFolder = Directory.Exists(path + DataManager.Instance.GetCareersFolderName());
            bool isThereSpecsFolder = Directory.Exists(path + DataManager.Instance.GetSpecsFolderName());

            return isThereSpeciesFolder && isThereCareersFolder && isThereSpecsFolder;
        }

        public bool IsDataPathGood(string path)
        {
            if (CheckForSpeciesCareersSpecsFolders(path))
            {
                int speciesFileCount = Directory.EnumerateFiles(path + DataManager.Instance.GetSpeciesFolderName(), "*.xml").Count();

                int careersFileCount = Directory.EnumerateFiles(path + DataManager.Instance.GetCareersFolderName(), "*.xml").Count();

                int specsFileCount = Directory.EnumerateFiles(path + DataManager.Instance.GetSpecsFolderName(), "*.xml").Count();

                if (speciesFileCount > 0 && careersFileCount > 0 && specsFileCount > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public void UpdateData(string path)
        {
            if (!IsDataPathGood(path))
            {
                return;
            }

            try
            {
                DataManager.Instance.LoadData(path);

                SetDataFolderPath(path);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e.Message);
            }
        }

        public bool IsDataLoaded()
        {
            return DataManager.Instance.IsDataLoaded();
        }
    }
}
