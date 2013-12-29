using System;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Multinotes.PhoneApp.ViewModels;
using UpdateControls.XAML;
using System.Windows;
using Multinotes.Model;

namespace Multinotes.PhoneApp.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            var viewModel = ForView.Unwrap<MainViewModel>(DataContext);
            ForView.BindAppBarItem(
                ApplicationBar.Buttons[0],
                viewModel.SendMessage);
            ForView.BindAppBarItem(
                ApplicationBar.Buttons[2],
                viewModel.LeaveBoard);

            viewModel.ConfirmLeaveBoard += ConfirmLeaveBoard;
        }

        bool ConfirmLeaveBoard(MessageBoard messageBoard)
        {
            return MessageBox.Show(
                String.Format("Leave the group {0}?", messageBoard.Topic),
                "Leave group",
                MessageBoxButton.OKCancel) == MessageBoxResult.OK;
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var viewModel = ForView.Unwrap<MainViewModel>(DataContext);
                var messageBoard = ForView.Unwrap<MessageBoardViewModel>(e.AddedItems[0]);
                if (viewModel != null && messageBoard != null)
                {
                    viewModel.SetSelectedMessageBoard(messageBoard);
                }
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/JoinMessageBoardView.xaml", UriKind.Relative));
        }

        private void Delete_Click(object sender, EventArgs e)
        {

        }
    }
}