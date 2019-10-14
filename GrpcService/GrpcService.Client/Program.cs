using Grpc.Core;
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

            Console.WriteLine("------双向流式调用开始------");
            var printClient = new Printer.PrinterClient(channel);
            using (var call = printClient.Echo())
            {
                Console.WriteLine("Starting background task to receive messages");
                var readTask = Task.Run(async () => 
                {
                    await foreach (var response in call.ResponseStream.ReadAllAsync())
                    {
                        Console.WriteLine(response.Message);
                    }
                });
                Console.WriteLine("Starting to send messages");
                Console.WriteLine("Type a message to echo then press enter.");
                while (true)
                {
                    var result = Console.ReadLine();
                    if (string.IsNullOrEmpty(result))
                    {
                        break;
                    }
                    await call.RequestStream.WriteAsync(new EchoMessageRequest { Message = result });
                }
                Console.WriteLine("Disconnecting");
                await call.RequestStream.CompleteAsync();
                await readTask;
            }
            Console.WriteLine("------双向流式调用结束------");
        }
    }
}
