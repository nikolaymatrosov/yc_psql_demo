using System;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using yc_psql_demo.Models;


namespace yc_psql_demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PostContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), b => b.RemoteCertificateValidationCallback(RemoteCertificateValidationCallback)));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        static bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain defaultChain, SslPolicyErrors defaultErrors)
        {
            string text1 = File.ReadAllText("CA1.pem");
            string text2 = File.ReadAllText("CA2.pem");

            X509Certificate2 ca1Cert = new X509Certificate2(Encoding.UTF8.GetBytes(text1));
            X509Certificate2 ca2Cert = new X509Certificate2(Encoding.UTF8.GetBytes(text2));

            X509Chain caCertChain = new X509Chain();
            caCertChain.ChainPolicy = new X509ChainPolicy()
            {
                RevocationMode = X509RevocationMode.NoCheck,
                RevocationFlag = X509RevocationFlag.EntireChain,
            };

            caCertChain.ChainPolicy.ExtraStore.Add(ca1Cert);
            caCertChain.ChainPolicy.ExtraStore.Add(ca2Cert);

            X509Certificate2 serverCert = new X509Certificate2(certificate);

            caCertChain.Build(serverCert);

            if (caCertChain.ChainStatus.Length == 0)
            {
                // No errors
                return true;
            }

            foreach (X509ChainStatus status in caCertChain.ChainStatus)
            {
                // Check if we got any errors other than UntrustedRoot (which we will always get if we don't install the CA cert to the system store)
                if (status.Status != X509ChainStatusFlags.UntrustedRoot)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
