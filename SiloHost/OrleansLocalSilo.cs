using System;
using System.Threading.Tasks;

using Orleans;
using Orleans.Runtime.Configuration;

namespace Portal.OrleansSiloHost
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class OrleansLocalSilo
    {
        private static AppDomain hostDomain;
        private static OrleansHostWrapper hostWrapper;

        public static void InitializeLocalSilo()
        {
            // The Orleans silo environment is initialized in its own app domain in order to more
            // closely emulate the distributed situation, when the client and the server cannot
            // pass data via shared memory.

            //this appdomain is initialized within the context of the IIS process so 
            //we have to point the appdomain back to where it can find all the orleans files
            //and config

            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            hostDomain = AppDomain.CreateDomain("TestOrleansHost", null, new AppDomainSetup
            {
                AppDomainInitializer = InitSilo,
                AppDomainInitializerArguments = new string[2]
                    {
                        "LocalDevSilo", /* silo name */
                        baseDir + "DevTestServerConfiguration.xml" /* orleans config path */
                    },
                ApplicationBase = baseDir,
                PrivateBinPath = baseDir + "bin\\Debug",
                ConfigurationFile = baseDir + "App.config"
            });
        }

        static void InitSilo(string[] args)
        {
            hostWrapper = new OrleansHostWrapper(args);

            if (!hostWrapper.Run())
            {
                Console.Error.WriteLine("Failed to initialize Orleans silo");
            }
        }

        static void ShutdownSilo()
        {
            if (hostWrapper != null)
            {
                hostWrapper.Dispose();
                GC.SuppressFinalize(hostWrapper);
            }
        }

    }
}
