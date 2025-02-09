using ContainerConfiguration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

DiConfiguration.Configure(builder.Services);
builder.Services.AddOpenApi();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
