using Microsoft.Extensions.FileProviders;
using Worker.Services;
Console.WriteLine(args.FirstOrDefault());

var builder = WebApplication.CreateBuilder();
builder.WebHost.ConfigureKestrel(options =>
{
	options.ListenAnyIP(5222, conf =>
	{
		conf.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
	});
	options.ListenAnyIP(5223, conf =>
	{
		conf.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
	});
	//options.ConfigureEndpointDefaults(k =>
	//{
	//	k.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
	//});
});
// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddControllers();

var app = builder.Build();
app.UseStaticFiles();
var fileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Resources"));
var requestPath = "/Resources";

app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = fileProvider,
	RequestPath = requestPath
});
//app.UseDirectoryBrowser(new DirectoryBrowserOptions
//{
//    FileProvider = fileProvider,
//    RequestPath = requestPath
//});
// Configure the HTTP request pipeline.
app.MapControllers();
app.MapGrpcService<GreeterService>();
app.MapGrpcService<MenuService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
