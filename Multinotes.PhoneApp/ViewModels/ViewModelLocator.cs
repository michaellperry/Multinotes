using System.ComponentModel;
using Multinotes.PhoneApp.Models;
using UpdateControls.XAML;

namespace Multinotes.PhoneApp.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        private readonly SynchronizationService _synchronizationService;

        private readonly MessageBoardSelectionModel _selection;

        public ViewModelLocator()
        {
            _synchronizationService = new SynchronizationService();
            if (!DesignerProperties.IsInDesignTool)
                _synchronizationService.Initialize();
            _selection = new MessageBoardSelectionModel();
        }

        public object Main
        {
            get
            {
                return ViewModel(() => new MainViewModel(
                    _synchronizationService.Individual,
                    _synchronizationService,
                    _selection));
            }
        }

        public object Join
        {
            get
            {
                return ViewModel(() => new JoinMessageBoardViewModel(
                    _selection,
                    _synchronizationService.Individual));
            }
        }
    }
}
