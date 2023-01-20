#region

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWF.DataAccess;
using RWF.Logic;
using RWF.Model.MapperProfiles;
using RWF.WebFramework.Middlewares;

#endregion

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDbContext<ForecastContext>(
    optionsBuilder => { optionsBuilder.UseInMemoryDatabase(databaseName: "ForecastDb"); });

builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<DataSeeder>();

var app = builder.Build();

app.UseSwaggerUI();

SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<DataSeeder>();
        service.Seed();
    }
}

app.UseSwagger(x => x.SerializeAsV2 = true);
app.UseMiddleware<LogMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGet("minimal/Provinces",
    ([FromServices] IProvinceLogic logic) =>
        logic.GetAllDetailed());
app.Run();