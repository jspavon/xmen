
using xmen.proxy.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace xmen.proxy.ServiceCollections
{
    /// <summary>
    /// Class Extensions.
    /// </summary>
    /// <remarks>Jhon Steven Pav�n Bedoya</remarks>
    public static class Extensions
    {
        /// <summary>
        /// Adds the proxy HTTP.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>IServiceCollection.</returns>
        /// <remarks>Jhon Steven Pav�n Bedoya</remarks>
        public static IServiceCollection AddProxyHttp(this IServiceCollection services)
        {
            services.AddSingleton<IHttpClientHelper, HttpClientHelper>();
            return services;
        }
    }
}
