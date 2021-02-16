using System;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace SWRCG
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void isThereObligationCheckBox_Click(object sender, RoutedEventArgs e)
        {
            SettingsManager.Instance.IsThereOblication = (bool)this.isThereObligationCheckBox.IsChecked;
        }

        private void isForceCareersEnabledCheckBox_Click(object sender, RoutedEventArgs e)
        {
            SettingsManager.Instance.IsForceCareersEnabled = (bool)this.isForceCareersEnabledCheckBox.IsChecked;
        }

        private void chooseDataPathButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                Description = "Select the data directory where the folder for Species, Careers, and Specializations are held.",
                ShowNewFolderButton = false
            };

            // Find an initial path to show the user.
            if (string.IsNullOrWhiteSpace(SettingsManager.Instance.GetDataFolderPath()))
            {
                folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            }
            else
            {
                folderBrowserDialog.SelectedPath = SettingsManager.Instance.GetDataFolderPath();
            }

            // Show folder browser dialog.
            if(folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string selectedPath = folderBrowserDialog.SelectedPath;

                try
                {
                    if (SettingsManager.Instance.IsDataPathGood(selectedPath))
                    {
                        SettingsManager.Instance.UpdateData(selectedPath);
                        dataFilePathTextBox.Text = selectedPath;
                        isDataLoadedStatus.IsChecked = SettingsManager.Instance.IsDataLoaded();
                    }
                    else
                    {
                        //isDataLoadedStatus.IsChecked = false;
                        MessageBox.Show("The selected directory does not have the required sub-folders or data.\n\n" +
                                        "Please choose a directory containing Species, Careers, and Specializations sub-folders, " +
                                        "with \".xml\" data files.\n\n" +
                                        "The previous data path has not changed.",
                                        "Data Issue");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Unexpected Error");
                }
            }
        }

        private void okayButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
