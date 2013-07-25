using System.Net;
using Common.Dto;

namespace Common.Services
{
    public static class LoginService
    {
        public static bool Login(LoginRequestDto loginRequestData)
        {
            loginRequestData.ToJson();
            WebRequest request = WebRequest.Create(GlobalContants.ServerPath + "LoginWebService.asmx/Login");

            CommonHelper.PrepareRequest(loginRequestData, request);

            AuthData.AuthResult = CommonHelper.ProcessRequest<AuthDto>(request);

            return AuthData.AuthResult.Token != null;
        }
    }
}