using System;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Multinotes.PhoneApp.ViewModels;
using UpdateControls.XAML;

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
    }
}