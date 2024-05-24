using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;

using Persistence.DAL;
using PresentationClient.Entities;
using PresentationClient.Pages;
using PresentationClient.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<ProductsScope>();
builder.Services.AddScoped<PersonalDetailsDataScoped>();

//builder.Services.AddSingleton<ProtectedLocalStorage>();
builder.Services.AddScoped<ProtectedLocalStorage>();
builder.Services.AddScoped<ICartService, CartServiceLocal>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

PersistenceAccess.Instance.SetIntegrationMode(IntegrationMode.Production);


app.UseRouting();


app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();