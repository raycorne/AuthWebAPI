using AuthWebAPI.Core.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MobileAppWebAPI.Context;
using MobileAppWebAPI.Services.FurnitureCategories;
using MobileAppWebAPI.Services.FurnitureImages;
using MobileAppWebAPI.Services.Furnitures;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<MobileAppDBContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("MobileAppCon") ??
		throw new InvalidOperationException("Database connection string is not found"));
});

builder.Services.AddIdentity<User, IdentityRole>(
	options =>
	{
		options.Password.RequiredLength = 8;
		options.Password.RequireDigit = false;
		options.Password.RequireLowercase = false;
		options.Password.RequireUppercase = false;
		options.Password.RequireNonAlphanumeric = false;
	}).AddEntityFrameworkStores<MobileAppDBContext>();

builder.Services.AddAuthentication(f =>
{
	f.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(k =>
{
	var Key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
	k.SaveToken = true;
	k.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["JWT:Issuer"],
		ValidAudience = builder.Configuration["JWT:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Key),
		ClockSkew = TimeSpan.Zero
	};
});

builder.Services.AddScoped<IFurnitureRepository, FurnitureRepository>();
builder.Services.AddScoped<IFurnitureImageRepository, FurnitureImageRepository>();
builder.Services.AddScoped<IFurnitureCategoryRepository, FurnitureCategoryRepository>();

//Add Authentication in Swagger
builder.Services.AddSwaggerGen(swagger =>
{
	swagger.SwaggerDoc("v1",
					   new OpenApiInfo
					   {
						   Title = "API Title",
						   Version = "V1",
						   Description = "API Description"
					   });

	var securitySchema = new OpenApiSecurityScheme
	{
		Description = "Authorization header using the Bearer scheme. Example \"Authorization: Bearer {token}\"",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.Http,
		Scheme = "Bearer",
		Reference = new OpenApiReference
		{
			Type = ReferenceType.SecurityScheme,
			Id = "Bearer"
		}
	};
	swagger.AddSecurityDefinition(securitySchema.Reference.Id, securitySchema);
	swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{securitySchema,Array.Empty<string>() }
	});
});

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
