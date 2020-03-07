using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Hosting;
using Orleans.Runtime;

namespace OrleansDerivedInterfaceAssessment
{
    internal class Startup
    {
        private static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .UseOrleans(builder => builder
                    .ConfigureApplicationParts(manager =>
                    {
                        manager.AddApplicationPart(typeof(Startup).Assembly).WithReferences();
                    })
                    .ConfigureLogging(loggingBuilder =>
                    {
                        loggingBuilder.AddConsole();
                        loggingBuilder.SetMinimumLevel(LogLevel.Information);
                    })
                    .UseLocalhostClustering()
                );
        }

        private static async Task Main(string[] args)
        {
            using var host = CreateHostBuilder().Build();
            await host.StartAsync();

            using var client = host.Services.GetRequiredService<IClusterClient>();
            var logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger(typeof(Startup));

            // casting direct reference

            var localAccount = (IAccountProxy) client.GetGrain<ILocalAccount>(Guid.NewGuid());
            Debug.Assert(await localAccount.GetAccountType() == AccountType.Local);
            logger.Info($"localAccount.GetAccountType(): {await localAccount.GetAccountType()}");

            var remoteAccount = (IAccountProxy) client.GetGrain<IRemoteAccount>(Guid.NewGuid());
            Debug.Assert(await remoteAccount.GetAccountType() == AccountType.Remote);
            logger.Info($"remoteAccount.GetAccountType(): {await remoteAccount.GetAccountType()}");

            // reference casted and hold by another grain

            var localAccountHolder = client.GetGrain<IAccountProxyHolder>(Guid.NewGuid());
            await localAccountHolder.Initialize(AccountType.Local);
            Debug.Assert(await localAccountHolder.GetAccountType() == AccountType.Local);
            logger.Info($"localAccountHolder.GetAccountType(): {await localAccountHolder.GetAccountType()}");

            var remoteAccountHolder = client.GetGrain<IAccountProxyHolder>(Guid.NewGuid());
            await remoteAccountHolder.Initialize(AccountType.Remote);
            Debug.Assert(await remoteAccountHolder.GetAccountType() == AccountType.Remote);
            logger.Info($"remoteAccountHolder.GetAccountType(): {await remoteAccountHolder.GetAccountType()}");

            if (Debugger.IsAttached)
                Debugger.Break(); // stop for checking logs             

            await host.StopAsync();
        }
    }
}