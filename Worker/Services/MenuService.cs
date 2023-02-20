using Grpc.Core;
using Grpc.Net.Client;

namespace Worker.Services
{
	public class MenuService : Menu.MenuBase
	{
		public MenuService()
		{

		}
		public override async Task<ResponseMessageMenu> BroadCast(RequestMessageMenu request, ServerCallContext context)
		{
			// The port number must match the port of the gRPC server.
			using var channel = GrpcChannel.ForAddress("http://localhost:5271");
			var client = new Menu.MenuClient(channel);
			request.Name = "Worker";
			var send = await client.ReceiverAsync
				(
					request
				);
			Console.WriteLine(request.Name + ":" + request.Body);

			return await Task.FromResult(new ResponseMessageMenu
			{
				Name = request.Name,
				Body = request.Body
			});
		}
		public override Task<ResponseMessageMenu> Receiver(RequestMessageMenu request, ServerCallContext context)
		{
			var filename = Guid.NewGuid().ToString();	
			var path = Path.Combine(Directory.GetCurrentDirectory(),"Resources",$"{filename}.text");
			File.WriteAllBytes(path,request.FileContent.ToArray());
			Console.WriteLine("From " + request.Name + ":" + request.Body);
			return Task.FromResult(new ResponseMessageMenu
			{
				Name = request.Name,
				Body = request.Body
			});
		}
	}
}
