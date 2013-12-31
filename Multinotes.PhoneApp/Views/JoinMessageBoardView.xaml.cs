using System;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Multinotes.PhoneApp.ViewModels;
using UpdateControls.XAML;
using System.Windows.Controls;

namespace Multinotes.PhoneApp.Views
{
    public partial class JoinMessageBoardView : PhoneApplicationPage
    {
        public JoinMessageBoardView()
        {
            InitializeComponent();

            ForView.BindAppBarItem(
                ApplicationBar.Buttons[0],
                ForView.Unwrap<JoinMessageBoardViewModel>(DataContext).Join);
        }

        private void OK_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            JoinMessageBoardViewModel viewModel = ForView.Unwrap<JoinMessageBoardViewModel>(DataContext);
            if (viewModel != null)
                viewModel.Topic = null;
            NavigationService.GoBack();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }
    }
}