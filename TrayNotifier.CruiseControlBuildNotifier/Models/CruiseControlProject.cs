using System.Linq;
using System.Xml.Linq;

namespace TrayNotifier.CruiseControlBuildNotifier.Models
{
    using System;
    using System.Collections.Generic;
    public class CruiseControlProject
    {
        public string Name { get; set; }

        public string LastBuildStatus { get; set; }

        public DateTime? LastBuildTime { get; set; }

        public string WebUrl { get; set; }

        /// <summary>
        /// Parses a Cruise Control export 'Project' node into a CruiseControlProject object
        /// </summary>
        /// <param name="element">XElement object</param>
        /// <returns>Cruise Control Project</returns>
        public static CruiseControlProject ParseCruiseControlExportToCruiseControlProject(XElement element)
        {
            if (element == null)
                return null;

            var project = new CruiseControlProject();

            var name = element.Attribute("name");
            if (name != null)
                project.Name = name.Value;

            var lastBuildStatus = element.Attribute("lastBuildStatus");
            if (lastBuildStatus != null)
                project.LastBuildStatus = lastBuildStatus.Value;

            var lastBuildTime = element.Attribute("lastBuildTime");
            DateTime date;
            if (lastBuildTime != null && DateTime.TryParse(lastBuildTime.Value, out date))
                project.LastBuildTime = date;

            var url = element.Attribute("webUrl");
            if (url != null)
                project.WebUrl = url.Value;

            return project;
        }

        /// <summary>
        /// Parses an Xml Document into a list of Cruise Control Project objects
        /// </summary>
        /// <param name="xml">Xml Document from the Cruise Control Site</param>
        /// <returns>List of Cruise Control Projects</returns>
        public static List<CruiseControlProject> ParseCruiseControlExportToListOfProjects(string xml)
        {
            var document = XDocument.Parse(xml);
            return document.Descendants("Projects")
                           .Descendants("Project")
                           .Select(ParseCruiseControlExportToCruiseControlProject)
                           .ToList();
        }
    }
}