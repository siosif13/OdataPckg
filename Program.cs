using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using OdataPckg.DAL;
using OdataPckg.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddOData(options => 
    options
        .Filter()
        .OrderBy()
        .SkipToken()
        .SetMaxTop(null)
        .Count()
        .Select()
        .Expand()
        .AddRouteComponents(GetEdmModel())
        );

IEdmModel GetEdmModel()
{
    var odataBuilder = new ODataConventionModelBuilder();

    odataBuilder.EntitySet<Blog>("Blogs");

    odataBuilder.EntitySet<Post>("Posts");

    return odataBuilder.GetEdmModel();
}


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Depinject
builder.Services.AddScoped<IBlogService, BlogService>();

builder.Services.AddDbContext<BloggingContext>(
    options => options.UseSqlServer(
        $"Data Source=localhost;Database=BlogTest;Persist Security Info=True;User ID=sa;Password=mOrc00vi!@#$"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
