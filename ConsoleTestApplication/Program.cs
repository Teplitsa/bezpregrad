using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Dto;
using Common.Services;

namespace ConsoleTestApplication
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var loginData = new LoginRequestDto()
            {
                Login = "admin",
                Password = "123"
            };

            if (!LoginService.Login(loginData))
                Debugger.Break();

            PointsSetDto points = PointService.ListPoints();

            var newPoint = new PointDto()
                {
                    Name = "123"
                };

            var result = PointService.NewPoint(newPoint);

            if(result.Result != null)
                Debugger.Break();

            points = PointService.ListPoints();

            var old = points.Points.Last();

            old.Name = "321";

            result = PointService.EditPoint(old);

            if (result.Result != null)
                Debugger.Break();

            result = PointService.RemovePoint(old);

            if (result.Result != null)
                Debugger.Break();

            points = PointService.ListPoints();


            // Thread.Sleep(1000000);
        }
    }
}
