/*
File Name:      MembershipUser.cs
Description:	Membership entity class to return AD Membership information.
Date created:	08 Aug 2007

Modification History:
*********************
CR		Name			            Date			Description
New		Sivashanmugam, Muniappan    08/08/2007		File has been created.
Changed	Sivashanmugam, Muniappan    03/10/2007		File modified to add additional properties.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace iAssetTrack.Security
{
    #region Custom Namespaces

    using c = iAssetTrack.Security.MembershipAttributeConstants;

    #endregion

    [Serializable]
    public sealed class MembershipUser
    {
        private Hashtable _properties = new Hashtable(StringComparer.OrdinalIgnoreCase);

        public MembershipUser() 
        {
        }
        
        public MembershipUser(Hashtable properties)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");
            _properties = properties;
        }

        private MembershipUser(String userName)
        {
            if (userName == null)
                throw new ArgumentNullException("userName");
            _properties[c.samAccountName] = userName;
        }

        private Hashtable Properties
        {
            get { return _properties; }
            set { _properties = value; }
        }

        public String UserName
        {
            get { return (String)_properties[c.samAccountName]; }
        }

        public String GivenName
        {
            get
            {
                if (_properties[c.givenName] == null)
                    return "";
                else
                    return (String)_properties[c.givenName];
            }
        }

        public String DisplayName
        {
            get
            {
                if (_properties[c.displayName] == null)
                    return "";
                else
                    return (String)_properties[c.displayName];
            }
        }

        public String Email
        {
            get
            {
                if (_properties[c.mail] == null)
                    return "";
                else
                    return (String)_properties[c.mail];
            }
        }

        public bool IsDisabled
        {
            get { return (bool)_properties[c.userAccountDisabled]; }
        }

        public bool IsLockedOut
        {
            get { return (bool)_properties[c.accountLocked]; }
        }

        public DateTime LastPasswordChangedDate
        {
            get { return (DateTime)_properties[c.pwdLastSet]; }
        }

        public bool IsPasswordNeverExpiring
        {
            get { return (bool)_properties[c.pwdNeverExpires]; }
        }
    }
}
