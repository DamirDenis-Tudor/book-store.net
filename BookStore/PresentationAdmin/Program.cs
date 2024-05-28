/**************************************************************************
 *                                                                        *
 *  File:        Program.cs                                               *
 *  Copyright:   (c) 2024, Asmarandei Catalin                             *
 *  Website:     https://github.com/DamirDenis-Tudor/BookStore.NET        *
 *  Description: The class that is called when the app is launched        *
 *                                                                        *
 *  This program is free software; you can redistribute it and/or modify  *
 *  it under the terms of the GNU General Public License as published by  *
 *  the Free Software Foundation. This program is distributed in the      *
 *  hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 *  the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 *  PURPOSE. See the GNU General Public License for more details.         *
 *                                                                        *
 **************************************************************************/

using Business.BAL;
using Persistence.DAL;
using PresentationAdmin.Entities;
using PresentationAdmin.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<ProductsScope>();
builder.Services.AddSingleton<BusinessFacade>();
builder.Services.AddScoped<IUserLoginService, UserLoginService>();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
    app.UseHsts();


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();
