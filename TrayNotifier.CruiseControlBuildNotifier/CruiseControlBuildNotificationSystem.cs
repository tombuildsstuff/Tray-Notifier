using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using TrayNotifier.Business.Helpers;
using TrayNotifier.CruiseControlBuildNotifier.Models;

namespace TrayNotifier.CruiseControlBuildNotifier
{
    using Business;

    public class CruiseControlBuildNotificationSystem : AbstractNotificationSystem
    {
        private readonly WebClient _webClient;
        private readonly string _cruiseControlInstallUrl;
        private readonly int _numSecondsToCheck;
        private bool _isDownloading;
        private List<CruiseControlProject> _currentProjects;

        /// <summary>
        /// Initializes a new instance of the <see cref="CruiseControlBuildNotificationSystem"/> class.
        /// </summary>
        /// <param name="cruiseControlInstallUrl">
        /// The Cruise Control URL i.e. "http://build.mycompany.com/ccnet/"
        /// </param>
        /// <param name="numSecondsToCheck">
        /// The interval used to check the build status at; defaults to 90
        /// </param>
        /// <param name="credentials">
        /// The credentials used to connect to the build URL
        /// </param>
        public CruiseControlBuildNotificationSystem(string cruiseControlInstallUrl, int numSecondsToCheck = 90, ICredentials credentials = null)
        {
            _cruiseControlInstallUrl = cruiseControlInstallUrl;
            _numSecondsToCheck = numSecondsToCheck;

            _currentProjects = new List<CruiseControlProject>();
            _webClient = new WebClient { Credentials = credentials, CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore) };
            _webClient.DownloadDataCompleted += Data_Downloaded;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CruiseControlBuildNotificationSystem"/> class using the Cruise Control Configuration provided
        /// </summary>
        /// <param name="configuration">
        /// Cruise Control Configuration Implementation
        /// </param>
        public CruiseControlBuildNotificationSystem(CruiseControlConfiguration configuration)
        {
            _cruiseControlInstallUrl = configuration.InstallDirectoryUrl;
            _numSecondsToCheck = configuration.CheckInterval ?? 90;

            _currentProjects = new List<CruiseControlProject>();
            var credentials = !string.IsNullOrWhiteSpace(configuration.Username) ? new NetworkCredential(configuration.Username, configuration.Password, configuration.Domain) : null;
            _webClient = new WebClient { Credentials = credentials, CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore) };
            _webClient.DownloadDataCompleted += Data_Downloaded;
        }

        public override int NumberOfSecondsToCheck()
        {
            return _numSecondsToCheck;
        }

        public override void Check()
        {
            if (_isDownloading)
                return;

            _isDownloading = true;
            _webClient.DownloadDataAsync(new Uri(string.Format("{0}/ViewFarmReport.aspx/XmlServerReport.aspx", _cruiseControlInstallUrl)));
        }

        private void Data_Downloaded(object sender, DownloadDataCompletedEventArgs e)
        {
            _isDownloading = false;
            
            if (e.Error != null)
                throw e.Error;

            var xml = e.Result.ConvertToString();
            var newProjects = CruiseControlProject.ParseCruiseControlExportToListOfProjects(xml);
            var changedProjects = new List<CruiseControlProject>();

            var initialLoad = _currentProjects.Count == 0;
            foreach (var project in newProjects)
            {
                var existing = _currentProjects.Where(p => p.Name == project.Name).FirstOrDefault();
                if (existing == null || (existing.LastBuildStatus != project.LastBuildStatus) || (existing.LastBuildTime != project.LastBuildTime))
                    changedProjects.Add(project);
            }

            _currentProjects = newProjects;

            var successful = changedProjects.Where(cp => cp.LastBuildStatus == "Success").OrderBy(p => p.Name).ToList();
            var failed = changedProjects.Where(cp => cp.LastBuildStatus == "Failed").OrderBy(p => p.Name).ToList();
            var unknown = changedProjects.Where(cp => !successful.Any(s => s.Name == cp.Name) && !failed.Any(f => f.Name == cp.Name)).ToList();

            if (!initialLoad)
                foreach (var project in unknown)
                    if (MessageReceived != null)
                        MessageReceived(10000, "Oooh look at you doing some work!", string.Format("{0} is a new project..", project.Name), Icon.Warning);

            foreach (var project in failed)
                if (MessageReceived != null)
                    MessageReceived(20000,
                                    string.Format("{0} is Broken", project.Name),
                                    string.Format("Broke At {0} {1}", project.LastBuildTime.Value.ToLongDateString(), project.LastBuildTime.Value.ToShortTimeString()),
                                    Icon.Error);

            if (!initialLoad)
                foreach (var project in successful)
                    if (MessageReceived != null)
                        MessageReceived(6000,
                                        string.Format("{0} Build Again", project.Name),
                                        string.Format("Built At {0} {1}", project.LastBuildTime.Value.ToLongDateString(), project.LastBuildTime.Value.ToShortTimeString()),
                                        Icon.Info);
        }
    }
}