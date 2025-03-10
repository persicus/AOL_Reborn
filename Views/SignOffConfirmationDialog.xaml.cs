using System.Windows;

namespace AOL_Reborn.Views
{
    public partial class SignOffConfirmationDialog : Window
    {
        public bool DoNotAskAgain { get; private set; }

        public SignOffConfirmationDialog()
        {
            InitializeComponent();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            DoNotAskAgain = DoNotAskAgainCheckBox.IsChecked == true;
            DialogResult = true;
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}