using System.ComponentModel;
using System.Linq;
using Multinotes.WinApp.Models;
using UpdateControls.XAML;

namespace Multinotes.WinApp.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        private readonly SynchronizationService _synchronizationService;

        private readonly MessageBoardSelectionModel _selection;

        public ViewModelLocator()
        {
            _synchronizationService = new SynchronizationService();
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _synchronizationService.Initialize();
            _selection = new MessageBoardSelectionModel();
            if (_synchronizationService.Individual != null)
                _selection.SelectedShare = _synchronizationService.Individual.Shares
                    .FirstOrDefault();
        }

        public object Main
        {
            get
            {
                return ViewModel(() => _synchronizationService.Individual == null
                    ? null
                    : new MainViewModel(_synchronizationService.Individual, _synchronizationService, _selection));
            }
        }

        public object Join
        {
            get
            {
                return ViewModel(() =>
                    _synchronizationService.Individual == null
                    ? null
                    : new JoinMessageBoardViewModel(_selection, _synchronizationService.Individual));
            }
        }
    }
}
