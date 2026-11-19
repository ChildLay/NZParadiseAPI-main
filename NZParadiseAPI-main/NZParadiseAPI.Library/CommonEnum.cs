using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZParadiseAPI.Library
{
    public static class CommonEnum
    {
        public enum ResponseStatus
        {
            Success = 1,
            Failed = -1
        }

        public enum AttachmentParentType
        {
            PropertyImage = 1
        }

        public enum UserType
        {
            Member = 1,
            Admin = 9
        }
    }
}
