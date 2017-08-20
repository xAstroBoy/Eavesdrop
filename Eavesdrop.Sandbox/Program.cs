﻿using System;
using System.Threading.Tasks;

namespace Eavesdrop.Sandbox
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = new Program();
            app.Run();
        }

        private void Run()
        {
            Eavesdropper.Overrides.AddRange(new[] { "*google.com", "*discordapp.com" });
            Eavesdropper.RequestInterceptedAsync += RequestInterceptedAsync;
            Eavesdropper.ResponseInterceptedAsync += ResponseInterceptedAsync;

            Eavesdropper.Certifier.CreateTrustedRootCertificate();
            Eavesdropper.Initiate(8282);

            Console.Title = "Eavesdrop.Sandbox - Press any key to exit...";

            Console.ReadLine();
            Eavesdropper.Terminate();

            Console.WriteLine("Eavesdropper has been terminated! | " + DateTime.Now);
            Console.ReadLine();
        }

        private Task RequestInterceptedAsync(object sender, RequestInterceptedEventArgs e)
        {
            Console.WriteLine("Intercepted Request: " + e.Uri);
            return Task.CompletedTask;
        }
        private async Task ResponseInterceptedAsync(object sender, ResponseInterceptedEventArgs e)
        {
            var payload = new byte[0];
            if (e.Content != null)
            {
                payload = await e.Content.ReadAsByteArrayAsync();
            }
            Console.WriteLine($"Intercepted Response: {e.Uri}[{payload.Length:n0} Bytes]");
        }
    }
}