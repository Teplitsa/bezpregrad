using System;
using Common.Dto;
using Common.Services;

namespace Common
{
    public static class AuthData
    {
        private static AuthDto authResuest;

        public static AuthDto AuthResult
        {
            get;
            set;
        }
    }
}