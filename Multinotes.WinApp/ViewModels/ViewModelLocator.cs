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
            if (!DesignMode)
                _synchronizationService.Initialize();
            else
                _synchronizationService.InitializeDesignMode();
            _selection = new MessageBoardSelectionModel();
            _selection.SelectedShare = _synchronizationService.Individual.Shares
                .FirstOrDefault();
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
