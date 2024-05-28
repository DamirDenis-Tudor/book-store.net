using Business.BAL;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;

using Persistence.DAL;
using PresentationClient.Entities;
using PresentationClient.Pages;
using PresentationClient.Service;
using PresentationClient.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//builder.Services.AddScoped<ProductsScope>();
builder.Services.AddScoped<PersonalDetailsDataScoped>();

//builder.Services.AddSingleton<ProtectedLocalStorage>();
builder.Services.AddScoped<ProtectedLocalStorage>();
builder.Services.AddSingleton<BusinessFacade>();
builder.Services.AddScoped<ICartService, CartServiceLocal>();
builder.Services.AddScoped<IUserLoginService, UserLoginService>();

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