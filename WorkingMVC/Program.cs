using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WorkingMVC.Data;
using WorkingMVC.Data.Entitys;
using WorkingMVC.Interfaces;
using WorkingMVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MyAppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddSingleton<IHiddenCategoryService, HiddenCategoryService>();
//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

//builder.Services.AddScoped<ICategoryService, CategoryService>();
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

string dirPath = builder.Configuration.GetValue<string>("DirPath")??"test";
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
    var dbContext = scoped.ServiceProvider.GetRequiredService<MyAppDbContext>();
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
}


app.Run();
