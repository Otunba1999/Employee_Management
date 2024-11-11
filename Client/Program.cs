using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using ClientLibrary.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using ClientLibrary.Services.Interfaces;
using ClientLibrary.Services.Implementations;
using Blazored.LocalStorage;
using Syncfusion.Blazor.Popups;
using Syncfusion.Blazor;
using Client.ApplicationState;
using BaseLibrary.Entities;
using Syncfusion.Licensing;

 var licence = "Ngo9BigBOggjHTQxAR8/V1NCaF1cWWhBYVppR2Nbe05zfldGalxVVAciSV9jS3pTfkVhWXxccHRRRWVeUg==";
SyncfusionLicenseProvider.RegisterLicense(licence);
var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<CustomHttpHandler>();
builder.Services.AddHttpClient("SystemAPIClient", client =>
    client.BaseAddress = new Uri("http://localhost:5022/")).AddHttpMessageHandler<CustomHttpHandler>();
// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5022/") });
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<HttpClientService>();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddScoped<SfDialogService>();
builder.Services.AddScoped<AppState>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGenericService<GenaralDepartement>, GenericService<GenaralDepartement>>();
builder.Services.AddScoped<IGenericService<Department>, GenericService<Department>>();
builder.Services.AddScoped<IGenericService<Branch>, GenericService<Branch>>();
builder.Services.AddScoped<IGenericService<Country>, GenericService<Country>>();
builder.Services.AddScoped<IGenericService<City>, GenericService<City>>();
builder.Services.AddScoped<IGenericService<Town>, GenericService<Town>>();
builder.Services.AddScoped<IGenericService<Employee>, GenericService<Employee>>();


await builder.Build().RunAsync();
