using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SWRCG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (SettingsManager.Instance.TryDefaultDataPath())
            {
                SettingsManager.Instance.UpdateData(SettingsManager.Instance.GetDataFolderPath());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!SettingsManager.Instance.IsDataLoaded())
            {
                MessageBox.Show("Please load data using Settings.", "Data Issue");
                return;
            }

            Species species = DataManager.Instance.GetRandomSpecies();
            Spec spec = DataManager.Instance.GetRandomSpec(species, SettingsManager.Instance.IsForceCareersEnabled);
            Career career = DataManager.Instance.GetRandomCareer(spec);
            int obligationXP = 0;

            if (SettingsManager.Instance.IsThereOblication)
            {
                obligationXP = Randomizer.Instance.GetRandomXP();
            }

            RandomCharacter randomCharacter = new RandomCharacter(species, career, spec, obligationXP);

            string output = randomCharacter.Print();

            listBox.Items.Add(output);
            listBox.ScrollIntoView(listBox.Items.GetItemAt(listBox.Items.Count - 1));
            listBox.SelectedItem = listBox.Items.GetItemAt(listBox.Items.Count - 1);
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Owner = this;
            settingsWindow.dataFilePathTextBox.Text = SettingsManager.Instance.GetDataFolderPath();
            settingsWindow.isThereObligationCheckBox.IsChecked = SettingsManager.Instance.IsThereOblication;
            settingsWindow.isForceCareersEnabledCheckBox.IsChecked = SettingsManager.Instance.IsForceCareersEnabled;
            settingsWindow.isDataLoadedStatus.IsChecked = SettingsManager.Instance.IsDataLoaded();

            settingsWindow.ShowDialog();
        }
    }
}
