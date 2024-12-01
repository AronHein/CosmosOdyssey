using DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

builder.Configuration.AddEnvironmentVariables();
var connectionString = builder.Configuration["MONGODB_CONNECTION_STRING"] ??
                       builder.Configuration.GetConnectionString("MongoDb") ??
                       throw new InvalidOperationException("Connection string 'MONGODB_CONNECTION_STRING' not found.");
var databaseName = builder.Configuration["MONGODB_DATABASE_NAME"] ??
                   builder.Configuration["DatabaseName"] ??
                   throw new InvalidOperationException("Database name 'MONGODB_DATABASE_NAME' not found.");

builder.Services.AddSingleton(new MongoDbContext(connectionString, databaseName));
builder.Services.AddSingleton<PricelistService>();
builder.Services.AddSingleton<ReservationService>();


var app = builder.Build();


app.UseExceptionHandler("/Error");
app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();