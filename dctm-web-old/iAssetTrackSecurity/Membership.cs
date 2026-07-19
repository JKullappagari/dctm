/*
File Name:      Membership.cs
Description:	Membership entity class to return domain information.
Date created:	08 Aug 2007

Modification History:
*********************
CR		Name			            Date			Description
New		Sivashanmugam, Muniappan    08/08/2007		File has been created.
*/
using System;
using System.Collections.Generic;
using System.Text;

using m = iAssetTrack.Security.MembershipErrorConstants;

namespace iAssetTrack.Security
{
    internal class MembershipConfigurator : MembershipConfigurationHandler
    {
        private const String ConfigSectionName = "activeDirectoryMembership";

        /// <summary>
        /// Reads AD settings from configuration file and builds ActiveDirectoryMembershipProvider class.
        /// </summary>
        /// <author>Sivashanmugam, Muniappan</author>
        /// <createdOn>23 Jul 2007</createdOn>
        public static ActiveDirectoryMembershipProvider GetConfig()
        {
            ActiveDirectoryMembershipProvider settings;
            settings = (ActiveDirectoryMembershipProvider)Instance.Read(ConfigSectionName);
            if (settings == null)
                throw new NullReferenceException(m.ADERR_SERVER_CONFIGURATIONSETTINGS_NOT_FOUND);
            return settings;
        }
    } 

    public static class Membership
    {
        private static ActiveDirectoryMembershipProvider _configurationSettings = MembershipConfigurator.GetConfig();

        /// <summary>
        /// Returns the DomainCollection by reading domain information from configuration file.
        /// </summary>
        /// <author>Sivashanmugam, Muniappan</author>
        /// <createdOn>03 Oct 2007</createdOn>
        public static DomainCollection Domains
        {
            get { return _configurationSettings.Domains; }
        }

        /// <summary>
        /// Returns default domain as specified in configuration file from DomainCollection. 
        /// </summary>
        /// <author>Sivashanmugam, Muniappan</author>
        /// <createdOn>03 Oct 2007</createdOn>
        public static Domain Domain
        {
            get
            {
                foreach (Domain domain in _configurationSettings.Domains)
                {
                    if (domain.DefaultDomain) return domain;
                }
                return null;
            }
        }
    }
}
