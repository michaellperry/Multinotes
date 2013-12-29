using System.ComponentModel;
using Multinotes.PhoneApp.Models;
using UpdateControls.XAML;

namespace Multinotes.PhoneApp.ViewModels
{
    public class ViewModelLocator
    {
        private readonly SynchronizationService _synchronizationService;

        private readonly MessageBoardSelectionModel _selection;
        private readonly MainViewModel _main;
        private readonly JoinMessageBoardViewModel _join;

        public ViewModelLocator()
        {
            _synchronizationService = new SynchronizationService();
            if (!DesignerProperties.IsInDesignTool)
                _synchronizationService.Initialize();
            _selection = new MessageBoardSelectionModel();
            _main = new MainViewModel(_synchronizationService.Individual, _synchronizationService, _selection);
            _join = new JoinMessageBoardViewModel(_selection, _synchronizationService.Individual);
        }

        public object Main
        {
            get { return ForView.Wrap(_main); }
        }

        public object Join
        {
            get { return ForView.Wrap(_join); }
        }
    }
}
