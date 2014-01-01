using Multinotes.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Threading;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.BinaryHTTPClient;
using UpdateControls.Correspondence.Memory;
using UpdateControls.Correspondence.SSCE;
using UpdateControls.Fields;

namespace Multinotes.Desktop
{
    public class SynchronizationService
    {
        private const string ThisIndividual = "Multinotes.Desktop.Individual.this";
        private static readonly Regex Punctuation = new Regex(@"[{}\-]");

        private Community _community;
        private Independent<Individual> _individual = new Independent<Individual>(
            Individual.GetNullInstance());

        public void Initialize()
        {
            string correspondenceDatabase = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CorrespondenceApp", "Multinotes.Desktop", "Correspondence.sdf");
            var storage = new SSCEStorageStrategy(correspondenceDatabase);
            var http = new HTTPConfigurationProvider();
            var communication = new BinaryHTTPAsynchronousCommunicationStrategy(http);

            _community = new Community(storage);
            _community.AddAsynchronousCommunicationStrategy(communication);
            _community.Register<CorrespondenceModel>();
            _community.Subscribe(() => Individual);
            _community.Subscribe(() => Individual.MessageBoards);

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

            CreateIndividualDesignData();
        }

        public Community Community
        {
            get { return _community; }
        }

        public Individual Individual
        {
            get
            {
                lock (this)
                {
                    return _individual;
                }
            }
            private set
            {
                lock (this)
                {
                    _individual.Value = value;
                }
            }
        }

        private async void CreateIndividual()
        {
            var individual = await _community.LoadFactAsync<Individual>(ThisIndividual);
            if (individual == null)
            {
                string randomId = Punctuation.Replace(Guid.NewGuid().ToString(), String.Empty).ToLower();
                individual = await _community.AddFactAsync(new Individual(randomId));
                await _community.SetFactAsync(ThisIndividual, individual);
            }
            Individual = individual;
        }

        private async void CreateIndividualDesignData()
        {
            var individual = await _community.AddFactAsync(new Individual("design"));
            var first = await individual.JoinMessageBoardAsync("Correspondence");
            first.MessageBoard.SendMessageAsync("First Message");
            first.MessageBoard.SendMessageAsync("Second Message");
            var second = await individual.JoinMessageBoardAsync("Azure");
            second.MessageBoard.SendMessageAsync("Another Message");
            second.MessageBoard.SendMessageAsync("Final Message");
            Individual = individual;
        }
    }
}
