using Microsoft.JSInterop;
using Persistence.DAL;
using PresentationClient.Entities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<ProductsScope>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

PersistenceFacade.Instance.SetIntegrationMode(IntegrationMode.Production);


app.UseRouting();


app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();