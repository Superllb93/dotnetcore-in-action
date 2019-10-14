using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;

namespace GrpcService.Server.Services
{
    public class PrinterService : Printer.PrinterBase
    {
        public PrinterService()
        {
        }

        public override async Task Echo(IAsyncStreamReader<EchoMessageRequest> requestStream, IServerStreamWriter<EchoMessageResponse> responseStream, ServerCallContext context)
        {
            var messageQueue = new Queue<string>();

            await foreach (var request in requestStream.ReadAllAsync())
            {
                messageQueue.Enqueue(request.Message);
            }

            while (messageQueue.TryDequeue(out var message))
            {
                await responseStream.WriteAsync(new EchoMessageResponse() { Message = $"收到的消息为：{message}" });

                await Task.Delay(500);//此处主要是为了方便客户端能看出流调用的效果
            }
        }
    }
}
