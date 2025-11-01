using Microsoft.EntityFrameworkCore;
using WorkingMVC.Data;
using WorkingMVC.Data.Entitys;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MyAppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scoped = app.Services.CreateScope())
{
    var dbContext = scoped.ServiceProvider.GetRequiredService<MyAppDbContext>();
    dbContext.Database.Migrate();
    if (!dbContext.Categories.Any())
    {
        var categories = new List<CategoryEntity>
        {
            new CategoryEntity { Name = "Напої безалкогольні", Image = "https://tykyiv.com/media/GettyImages-525338134.jpg" },
            new CategoryEntity { Name = "Овочі та фрукти", Image = "https://ecosmak.com.ua/image/cache/catalog/blog/frukty-ta-ovochi/frukty-i-ovoshi-1200x700.jpeg" },
            new CategoryEntity { Name = "Молочні продукти", Image = "https://kurs.if.ua/wp-content/uploads/2023/04/image1-1024x682.png" },
        };
        dbContext.Categories.AddRange(categories);
        dbContext.SaveChanges();
    }
}


app.Run();
