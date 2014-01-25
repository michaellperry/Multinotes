using System;
using System.Configuration;
using System.IO;
using System.Timers;
using System.Web.Hosting;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.BinaryHTTPClient;
using UpdateControls.Correspondence.FileStream;
using UpdateControls.Correspondence.SQL;
using UpdateControls.Collections;
using System.Collections.Generic;
using System.Linq;
using Multinotes.Model;
using UpdateControls.Fields;
using UpdateControls.Correspondence.Memory;

namespace Multinotes.Web
{
    public class SynchronizationService
    {
        private Community _community;
        private Independent<Domain> _domain = new Independent<Domain>();

        public void Initialize()
        {
			// TODO: Uncomment these lines to choose a database storage strategy.
            // string correspondenceConnectionString = ConfigurationManager.ConnectionStrings["Correspondence"].ConnectionString;
			// var storage = new SQLStorageStrategy(correspondenceConnectionString).UpgradeDatabase();

            string path = HostingEnvironment.MapPath("~/App_Data/Correspondence");
			var storage = new FileStreamStorageStrategy(path);
            var http = new HTTPConfigurationProvider();
			var communication = new BinaryHTTPAsynchronousCommunicationStrategy(http);

            _community = new Community(storage);
			_community.AddAsynchronousCommunicationStrategy(communication);
			_community.Register<CorrespondenceModel>();
            _community.Subscribe(() => Domain);
            _community.ClientApp = false;

            LoadDomain();

            // Synchronize whenever the user has something to send.
            _community.FactAdded += delegate
            {
                _community.BeginSending();
            };

            // Resume in 5 minutes if there is an error.
            Timer synchronizeTimer = new Timer();
            synchronizeTimer.Elapsed += delegate
            {
                _community.BeginSending();
                _community.BeginReceiving();
            };
            synchronizeTimer.Interval = 5.0 * 60.0 * 1000.0;
            synchronizeTimer.Start();
        }

        public void InitializeForTest()
        {
            _community = new Community(new MemoryStorageStrategy());
            _community.Register<CorrespondenceModel>();
            LoadDomain();
        }

        public Community Community
        {
            get { return _community; }
        }

        public Domain Domain
        {
            get
            {
                lock (this)
                {
                    return _domain;
                }
            }
            set
            {
                lock (this)
                {
                    _domain.Value = value;
                }
            }
        }

        private void LoadDomain()
        {
            _community.Perform(async delegate
            {
                var domain = await _community.AddFactAsync(new Domain());
                Domain = domain;

                _community.BeginSending();
                _community.BeginReceiving();
            });
        }
    }
}
