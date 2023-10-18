using Downloader.Data;
using Downloader.Services;
using Downloader.Services.Interfaces;
using Downloader.Workers;
using Lib.AspNetCore.ServerSentEvents;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DbConnection") ??
                       throw new InvalidOperationException("Connection string 'DbConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)
);
builder.Services.AddServerSentEvents();
// Add services to the container.
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddSingleton<DownloadQueueManager>();
builder.Services.AddHostedService<DownloadQueueLoader>();
builder.Services.AddHostedService<DownloadQueueProgress>();

builder.Services.AddHttpClient(nameof(DownloadQueueManager));
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(e =>
{
    e.MapServerSentEvents("/api/sse");
    e.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");
});
app.MapFallbackToFile("index.html");
app.Run();