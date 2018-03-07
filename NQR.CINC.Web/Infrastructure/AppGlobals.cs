using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NQR.CINC.Web.Infrastructure
{
    public static class AppGlobals
    {
        /// <summary>
        /// Set Grid page size globally
        /// </summary>
        /// <returns></returns>
        public static int GridPageSize()
        {
            int pageSize = 10;
            return pageSize;
        }
    }
}