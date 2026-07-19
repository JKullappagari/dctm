using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iAssetTrackBAL
{
    public class ErrorHandlerBAL
    {
        Exception exception;
        public ErrorHandlerBAL()
        {

        }

        public ErrorHandlerBAL(Exception ex)
        {
            exception = ex;
        }

        public string ReturnErrorMessage()
        {
            return "<BR><B>Error Message : " + exception.Message + "</B><BR>";
        }
    }
}
