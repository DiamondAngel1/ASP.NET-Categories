using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System;
using WorkingMVC.Areas.Admin.Interfaces;
using WorkingMVC.Areas.Admin.Repositories;
using WorkingMVC.Areas.Admin.Services;
using WorkingMVC.Constants;
using WorkingMVC.Data;
using WorkingMVC.Data.Entitys;
using WorkingMVC.Data.Entitys.Identity;
using WorkingMVC.Interfaces;
using WorkingMVC.Mappers;
using WorkingMVC.Repositories;
using WorkingMVC.Services;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddDbContext<MyAppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(AdminCategoryProfile));
//builder.Services.AddAutoMapper(typeof(ProductProfile));
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ICategoryRepositoryUser, CategoryRepositoryUser>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<ICategoryServiceUser, CategoryServiceUser>();
builder.Services.AddScoped<IUserServices, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
//builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IHiddenCategoryService, HiddenCategoryService>();
builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
    .AddEntityFrameworkStores<MyAppDbContext>()
    //Використання нашої БД
    .AddDefaultTokenProviders();
//додає токени для підтвердження email, скидання пароля і т.д.
var app = builder.Build();

// Configure the HTTP request pipeline. if (!app.Environment.IsDevelopment()) { app.UseExceptionHandler("/Home/Error"); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts. app.UseHsts(); }
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapAreaControllerRoute(
    name: "MyAdminArea",
    areaName: "Admin",
    pattern: "admin/{controller=Dashboards}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Products",
    pattern: "{controller=Main}/{action=Products}/{id?}");
