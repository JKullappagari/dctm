/*
File Name:      MembershipException.cs
Description:	Exception class to handle custom application exceptions in AD authentication.
Date created:	08 Aug 2007

Modification History:
*********************
CR		Name			            Date			Description
New		Sivashanmugam, Muniappan    08/08/2007		File has been created.
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace iAssetTrack.Security
{
    [Serializable]
    public class MembershipException : ApplicationException
    {
        public MembershipException()
            : base()
        {
        }

        public MembershipException(string message)
            : base(message)
        {
        }

        protected MembershipException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public MembershipException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
