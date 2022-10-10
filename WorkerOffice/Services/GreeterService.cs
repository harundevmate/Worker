using Grpc.Core;
using Grpc.Net.Client;

namespace WorkerOffice.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        public GreeterService()
        {
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
        public override Task<ResponseMessage> Receiver(RequestMessage request, ServerCallContext context)
        {
            Console.WriteLine("From " + request.Name + ":" + request.Body);
            return Task.FromResult(new ResponseMessage
            {
                Name = request.Name,
                Body = request.Body
            });
        }
        public override async Task<ResponseMessage> BroadCast(RequestMessage request, ServerCallContext context)
        {
            // The port number must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("http://localhost:5222");
            var client = new Greeter.GreeterClient(channel);
            request.Name = "Office";
            var send = await client.ReceiverAsync
                (
                    request
                );
            Console.WriteLine(request.Name + ":" + request.Body);

            return await Task.FromResult(new ResponseMessage
            {
                Name = request.Name,
                Body = request.Body
            });
        }
    }
}