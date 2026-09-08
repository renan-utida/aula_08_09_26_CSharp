using MyPDV.Database;
using Microsoft.EntityFrameworkCore;
using MyPDV.Services;

namespace MyPDV;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<IProdutoService, ProdutoService>();
        builder.Services.AddScoped<IPedidoService, PedidoService>();

        WebApplication app = builder.Build();

        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
