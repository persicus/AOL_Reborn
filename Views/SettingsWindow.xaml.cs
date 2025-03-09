using System.Windows;
using System.Windows.Controls;

namespace AOL_Reborn.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            IpAddressTextBox.Text = Properties.Settings.Default.ServerIp;
            ReceivePortTextBox.Text = Properties.Settings.Default.ReceivePort.ToString();
            SendPortTextBox.Text = Properties.Settings.Default.SendPort.ToString();
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default["ServerIp"] = IpAddressTextBox.Text;
            Properties.Settings.Default["ReceivePort"] = int.Parse(ReceivePortTextBox.Text);
            Properties.Settings.Default["SendPort"] = int.Parse(SendPortTextBox.Text);
            Properties.Settings.Default.Save();

            // Restart the network service with new settings
            if (Application.Current.MainWindow is BuddyListWindow mainWindow)
            {
                mainWindow.RestartNetworkService();
            }

            this.Close();
        }



        private void CancelSettings_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
