using System.Windows;
using AOL_Reborn.Properties;
using WpfMessageBox = System.Windows.MessageBox;

namespace AOL_Reborn.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            // Load current network settings into the textboxes
            ServerIpTextBox.Text = Settings.Default.ServerIp;
            ReceivePortTextBox.Text = Settings.Default.ReceivePort.ToString();
            SendPortTextBox.Text = Settings.Default.SendPort.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate and save settings
            Settings.Default.ServerIp = ServerIpTextBox.Text.Trim();

            if (int.TryParse(ReceivePortTextBox.Text, out int receivePort))
            {
                Settings.Default.ReceivePort = receivePort;
            }
            else
            {
                WpfMessageBox.Show("Invalid Receive Port.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (int.TryParse(SendPortTextBox.Text, out int sendPort))
            {
                Settings.Default.SendPort = sendPort;
            }
            else
            {
                WpfMessageBox.Show("Invalid Send Port.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Settings.Default.Save();
            WpfMessageBox.Show("Network settings saved.", "Settings", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}
