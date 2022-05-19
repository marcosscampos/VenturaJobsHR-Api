var builder = WebApplication.CreateBuilder(args);
builder.AddConfiguration();

var app = builder.Build();
app.UseConfiguration();

app.Run();
