using AzureSubscriptionVendor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton(sp =>
    new MongoService(
        builder.Configuration.GetConnectionString("MongoDb"),
        builder.Configuration["MongoDb:Database"],
        builder.Configuration["MongoDb:Collection"]));

/*builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8081, listenOptions => listenOptions.UseHttps());
});*/

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
