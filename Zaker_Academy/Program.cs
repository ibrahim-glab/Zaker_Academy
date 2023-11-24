using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Unicode;
using Zaker_Academy.core.Interfaces;
using Zaker_Academy.infrastructure;
using Zaker_Academy.infrastructure.Entities;
using Zaker_Academy.infrastructure.Repository;
using Zaker_Academy.Service;
using Zaker_Academy.Service.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<JwtHelper>(builder.Configuration.GetSection("JWT"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.RequireHttpsMetadata = false;
    opt.SaveToken = false;
    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
    };
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddIdentityCore<Student>().AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentityCore<applicationUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentityCore<Instructor>().AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddServices();
builder.Services.Services();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();