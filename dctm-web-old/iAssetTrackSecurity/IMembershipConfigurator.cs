/*
File Name:      IConfigurator.cs
Description:	Interface definition to implement reading AD configuration settings.
Date created:	23 Jul 2007

Modification History:
*********************
CR		Name			            Date			Description
New		Sivashanmugam, Muniappan    23/07/2007		File has been created.
Changed	Sivashanmugam, Muniappan    03/10/2007		File has been modified to remove unused methods.
*/
using System;
using System.Collections;
using System.Collections.Specialized;

namespace iAssetTrack.Security
{
    /// <summary>
    /// Interface for accessing configurator to read AD configuration settings.
    /// </summary>
    /// <author>Sivashanmugam, Muniappan</author>
    /// <createdOn>23 Jul 2007</createdOn>
    public interface IMembershipConfigurator
    {
        object Read(string sectionName);
    }
}
