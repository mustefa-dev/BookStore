using BookStore.DATA;
using BookStore.Helpers;
using BookStore.Interface;
using BookStore.Respository;
using BookStore.Services;
using Microsoft.EntityFrameworkCore;
using BookStore.Repository;

namespace BookStore.Extensions;

public static class ApplicationServicesExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<DataContext>(
            options => options.UseNpgsql(config.GetConnectionString("DefaultConnection")));
        services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        services.AddScoped<IUserService, UserService>();
        // here to add
services.AddScoped<ICartServices, CartServices>();
services.AddScoped<ICartProductServices, CartProductServices>();
services.AddScoped<ICategoryServices, CategoryServices>();
services.AddScoped<IBookServices, BookServices>();

        services.AddScoped<IFileService, FileService>();
        services.AddHttpClient();
        





        return services;
    }
}
