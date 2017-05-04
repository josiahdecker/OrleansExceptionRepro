using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using GrainInterfaces;
using System.Net;
using Microsoft.WindowsAzure.Storage;

namespace Grains
{
    public class SerializationTesterGrain : Grain, IValueInterface
    {
        public Task<int> GetValue()
        {
            var inner = new Exception();
            var z = new RequestResult();

            var e = new StorageException(z, "some ex", inner);
            e.RequestInformation.Exception = e;


            throw e;
        }
    }
}
