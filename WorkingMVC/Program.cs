using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WorkingMVC.Data;
using WorkingMVC.Data.Entitys;
using WorkingMVC.Data.Entitys.Identity;
using WorkingMVC.Interfaces;
using WorkingMVC.Repositories;
using WorkingMVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MyAppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddSingleton<IHiddenCategoryService, HiddenCategoryService>();
builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
{
    //налаштування пароля для користувачів
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
    .AddEntityFrameworkStores<MyAppDbContext>() //Використання нашої БД
    .AddDefaultTokenProviders(); //додає токени для підтвердження email, скидання пароля і т.д.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=Index}/{id?}")
    .WithStaticAssets();

string dirPath = builder.Configuration.GetValue<string>("DirPath") ?? "test";
//Console.WriteLine($"DirPath: {dirPath}");
string fullDirPath = Path.Combine(Directory.GetCurrentDirectory(), dirPath);
Directory.CreateDirectory(fullDirPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(fullDirPath),
    RequestPath = $"/{dirPath}"
});

using (var scoped = app.Services.CreateScope())
{
    //Ініціалізація бази даних
    var dbContext = scoped.ServiceProvider.GetRequiredService<MyAppDbContext>();
    //Отримання сервісу для роботи з ролями
    var roleManager = scoped.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
    dbContext.Database.Migrate();
    if (!dbContext.Categories.Any())
    {
        //var categories = new List<CategoryEntity>
        //{
        //    new CategoryEntity { Name = "Напої безалкогольні", Image = "https://tykyiv.com/media/GettyImages-525338134.jpg" },
        //    new CategoryEntity { Name = "Овочі та фрукти", Image = "https://ecosmak.com.ua/image/cache/catalog/blog/frukty-ta-ovochi/frukty-i-ovoshi-1200x700.jpeg" },
        //    new CategoryEntity { Name = "Молочні продукти", Image = "https://kurs.if.ua/wp-content/uploads/2023/04/image1-1024x682.png" },
        //};
        //dbContext.Categories.AddRange(categories);
        //dbContext.SaveChanges();
    }
    //Створення ролей в БД, якщо їх ще немає
    if (!dbContext.Roles.Any())
    {
        //масив з назвами ролей
        string[] roles = { "Admin", "User" };
        //перебираємо назви ролей
        foreach (var roleName in roles)
        {
            //Створюємо об'єкт ролі
            var role = new RoleEntity { Name = roleName };
            //Додаємо роль в БД через RoleManager
            var result = await roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
               Console.WriteLine($"Role '{roleName}' created successfully.");
            }
            else
            {
                Console.WriteLine($"Error creating role '{roleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

        }
    }
}


app.Run();
