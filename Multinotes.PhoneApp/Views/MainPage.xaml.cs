using System;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Multinotes.PhoneApp.ViewModels;
using UpdateControls.XAML;
using System.Windows;
using Multinotes.Model;
using System.Windows.Input;
using System.Windows.Data;

namespace Multinotes.PhoneApp.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        private static DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source",
            typeof(object),
            typeof(MainPage),
            new PropertyMetadata(new PropertyChangedCallback(SourceChanged)));
        private static DependencyProperty SendMessageCommandProperty = DependencyProperty.Register(
            "SendMessageCommand",
            typeof(ICommand),
            typeof(MainPage),
            new PropertyMetadata(new PropertyChangedCallback(SendMessageCommandChanged)));
        private static DependencyProperty LeaveBoardCommandProperty = DependencyProperty.Register(
            "LeaveBoardCommand",
            typeof(ICommand),
            typeof(MainPage),
            new PropertyMetadata(new PropertyChangedCallback(LeaveBoardCommandChanged)));

        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetBinding(SendMessageCommandProperty, new Binding("SendMessage"));
            this.SetBinding(LeaveBoardCommandProperty, new Binding("LeaveBoard"));
        }

        public object Source
        {
            get { return GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public ICommand SendMessageCommand
        {
            get { return (ICommand)GetValue(SendMessageCommandProperty); }
            set { SetValue(SendMessageCommandProperty, value); }
        }

        public ICommand LeaveBoardCommand
        {
            get { return (ICommand)GetValue(LeaveBoardCommandProperty); }
            set { SetValue(LeaveBoardCommandProperty, value); }
        }

        private static void SourceChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            ((MainPage)d).OnSourceChanged(
                e.OldValue,
                e.NewValue);
        }

        private static void SendMessageCommandChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            ((MainPage)d).OnSendMessageCommandChanged(
                (ICommand)e.OldValue,
                (ICommand)e.NewValue);
        }

        private static void LeaveBoardCommandChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            ((MainPage)d).OnLeaveBoardCommandChanged(
                (ICommand)e.OldValue,
                (ICommand)e.NewValue);
        }

        private void OnSourceChanged(object oldValue, object newValue)
        {
            var oldViewModel = ForView.Unwrap<MainViewModel>(oldValue);
            if (oldViewModel != null)
                oldViewModel.ConfirmLeaveBoard -= ConfirmLeaveBoard;

            var newViewModel = ForView.Unwrap<MainViewModel>(newValue);
            if (newViewModel != null)
                newViewModel.ConfirmLeaveBoard += ConfirmLeaveBoard;
        }

        public void OnSendMessageCommandChanged(ICommand oldValue, ICommand newValue)
        {
            if (oldValue != null)
                oldValue.CanExecuteChanged -= SendMessageCommand_CanExecuteChanged;
            if (newValue != null)
                newValue.CanExecuteChanged += SendMessageCommand_CanExecuteChanged;
        }

        public void OnLeaveBoardCommandChanged(ICommand oldValue, ICommand newValue)
        {
            if (oldValue != null)
                oldValue.CanExecuteChanged -= LeaveBoardCommand_CanExecuteChanged;
            if (newValue != null)
                newValue.CanExecuteChanged += LeaveBoardCommand_CanExecuteChanged;
        }

        void SendMessageCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            var sendMessageButton = ApplicationBar.Buttons[0] as IApplicationBarMenuItem;
            if (sendMessageButton != null && SendMessageCommand != null)
                sendMessageButton.IsEnabled = SendMessageCommand.CanExecute(null);
        }

        void LeaveBoardCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            var leaveBoardButton = ApplicationBar.Buttons[2] as IApplicationBarMenuItem;
            if (leaveBoardButton != null && LeaveBoardCommand != null)
                leaveBoardButton.IsEnabled = LeaveBoardCommand.CanExecute(null);
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

        private void Send_Click(object sender, EventArgs e)
        {
            if (SendMessageCommand != null)
            {
                SendMessageCommand.Execute(null);
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/JoinMessageBoardView.xaml", UriKind.Relative));
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (LeaveBoardCommand != null)
            {
                LeaveBoardCommand.Execute(null);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }
    }
}