using UcomGridView.Data.Classes;
using UcomGridView.Data.Interfaces;
using UcomGridView.Infrastructure.Classes;
using UcomGridView.Infrastructure.Interfaces;

namespace UcomGridView.Web
{
    public static class DependencyInjection
    {
        public static void AddDI(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();

            services.AddAutoMapper(typeof(AutoMapperProfile));
        }
    }
}
