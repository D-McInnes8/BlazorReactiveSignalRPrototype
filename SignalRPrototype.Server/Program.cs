using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using SignalRPrototype.Server.HostedServices;
using SignalRPrototype.Server.Hubs;
using SignalRPrototype.Server.Hubs.Filters;
using SignalRPrototype.Server.Services.Concrete;
using SignalRPrototype.Server.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen();

builder.Services
    .AddSignalR()
    .AddHubOptions<MessageHub>(options =>
    {
        options.AddFilter<RefreshPricesFilter>();
    });

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

builder.Services.AddHostedService<AzureQueueHostedService>();
builder.Services.AddSingleton<ISessionHandler, InMemorySessionStorage>();
builder.Services.AddSingleton<ISignalRService, SignalRService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
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
app.MapFallbackToFile("index.html");

app.MapHub<MessageHub>("/hub");
app.MapDefaultControllerRoute();

// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapHub<MessageHub>("/hub");
//     endpoints.MapDefaultControllerRoute();
// });

app.Run();