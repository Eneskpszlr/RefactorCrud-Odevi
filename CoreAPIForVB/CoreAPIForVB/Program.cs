
using CoreAPIForVB.Models1.ContextClasses;
using Microsoft.EntityFrameworkCore;

namespace CoreAPIForVB
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

            builder.Services.AddDbContext<MyContext>(x => x.UseSqlServer
            (builder.Configuration.GetConnectionString("MyConnection")).UseLazyLoadingProxies());

            builder.Services.AddCors(); // Cross origins resource server policy'sini açar
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

            app.MapControllers();

            app.Run();
        }
    }
}
