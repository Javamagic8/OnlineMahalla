using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineMahalla.Common.Model;
using OnlineMahalla.Common.Model.Interface;
using OnlineMahalla.Data.Model;
using OnlineMahalla.Data.Repository.SqlServer;
using OnlineMahalla.Web.MVCClient.Extentions;
using System.Globalization;
using System.Reflection;

namespace OnlineMahalla.Web.MVCClient
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            _env = env;
        }

        public IWebHostEnvironment _env { get; }
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            var connection = Configuration.GetSection("DefaultConnection");
            services.Configure<DbConfiguration>(connection);

            services.AddTransient<IDataRepository, DataRepository>();

            services.AddDbContext<DataProtectionContext>(
                options => options.UseSqlServer(connection.Get<DbConfiguration>().ConnectionString), ServiceLifetime.Transient);
            if (_env.EnvironmentName == "Production")
                services.AddDataProtection()
                .SetApplicationName("SharedCookieApp")
                .PersistKeysToDbContext<DataProtectionContext>();

            var swaggerInfo = Configuration.GetSection("Swagger").Get<SwaggerInfo>();
            services.AddSingleton(swaggerInfo);

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = ".AspNet.SharedCookie";
            });

            services.AddMvc(opt =>
            {
                opt.Filters.Add<OperationCancelledExceptionFilter>();
            }).AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                opt.SerializerSettings.DateFormatString = "dd.MM.yyyy";

            }).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, opts => { opts.ResourcesPath = "Resources"; })
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(SharedResource));
            });

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.Configure<RequestLocalizationOptions>(
                    opts =>
                    {
                        var supportedCultures = new List<CultureInfo>
                        {
                new CultureInfo("ru")
                        };
                        var supportedUICultures = new List<CultureInfo>
                        {
                new CultureInfo("ru"),
                new CultureInfo("uz-Cyrl")
                        };
                        opts.DefaultRequestCulture = new RequestCulture("ru");
                        // Formatting numbers, dates, etc.
                        opts.SupportedCultures = supportedUICultures;
                        // UI strings that we have localized.
                        opts.SupportedUICultures = supportedUICultures;
                    });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            Extentions.AppConst.AuthServiceUrl = Configuration.GetSection("UZASBOAuthService:url").Value; //e-imzo

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(o =>
                {
                    o.LoginPath = "/account/login";
                    o.AccessDeniedPath = "/account/login";
                }).AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["Tokens:Issuer"],
                        ValidAudience = Configuration["Tokens:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Constants.ApiGroup.DEFAULT, new OpenApiInfo { Title = "UZASBO", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });

                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.ConfigureSwaggerGen(options =>
            {
                options.CustomSchemaIds(x => x.FullName);
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            app.UseDeveloperExceptionPage();
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.ConfigureExceptions();

            var swaggerInfo = serviceProvider.GetService<SwaggerInfo>();
            if (swaggerInfo.Enabled)
            {
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"/swagger/{Constants.ApiGroup.DEFAULT}/swagger.json", "UZASBO v1");
                });

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineMahalla");
                });
            }

            System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
