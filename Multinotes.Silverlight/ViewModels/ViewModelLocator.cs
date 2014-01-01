using System;
using System.ComponentModel;
using System.Linq;
using UpdateControls.XAML;

namespace Multinotes.Silverlight.ViewModels
{
    public class ViewModelLocator : ViewModelLocatorBase
    {
        private readonly SynchronizationService _synchronizationService;

        public ViewModelLocator()
        {
            _synchronizationService = new SynchronizationService();
            if (DesignMode)
                _synchronizationService.Initialize();
            else
                _synchronizationService.InitializeDesignMode();
        }

        public object Main
        {
            get
            {
                return ViewModel(() => _synchronizationService.Individual == null
                    ? null :
                    new MainViewModel(
                        _synchronizationService.Community,
                        _synchronizationService.Individual));
            }
        }
    }
}
