using VenturaJobsHR.Bff.Common;

var builder = WebApplication.CreateBuilder(args);
builder.AddConfiguration();

var app = builder.Build();
app.UseConfiguration();

app.Run();


//TODO: CONFIGURAR MIDDLEWARE E VALIDAÇÕES NO BFF E NAS APIS