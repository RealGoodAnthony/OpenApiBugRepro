using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using OpenApiBugRepro;

[assembly: FunctionsStartup(typeof(Startup))]

namespace OpenApiBugRepro
{
    public sealed class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
        }
    }
}
