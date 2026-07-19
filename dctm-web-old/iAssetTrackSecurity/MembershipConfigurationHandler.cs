/*
File Name:      ADConfigurationHandler.cs
Description:	Implementation of IConfigurator interface to read AD configuration settings.
Date created:	23 Jul 2007

Modification History:
*********************
CR		Name			            Date			Description
New		Sivashanmugam, Muniappan    23/07/2007		File has been created.
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Configuration;

namespace iAssetTrack.Security
{
    public class MembershipConfigurationHandler : IConfigurationSectionHandler, IMembershipConfigurator
    {
        /// <summary>
        /// Returns instance of MembershipConfigurationHandler class.
        /// </summary>
        public static MembershipConfigurationHandler Instance
        {
            get { return new MembershipConfigurationHandler(); }
        }

        /// <summary>
        /// Reads AD settings from configuration file and builds ActiveDirectoryMembershipProvider class.
        /// </summary>
        /// <author>Sivashanmugam, Muniappan</author>
        /// <createdOn>23 Jul 2007</createdOn>
        public object Create(object parent, object configContext, XmlNode section)
        {
            ActiveDirectoryMembershipProvider config = new ActiveDirectoryMembershipProvider();
            config.LoadValuesFromConfigurationXml(section);
            return config;
        }

        #region IConfigurator Members

        /// <summary>
        /// Reads AD configuration given by SectionName.
        /// </summary>
        /// <author>Sivashanmugam, Muniappan</author>
        /// <createdOn>23 Jul 2007</createdOn>
        public object Read(string sectionName)
        {
            return (ActiveDirectoryMembershipProvider)ConfigurationManager.GetSection(sectionName);
        }

        #endregion
    }
}
