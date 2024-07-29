using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Newtonsoft.Json;
using RESTFulSense.Clients;

namespace GeoApi.Benchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Summary summary =
                BenchmarkRunner.Run<Benchmark>();

            Console.WriteLine(summary);
        }
    }

    public class Benchmark
    {
        private readonly IRESTFulApiFactoryClient client;

        public Benchmark()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:44376/");

            this.client =
                new RESTFulApiFactoryClient(httpClient);
        }

        [Benchmark]
        public async Task WithValueTaskAsync()
        {
            List<Geo> geos = await this.client
                .GetContentAsync<List<Geo>>(
                    relativeUrl: "api/v2/geos");
        }

        [Benchmark]
        public async Task WithoutValueTaskAsync()
        {
            List<Geo> geos = await this.client
                .GetContentAsync<List<Geo>>(
                    relativeUrl: "api/geos");
        }
    }

    public class Geo
    {
        public Guid Id { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Zip { get; set; }
    }
}
