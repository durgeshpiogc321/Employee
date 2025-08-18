using Repositories.IRepository;
using Repositories.Repository;
using Services.IServices;
using Services.Services;

namespace Employee.IOC
{
    public static class DependencyContainer
    {
        public static void ConfigureDependencyContainer(this IServiceCollection services,IConfiguration configuration) 
        {
            services.RegisterServices();
            services.RegisterRepositories(configuration);
        }   

        private static void RegisterServices(this IServiceCollection services) 
        {
            services.AddScoped<IEmployeeService, EmployeeService>();
        }

        private static void RegisterRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
        }
    }
}
