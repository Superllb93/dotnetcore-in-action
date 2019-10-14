using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace GrpcService.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(
                new HelloRequest { Name = "longbao" });
            Console.WriteLine("Greeter 服务返回数据: " + reply.Message);
        }
    }
}
