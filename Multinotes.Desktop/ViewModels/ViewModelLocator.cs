using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using UpdateControls.XAML;
using Multinotes.Desktop.Models;

namespace Multinotes.Desktop.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        private readonly SynchronizationService _synchronizationService;
        private MessageBoardSelectionModel _selection;

        public ViewModelLocator()
        {
            _synchronizationService = new SynchronizationService();
            if (!DesignMode)
                _synchronizationService.Initialize();
            else
                _synchronizationService.InitializeDesignMode();
            _selection = new Models.MessageBoardSelectionModel();
            _selection.SelectedShare = _synchronizationService.Individual.Shares
                .FirstOrDefault();
        }

        public object Main
        {
            get
            {
                return ViewModel(() => new MainViewModel(
                    _synchronizationService.Community,
                    _synchronizationService.Individual,
                    _selection));
            }
        }
    }
}
