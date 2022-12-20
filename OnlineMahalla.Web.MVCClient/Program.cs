using Microsoft.AspNetCore;


namespace OnlineMahalla.Web.MVCClient
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Console.Title = "Online Mahalla.Web.MVCClient";
            CreateWebHostBuilder(args).UseContentRoot(Directory.GetCurrentDirectory()).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureKestrel(o =>
                {
                    o.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
                    o.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
                    o.Limits.MaxConcurrentConnections = 3000;
                    o.Limits.MaxConcurrentUpgradedConnections = 3000;
                    o.Limits.Http2.MaxStreamsPerConnection = 3000;
                });
    }
}
