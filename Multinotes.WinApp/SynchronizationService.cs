using Multinotes.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.BinaryHTTPClient;
using UpdateControls.Correspondence.FileStream;
using UpdateControls.Correspondence.Memory;
using UpdateControls.Fields;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Multinotes.WinApp
{
    public class SynchronizationService
    {
        private const string ThisIndividual = "Multinotes.WinApp.Individual.this";
        private static readonly Regex Punctuation = new Regex(@"[{}\-]");

        private Community _community;
        private Independent<Individual> _individual = new Independent<Individual>(
            Individual.GetNullInstance());

        public void Initialize()
        {
            var storage = new FileStreamStorageStrategy();
            var http = new HTTPConfigurationProvider();
            var communication = new BinaryHTTPAsynchronousCommunicationStrategy(http);

            _community = new Community(storage);
            _community.AddAsynchronousCommunicationStrategy(communication);
            _community.Register<CorrespondenceModel>();
            _community.Subscribe(() => Individual);
            _community.Subscribe(() => Individual.MessageBoards);

            // Synchronize periodically.
            DispatcherTimer timer = new DispatcherTimer();
            int timeoutSeconds = Math.Min(http.Configuration.TimeoutSeconds, 30);
            timer.Interval = TimeSpan.FromSeconds(5 * timeoutSeconds);
            timer.Tick += delegate(object sender, object e)
            {
                Synchronize();
            };
            timer.Start();

            CreateIndividual(http);

            // Synchronize whenever the user has something to send.
            _community.FactAdded += delegate
            {
                Synchronize();
            };

            // Synchronize when the network becomes available.
            System.Net.NetworkInformation.NetworkChange.NetworkAddressChanged += (sender, e) =>
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                    Synchronize();
            };

            // And synchronize on startup or resume.
            Synchronize();
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
            set
            {
                lock (this)
                {
                    _individual.Value = value;
                }
            }
        }

        public void Synchronize()
        {
            _community.BeginSending();
            _community.BeginReceiving();
        }

        private async void CreateIndividual(HTTPConfigurationProvider http)
        {
            Individual individual = await _community.LoadFactAsync<Individual>(ThisIndividual);
            if (individual == null)
            {
                string randomId = Punctuation.Replace(Guid.NewGuid().ToString(), String.Empty).ToLower();
                individual = await _community.AddFactAsync(new Individual(randomId));
                await _community.SetFactAsync(ThisIndividual, individual);
            }
            Individual = individual;
            http.Individual = individual;
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
