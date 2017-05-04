using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.OrleansSiloHost;
using GrainInterfaces;
using Orleans;
using Orleans.Runtime.Configuration;
using Orleans.Serialization;
using System.Reflection;
using Microsoft.WindowsAzure.Storage;

namespace OrleansInterfacesTest
{
    class Program
    {
        static void Main(string[] args)
        {
            OrleansLocalSilo.InitializeLocalSilo();

            ClientConfiguration config = ClientConfiguration.LoadFromFile("OrleansClientConfiguration.xml");
            config.FallbackSerializationProvider = typeof(ILBasedSerializer).GetTypeInfo();
            GrainClient.Initialize(config);

            var p = new Program();
            p.Run().GetAwaiter().GetResult();
        }

        private async Task Run()
        {
            var grain = GrainClient.GrainFactory.GetGrain<IValueInterface>(4);
            int value = await grain.GetValue().ConfigureAwait(false);
            Console.WriteLine(value);
        }
    }
}
