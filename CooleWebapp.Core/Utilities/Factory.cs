using Microsoft.Extensions.DependencyInjection;

namespace CooleWebapp.Core.Utilities;

public interface IFactory<out T>
{
  T Create();
}

internal class Factory<T> : IFactory<T>
{
  private readonly Func<T> _factoryFunc;
  public Factory(Func<T> factoryFunc)
  {
    _factoryFunc = factoryFunc;
  }

  public T Create()
  {
    return _factoryFunc();
  }
}

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddScopedFactory<TService, TImplementation>(
    this IServiceCollection services)
    where TService : class
    where TImplementation : class, TService
  {
    services.AddTransient<TService, TImplementation>();
    services.AddScoped<Func<TService>>(x => () => x.GetRequiredService<TService>());
    services.AddScoped<IFactory<TService>, Factory<TService>>();
    return services;
  }
  public static IServiceCollection AddScopedFactory<TService>(
    this IServiceCollection services)
    where TService : class
  {
    return services.AddScopedFactory<TService, TService>();
  }

  public static IServiceCollection AddSingletonFactory<TService, TImplementation>(
    this IServiceCollection services)
    where TService : class
    where TImplementation : class, TService
  {
    services.AddTransient<TService, TImplementation>();
    services.AddSingleton<Func<TService>>(x => () => x.GetRequiredService<TService>());
    services.AddSingleton<IFactory<TService>, Factory<TService>>();
    return services;
  }
  public static IServiceCollection AddSingletonFactory<TService>(
    this IServiceCollection services)
    where TService : class
  {
    return services.AddSingletonFactory<TService, TService>();
  }
}
