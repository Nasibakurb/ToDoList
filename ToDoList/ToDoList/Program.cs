using Microsoft.EntityFrameworkCore;
using System;
using ToDoList.Domain.Entity;
using ToDoList.Domain.Interfaces;
using ToDoList.Infrastructure.Data;
using ToDoList.Infrastructure.Response;
using ToDoList.Infrastructure.Service;

var builder = WebApplication.CreateBuilder(args);

//........ Подключение к дб ........//
var connectstr = builder.Configuration.GetConnectionString("MSSql");
builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(connectstr);
});
//................//

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation(); // .AddRazorRuntimeCompilation() Чтобы верстка обновлялась

//....... Регистрация репозиторий и сервисов........//
builder.Services.AddScoped<IBaseRepository<TaskEntity>, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();
//...............//


var app = builder.Build();

 if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
     app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Task}/{action=Index}/{id?}");

app.Run();
