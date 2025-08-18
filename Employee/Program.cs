using AutoMapper;
using Employee.IOC;
using Employee.Mapper;
using Microsoft.EntityFrameworkCore;
using Repositories.DatabaseModels;
using Shared.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureDependencyContainer(builder.Configuration);

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();


ConfigurationKeys.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<EmployeeDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


var autoMapper = new MapperConfiguration(item => item.AddProfile(new MappingProfile()));
IMapper mapper = autoMapper.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
