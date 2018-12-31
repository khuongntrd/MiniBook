using System;
using System.Collections.Generic;
using System.Text;

namespace MiniBook
{
    public enum ErrorCode
    {
        BAD_REQUEST = 9999,

        REGISTER_DUPLICATE_USER_NAME = 1001,
        REGISTER_REQUIRED_EMAIL = 1002,
        REGISTER_REQUIRED_LAST_NAME = 1003,
        REGISTER_REQUIRED_FIRST_NAME = 1004
    }
}
