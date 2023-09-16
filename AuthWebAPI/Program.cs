using AuthWebAPI.Core.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MobileAppWebAPI.Context;
using MobileAppWebAPI.Services.Furnitures;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<MobileAppDBContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("MobileAppCon"));
});

builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<MobileAppDBContext>();

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

builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
