using Battleship.WASM.Server.Hubs;
using Battleship.WASM.Server.Services;
using MediatR;
using Microsoft.AspNetCore.ResponseCompression;
using Serilog;

var builder = WebApplication
    .CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console().CreateLogger();

var loggerFactory = new LoggerFactory().AddSerilog(Log.Logger);


// Add services to the container.
builder.Host.UseSerilog(Log.Logger);
builder.Logging.AddSerilog(Log.Logger);
builder.Services.AddSingleton(typeof(ILoggerFactory), loggerFactory);
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddSingleton<PlayerQueue>();
builder.Services.AddSingleton<PlayerConnections>();
builder.Services.AddSingleton<IBattleshipService, BattleshipService>();
builder.Services.AddSingleton<IBattleshipGameEngine, BattleshipGameEngine>();
builder.Services.AddHostedService<MatchMakingService>();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapHub<ChatHub>("/chathub");
app.MapHub<BattleshipHub>("/battleshiphub");
app.MapFallbackToFile("index.html");

app.Run();
