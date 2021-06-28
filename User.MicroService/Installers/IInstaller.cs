using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace User.MicroService.Installers
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services,IConfiguration configuration);
    }
}
