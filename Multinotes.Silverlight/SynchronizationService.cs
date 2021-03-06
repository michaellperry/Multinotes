using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.IsolatedStorage;
using UpdateControls.Correspondence.BinaryHTTPClient;
using UpdateControls.Correspondence.Memory;

namespace Multinotes.Silverlight
{
    public class SynchronizationService
    {
        private const string ThisIndividual = "Multinotes.Silverlight.Individual.this";
        private static readonly Regex Punctuation = new Regex(@"[{}\-]");

        private Community _community;
        private Individual _individual;

        public void Initialize()
        {
            var storage = IsolatedStorageStorageStrategy.Load();
            var http = new HTTPConfigurationProvider();
            var communication = new BinaryHTTPAsynchronousCommunicationStrategy(http);

            _community = new Community(storage);
            _community.AddAsynchronousCommunicationStrategy(communication);
            _community.Register<CorrespondenceModel>();
            _community.Subscribe(() => _individual);

            CreateIndividual();
			
            // Synchronize whenever the user has something to send.
            _community.FactAdded += delegate
            {
                _community.BeginSending();
            };

            // Periodically resume if there is an error.
            DispatcherTimer synchronizeTimer = new DispatcherTimer();
            synchronizeTimer.Tick += delegate
            {
                _community.BeginSending();
                _community.BeginReceiving();
            };
            synchronizeTimer.Interval = TimeSpan.FromSeconds(60.0);
            synchronizeTimer.Start();

            // And synchronize on startup.
            _community.BeginSending();
            _community.BeginReceiving();
        }

        public void InitializeDesignMode()
        {
            _community = new Community(new MemoryStorageStrategy());
            _community.Register<CorrespondenceModel>();

            CreateIndidualDesignData();
        }

        public Community Community
        {
            get { return _community; }
        }

        public Individual Individual
        {
            get { return _individual; }
        }

        private void CreateIndividual()
        {
            _individual = _community.LoadFact<Individual>(ThisIndividual);
            if (_individual == null)
            {
                string randomId = Punctuation.Replace(Guid.NewGuid().ToString(), String.Empty).ToLower();
                _individual = _community.AddFact(new Individual(randomId));
                _community.SetFact(ThisIndividual, _individual);
            }
        }

        private void CreateIndidualDesignData()
        {
            var individual = _community.AddFact(new Individual("design"));
            _individual = individual;
        }
    }
}
