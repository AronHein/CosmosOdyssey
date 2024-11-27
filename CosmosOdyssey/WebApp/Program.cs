using DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

// TODO: change before deployment
builder.Environment.EnvironmentName = "Development";

var connectionString = builder.Configuration.GetConnectionString("MongoDb") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var databaseName = builder.Configuration["DatabaseName"] ??
                   throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddSingleton(new MongoDbContext(connectionString, databaseName));
builder.Services.AddSingleton<PricelistService>();
builder.Services.AddSingleton<ReservationService>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();