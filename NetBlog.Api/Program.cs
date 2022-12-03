using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetBlog.Api.Config;
using NetBlog.Data;
using NetBlog.Models;
using NetBlog.Repositories.Implementations;
using NetBlog.Repositories.Interfaces;
using NetBlog.Services.Implementations;
using NetBlog.Services.Interfaces;
using NetBlog.Utilities.Implementations;
using NetBlog.Utilities.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();

    builder.Services.AddDbContext<ApplicationDbContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedEmail = false)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

    builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(jwt =>
    {
        var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value);
        jwt.SaveToken = true;
        jwt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = false,
            ValidateLifetime = true,
        };
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IDbInitializer, DbInitializer>();

    builder.Services.AddCors();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

    DataSeeding();

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

    void DataSeeding()
    {
        using (var scope = app.Services.CreateScope())
        {
            var DbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            DbInitializer.Initialize();
        }
    }
}
