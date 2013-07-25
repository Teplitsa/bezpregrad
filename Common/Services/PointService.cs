using System;
using System.Net;
using Common.Dto;

namespace Common.Services
{
    public static class PointService
    {
        public static PointsSetDto ListPoints()
        {
            WebRequest request = WebRequest.Create(GlobalContants.ServerPath + "PointManager.asmx/MyPoints");

            CommonHelper.PrepareRequest(AuthData.AuthResult, request);

            return CommonHelper.ProcessRequest<PointsSetDto>(request);
        }

        public static StringResultDto NewPoint(PointDto point)
        {
            WebRequest request = WebRequest.Create(GlobalContants.ServerPath + "PointManager.asmx/NewPoint");

            var input = new EditPointDto
                {
                    Auth = AuthData.AuthResult,
                    Point = point
                };

            CommonHelper.PrepareRequest(input, request);

            return CommonHelper.ProcessRequest<StringResultDto>(request);
        }

        public static StringResultDto EditPoint(PointDto point)
        {
            WebRequest request = WebRequest.Create(GlobalContants.ServerPath + "PointManager.asmx/EditPoint");

            var input = new EditPointDto
                {
                    Auth = AuthData.AuthResult,
                    Point = point
                };

            CommonHelper.PrepareRequest(input, request);

            return CommonHelper.ProcessRequest<StringResultDto>(request);
        }

        public static StringResultDto RemovePoint(PointDto point)
        {
            WebRequest request = WebRequest.Create(GlobalContants.ServerPath + "PointManager.asmx/RemovePoint");

            var input = new EditPointDto
                {
                    Auth = AuthData.AuthResult,
                    Point = point
                };

            CommonHelper.PrepareRequest(input, request);

            return CommonHelper.ProcessRequest<StringResultDto>(request);
        }
        
        public static StringResultDto SavePhoto(int pointId, string photoBase64)
        {
            WebRequest request = WebRequest.Create(GlobalContants.ServerPath + "PointManager.asmx/SavePhoto");

            var input = new PointPhotoDto()
            {
                Auth = AuthData.AuthResult,
                Id = pointId,
                PhotoBase64 = photoBase64
            };

            CommonHelper.PrepareRequest(input, request);

            return CommonHelper.ProcessRequest<StringResultDto>(request);
        }
    }
}