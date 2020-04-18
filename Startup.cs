using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jering.Javascript.NodeJS;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using RazorApp.Services;


namespace RazorApp
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this._env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<INewsService, NewsService>();
            services.AddNodeJS();
            services.Configure<OutOfProcessNodeJSServiceOptions>(options =>
            {
                options.Concurrency = Concurrency.MultiProcess;
                options.ConcurrencyDegree = 3; //Not to have to many processes and also round robin is making n first requests slow
                options.EnableFileWatching = true;
            });
            services.AddControllersWithViews(o =>
            {
                o.RespectBrowserAcceptHeader = true;
            })
                .AddXmlSerializerFormatters()
                .AddXmlDataContractSerializerFormatters();

            services.AddSpaStaticFiles(configuration => // toto je vlastne to iste ako UseStaticFiles dole zakomentovane
            {
                configuration.RootPath = _env.IsDevelopment() ? "ClientApp/build" : "ClientApp/build/public";
            });

            services.ConfigureOptions<ReactHtmlOutputConfigureOptions>();



            // services.AddJsEngineSwitcher(options =>
            //     options.DefaultEngineName = ChakraCoreJsEngine.EngineName
            // )
            //     .AddChakraCore();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStaticFiles();
            app.UseSpaStaticFiles();


            // app.UseStaticFiles(new StaticFileOptions
            // {
            //     FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "ClientApp/build/static")),
            //     RequestPath = "/static"
            // });


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    public class ReactHtmlOutputFormatter : TextOutputFormatter
    {
        INodeJSService _nodeJSService;
        public ReactHtmlOutputFormatter(INodeJSService nodeJSService)
        {
           _nodeJSService = nodeJSService ?? throw new ArgumentNullException(nameof(nodeJSService));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/html"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type type)
        {
            // TODO: is page model
            return true;
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();


            var result = await _nodeJSService.InvokeFromFileAsync<string>("./render.js", args: new object[] { context.HttpContext.Request.Path.ToString(), "http://localhost:5000", context.Object });
            buffer.Append(result);

            // buffer.AppendLine("Hello " + context.HttpContext.Request.Path);

            await response.WriteAsync(buffer.ToString());
        }
    }

    public class ReactHtmlOutputConfigureOptions : IConfigureOptions<MvcOptions>
    {
        private readonly INodeJSService _nodeJSService;

        public ReactHtmlOutputConfigureOptions(INodeJSService nodeJSService)
        {
            this._nodeJSService = nodeJSService;
        }
        
        public void Configure(MvcOptions options)
        {
            options.OutputFormatters.Add(new ReactHtmlOutputFormatter(_nodeJSService));
        }
    }

    //    public static class ReactHtmlOutputBuilderExtensions
    // {
    //     public static IMvcBuilder AddCustomOutputFormatter(this IMvcBuilder builder)
    //     {
    //         if (builder == null)
    //         {
    //             throw new ArgumentNullException(nameof(builder));
    //         }


    //         builder.Services.ConfigureNamedOptions
    //         .TryAddEnumerable(
    //             ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, CustomOutputFormatterConfigureOptions>());

    //         return builder;
    //     }
    // }
}