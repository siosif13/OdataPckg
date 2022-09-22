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
        //.Select().Expand() --> not supported with odataQueryOptions
        .AddRouteComponents(GetEdmModel())
        );

IEdmModel GetEdmModel()             // conventional
{
    var builder = new ODataConventionModelBuilder();

    builder.EntitySet<PostDto>("Posts");
    builder.EntitySet<BlogDto>("Blogs");


    return builder.GetEdmModel();
}
//IEdmModel GetEdmModel() //   unconventional
//{
//    var builder = new ODataModelBuilder();

//    //builder.EntitySet<BlogDto>("Blogs");

//    //builder.EntitySet<PostDto>("Posts");

//    //var post = builder.ComplexType<PostDto>();
//    //post.Property(p => p.Id);
//    //post.Property(p => p.Title);
//    //post.Property(p => p.Content);

//    //var blog = builder.ComplexType<BlogDto>();
//    //blog.Property(p => p.Id);
//    //blog.Property(p => p.Url);
//    //blog.HasMany(p => p.Posts).AutomaticallyExpand(false);

//    var post = builder.EntityType<PostDto>();
//    post.HasKey(p => p.Id);
//    post.Property(p => p.Title);
//    post.Property(p => p.Content);

//    var blog = builder.EntityType<BlogDto>();
//    blog.HasKey(p => p.Id);
//    blog.Property(p => p.Url);
//    blog.HasMany(p => p.Posts);
//    //.AutomaticallyExpand(false);

//    //var test = blog.HasMany(p => p.Posts).HasAutoExpand();

//    builder.EntitySet<BlogDto>("Blogs");
//    builder.EntitySet<PostDto>("Posts");

//    return builder.GetEdmModel();
//}

//IEdmModel GetEdmModel()               --- not working
//{
//    var model = new EdmModel();

//    EdmEntityType post = new EdmEntityType("dto", "Post");
//    post.AddKeys(post.AddStructuralProperty("Id", EdmPrimitiveTypeKind.Int32));
//    post.AddStructuralProperty("Title", EdmPrimitiveTypeKind.Int32);
//    post.AddStructuralProperty("Content", EdmPrimitiveTypeKind.Int32);
//    model.AddElement(post);

//    EdmEntityType blog = new EdmEntityType("dto", "Blog");
//    blog.AddKeys(blog.AddStructuralProperty("Id", EdmPrimitiveTypeKind.Int32));
//    blog.AddStructuralProperty("Url", EdmPrimitiveTypeKind.String);
//    model.AddElement(blog);

//    EdmNavigationProperty postsNavProp = blog.AddUnidirectionalNavigation(
//        new EdmNavigationPropertyInfo
//        {
//            Name = "Posts",
//            TargetMultiplicity = EdmMultiplicity.Many,
//            Target = post
//        });

//    EdmEntityContainer container = new EdmEntityContainer("dto", "Container");
//    EdmEntitySet posts = container.AddEntitySet("Posts", post);
//    EdmEntitySet blogs = container.AddEntitySet("Blogs", blog);

//    blogs.AddNavigationTarget(postsNavProp, posts);

//    model.AddElement(container);

//    return model;
//}

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
