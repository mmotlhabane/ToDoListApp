using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace DotNetCoreSqlDb
{
  public class Program
  {
    public static void Main(string[] args)
    {
      BuildWebHost(args).Run();
    }

    //public static IWebHost BuildWebHost(string[] args) =>
    //    WebHost.CreateDefaultBuilder(args)
    //        .UseStartup<Startup>()
    //        .Build();

    /*public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
          .ConfigureAppConfiguration((context, config) =>
          {
            var builder = config.Build();

            var keyVaultEndpoint = builder["AzureKeyVaultEndpoint"];

            var azureServiceTokenProvider = new AzureServiceTokenProvider();

            var keyVaultClient = new KeyVaultClient(
              new KeyVaultClient.AuthenticationCallback(
                azureServiceTokenProvider.KeyVaultTokenCallback)
              );
            config.AddAzureKeyVault(keyVaultEndpoint);
          }).UseStartup<Startup>()
            .Build();
            */

    public static IWebHost BuildWebHost(string[] args) =>
     WebHost.CreateDefaultBuilder(args)
         .ConfigureAppConfiguration((ctx, builder) =>
         {
           var keyVaultEndpoint = GetKeyVaultEndpoint();
           if (!string.IsNullOrEmpty(keyVaultEndpoint))
           {
             var azureServiceTokenProvider = new AzureServiceTokenProvider();
             var keyVaultClient = new KeyVaultClient(
                       new KeyVaultClient.AuthenticationCallback(
                           azureServiceTokenProvider.KeyVaultTokenCallback));
             builder.AddAzureKeyVault(
                       keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
           }
         }
      ).UseStartup<Startup>()
       .Build();

    private static string GetKeyVaultEndpoint() => "https://Fort-Knox.vault.azure.net";

  }
}
