/*
File Name:      IAuthentication.cs
Description:	Interface definition to implement AD authentication methods.
Date created:	08 Aug 2007

Modification History:
*********************
CR		Name			            Date			Description
New		Sivashanmugam, Muniappan    08/08/2007		File has been created.
Changed	Sivashanmugam, Muniappan    03/10/2007		File has been modified to use new methods.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace iAssetTrack.Security
{
    public interface IMembershipProvider
    {
        bool ValidateUser(String userName, String password);
        void ChangePassword(String userName, String currentPassword, String newPassword);
        MembershipUser GetUser(String userName);
    }
}