app.MapControllerRoute(
    name: "default", pattern: "{controller=Main}/{action=Index}/{id?}")
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
{ //Ініціалізація бази даних
    var dbContext = scoped.ServiceProvider.GetRequiredService<MyAppDbContext>();
    //Отримання сервісу для роботи з ролями
    var roleManager = scoped.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
    var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
    dbContext.Database.Migrate();

    //if (!dbContext.Categories.Any())
    //{
    //    //var categories = new List<CategoryEntity>
    //    //{
    //    //    new CategoryEntity { Name = "Напої безалкогольні", Image = "https://tykyiv.com/media/GettyImages-525338134.jpg" },
    //    //    new CategoryEntity { Name = "Овочі та фрукти", Image = "https://ecosmak.com.ua/image/cache/catalog/blog/frukty-ta-ovochi/frukty-i-ovoshi-1200x700.jpeg" },
    //    //    new CategoryEntity { Name = "Молочні продукти", Image = "https://kurs.if.ua/wp-content/uploads/2023/04/image1-1024x682.png" },
    //    //};
    //    //dbContext.Categories.AddRange(categories);
    //    //dbContext.SaveChanges();
    //}

    //Створення ролей в БД, якщо їх ще немає
    if (!dbContext.Roles.Any())
    {
        //перебираємо назви ролей
        foreach (var roleName in Roles.AllRoles)
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
    if (!dbContext.OrderStatuses.Any())
    {
        List<string> names = new List<string>() {
            "Нове", "Очікує оплати", "Оплачено",
            "В обробці", "Готується до відправки",
            "Відправлено", "У дорозі", "Доставлено",
            "Завершено", "Скасовано (вручну)", "Скасовано (автоматично)",
            "Повернення", "В обробці повернення" };

        var orderStatuses = names.Select(name => new OrderStatusEntity { Name = name }).ToList();

        await dbContext.OrderStatuses.AddRangeAsync(orderStatuses);
        await dbContext.SaveChangesAsync();
    }

    string seedDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "seed-images");
    string imageDir = Path.Combine(Directory.GetCurrentDirectory(), builder.Configuration.GetValue<string>("DirPath") ?? "images");
    Directory.CreateDirectory(imageDir);
    string ConvertToWebp(string file)
    {
        string src = Path.Combine(seedDir, file);
        string dst = Path.Combine(imageDir, Path.GetFileNameWithoutExtension(file) + ".webp");
        if (!File.Exists(dst))
        {
            using var img = Image.Load(src);
            img.Mutate(x => x.Resize(new ResizeOptions { Size = new(500, 500), Mode = ResizeMode.Max }));
            img.Save(dst, new WebpEncoder());
        }
        return Path.GetFileName(dst);
    }

    // Категорії
    if (!dbContext.Categories.Any())
    {
        dbContext.Categories.AddRange(
            new CategoryEntity { Name = "Парфюми", Image = ConvertToWebp("perfume.jpg") },
            new CategoryEntity { Name = "Козачок", Image = ConvertToWebp("kozachok.jpg") },
            new CategoryEntity { Name = "Подкрадулі", Image = ConvertToWebp("Shoes.jpg") }
        );
        await dbContext.SaveChangesAsync();
    }

    // Товари
    if (!dbContext.Products.Any())
    {
        int parfId = dbContext.Categories.First(c => c.Name == "Парфюми").Id;
        int podkId = dbContext.Categories.First(c => c.Name == "Подкрадулі").Id;

        var products = new[]
        {
        new { Name = "Gucci bloom", Desc = "Аромат Gucci", Price = 2500m, CatId = parfId, Images = new[] { "guccibloom1.jpg", "guccibloom2.jpg", "guccibloom3.jpg" } },
        new { Name = "Jean Paul Gaultier", Desc = "Французький стиль", Price = 2700m, CatId = parfId, Images = new[] { "jeanpaul2.jpg", "jeanpaul2.jpg"} },
        new { Name = "La vie est belle", Desc = "Життя прекрасне", Price = 2300m, CatId = parfId, Images = new[] { "lavie1.jpg", "lavie2.jpg", "lavie3.jpg", "lavie4.jpg" } },
        new { Name = "Versache eros", Desc = "Сила і пристрасть", Price = 2600m, CatId = parfId, Images = new[] { "versache1.jpg", "versache2.jpg", "versache3.jpg" } },
        new { Name = "Gaultier divine", Desc = "Божественний аромат", Price = 2400m, CatId = parfId, Images = new[] { "wom1.jpg", "wom2.jpg" } },
        new { Name = "Сімейні подкрадулі", Desc = "Для всієї родини", Price = 1800m, CatId = podkId, Images = new[] { "podkraduli1.jpg", "podkraduli2.jpg" } }
    };

        foreach (var p in products)
        {
            var prod = new ProductEntity { Name = p.Name, Description = p.Desc, Price = p.Price, CategoryId = p.CatId };
            dbContext.Products.Add(prod);
            await dbContext.SaveChangesAsync();

            short prio = 1;
            foreach (var img in p.Images)
                dbContext.ProductImages.Add(new ProductImageEntity { ProductId = prod.Id, Name = ConvertToWebp(img), Priority = prio++ });
        }

        await dbContext.SaveChangesAsync();
    }
    // Користувачі
    var adminUser = await userManager.FindByNameAsync("Admin");
    var regularUser = await userManager.FindByNameAsync("User");

    if (adminUser == null)
    {
        adminUser = new UserEntity
        {
            UserName = "Admin",
            Email = "admin@gmail.com",
            FirstName = "Admin",
            EmailConfirmed = true
        };
        await userManager.CreateAsync(adminUser, "admin123");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }

    if (regularUser == null)
    {
        regularUser = new UserEntity
        {
            UserName = "User",
            Email = "user@gmail.com",
            FirstName = "User",
            EmailConfirmed = true
        };
        await userManager.CreateAsync(regularUser, "user123");
        await userManager.AddToRoleAsync(regularUser, "User");
    }
    // Кошики
    if (!dbContext.Carts.Any())
    {
        var products = dbContext.Products.Take(3).ToList();

        dbContext.Carts.Add(new CartEntity
        {
            UserId = adminUser.Id,
            ProductId = products[0].Id,
            Quantity = 1
        });

        dbContext.Carts.AddRange(
            new CartEntity { UserId = regularUser.Id, ProductId = products[0].Id, Quantity = 2 },
            new CartEntity { UserId = regularUser.Id, ProductId = products[1].Id, Quantity = 2 },
            new CartEntity { UserId = regularUser.Id, ProductId = products[2].Id, Quantity = 1 }
        );

        await dbContext.SaveChangesAsync();
    }

    if (!dbContext.Orders.Any())
    {
        adminUser = await userManager.FindByNameAsync("Admin");
        regularUser = await userManager.FindByNameAsync("User");
        var statusId = dbContext.OrderStatuses.First(s => s.Name == "Нове").Id;
        var products = dbContext.Products.Take(2).ToList();

        // Замовлення для Admin
        var adminOrder = new OrderEntity
        {
            UserId = adminUser.Id,
            OrderStatusId = statusId,
            OrderItems = new List<OrderItemEntity>
        {
            new OrderItemEntity
            {
                ProductId = products[0].Id,
                Count = 1,
                PriceBuy = products[0].Price
            }
        }
        };
        dbContext.Orders.Add(adminOrder);

        // Замовлення для User
        var userOrder = new OrderEntity
        {
            UserId = regularUser.Id,
            OrderStatusId = statusId,
            OrderItems = new List<OrderItemEntity>
        {
            new OrderItemEntity
            {
                ProductId = products[0].Id,
                Count = 2,
                PriceBuy = products[0].Price
            },
            new OrderItemEntity
            {
                ProductId = products[1].Id,
                Count = 1,
                PriceBuy = products[1].Price
            }
        }
        };
        dbContext.Orders.Add(userOrder);

        await dbContext.SaveChangesAsync();
    }
    app.Run();
}