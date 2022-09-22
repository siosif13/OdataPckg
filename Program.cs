using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using OdataPckg.DAL;
using OdataPckg.DTO;
using OdataPckg.Mapper;
using OdataPckg.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddOData(options => 
    options
        .Filter()
        .OrderBy()
        .SkipToken()
        .SetMaxTop(100)
        .Count()
        .AddRouteComponents(GetEdmModel())
        );

IEdmModel GetEdmModel()             // conventional
{
    var builder = new ODataConventionModelBuilder();

    builder.EntitySet<PostDto>("Posts");
    builder.EntitySet<BlogDto>("Blogs");

    return builder.GetEdmModel();
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Depinject
builder.Services.AddScoped<IBlogService, BlogService>();

builder.Services.AddDbContext<BloggingContext>(
    options => options.UseSqlServer(
        $"Data Source=localhost;Database=BlogTest;Persist Security Info=True;User ID=sa;Password=mOrc00vi!@#$"));

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MapperProfile());
});

//mapper
builder.Services.AddSingleton(
    new MapperConfiguration(cfg =>
    {
        cfg.AddProfile(new MapperProfile());
        cfg.AddExpressionMapping();             // ---> used for expression mapping
    })
    .CreateMapper()
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseODataRouteDebug(); // remove it when depoly to product

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
