using Microsoft.AspNetCore.ResponseCompression;
using RobustApiTemplate.Engine.Services;
using RobustApiTemplate.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Get connectionstring from appsettings.json.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddSingleton<IDatabaseService>(provider =>
    new DatabaseService(connectionString));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add response compression services
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<GzipCompressionProvider>();
    options.EnableForHttps = true; // Enable compression for HTTPS requests
});

// Configure the Gzip compression level (optional)
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Fastest; // Or use Optimal
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Add custom middleware to the request pipeline

// Add CorrelationIdMiddleware, to make a unique identifier for each request,
// that is available to all downstream components.
app.UseMiddleware<CorrelationIdMiddleware>();

// Add custom middleware to enforce maximum request size
app.UseMiddleware<MaxRequestSizeMiddleware>();

app.MapControllers();

app.Run();
