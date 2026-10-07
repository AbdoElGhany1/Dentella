using Dentella3.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace MainDentalla
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<Appdbcontext>(options =>
            { options.UseSqlServer(builder.Configuration.GetConnectionString("connect")); }
           );
            // builder.Services.AddAuthentication()
            //.AddFacebook(options =>
            //{
            //    options.AppId = "353423947549108";
            //    options.AppSecret = "f500f71942000de7c8b8af8c8b9eede1";
            //});
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(option =>
           {
               option.Password.RequireUppercase = false;
               option.Password.RequireNonAlphanumeric = false;
               option.Password.RequiredLength = 3;
               option.Password.RequireLowercase = false;
               option.Password.RequireDigit = false;

           })
           .AddEntityFrameworkStores<Appdbcontext>()
           .AddDefaultTokenProviders();

           builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme= JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata= false;
                
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("StrongAuthenticationKEY")),
                    ValidIssuer = "http://dentella.somee.com/",
                   // ValidAudience ="https://localhost:4200/"
                    
                };
            });
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = " API Title",
                    Description = "Your API Description",
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your vaild token in the next input below.\r\n\r\nExample: \"Bearer eyJhbGnmkkkkkkkkk\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference= new OpenApiReference
                            {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                            }

                        },
                        new string[]
                        {

                        }

                }});
            });
            builder.Services.AddCors(corsoptions =>
            {
                corsoptions.AddPolicy("MyPolicy", configurePolicy =>
                {
                    configurePolicy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}
            app.UseStaticFiles();
            app.UseCors("MyPolicy");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}