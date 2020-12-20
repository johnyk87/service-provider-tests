namespace ServiceProviderTests
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public static class Program
    {
        public static void Main()
        {
            var provider = new ServiceCollection()
                .AddTransient<Transient>()
                .AddScoped<Scoped>()
                .AddSingleton<Singleton>()
                .AddSingleton<SingletonResolver>()
                .AddScoped<ScopedResolver>()
                .BuildServiceProvider();
            
            var instance = new object();
            
            Console.WriteLine("Singleton provider = " + provider.GetHashCode());

            instance = provider.GetRequiredService<Transient>();
            Console.WriteLine("Transient instance = " + instance.GetHashCode());

            instance = provider.GetRequiredService<Scoped>();
            Console.WriteLine("Scoped instance = " + instance.GetHashCode());

            instance = provider.GetRequiredService<Singleton>();
            Console.WriteLine("Singleton instance = " + instance.GetHashCode());

            var singletonResolver = provider.GetRequiredService<SingletonResolver>();
            Console.WriteLine("Singleton resolver = " + singletonResolver.GetHashCode());
            Console.WriteLine("Singleton resolver provider = " + singletonResolver.GetProviderHashCode());
            
            singletonResolver = provider.GetRequiredService<SingletonResolver>();
            Console.WriteLine("Singleton resolver = " + singletonResolver.GetHashCode());
            Console.WriteLine("Singleton resolver provider = " + singletonResolver.GetProviderHashCode());

            instance = singletonResolver.Resolve<Transient>();
            Console.WriteLine("Transient instance = " + instance.GetHashCode());
            
            instance = singletonResolver.Resolve<Transient>();
            Console.WriteLine("Transient instance = " + instance.GetHashCode());

            instance = singletonResolver.Resolve<Scoped>();
            Console.WriteLine("Scoped instance = " + instance.GetHashCode());
            
            instance = singletonResolver.Resolve<Scoped>();
            Console.WriteLine("Scoped instance = " + instance.GetHashCode());

            instance = singletonResolver.Resolve<Singleton>();
            Console.WriteLine("Singleton instance = " + instance.GetHashCode());
            
            instance = singletonResolver.Resolve<Singleton>();
            Console.WriteLine("Singleton instance = " + instance.GetHashCode());

            Console.WriteLine();

            var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
            for (var i = 1; i <= 2; i++)
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    Console.WriteLine("Scope provider = " + scope.ServiceProvider.GetHashCode());

                    instance = scope.ServiceProvider.GetRequiredService<Transient>();
                    Console.WriteLine("Transient instance = " + instance.GetHashCode());

                    instance = scope.ServiceProvider.GetRequiredService<Scoped>();
                    Console.WriteLine("Scoped instance = " + instance.GetHashCode());

                    instance = scope.ServiceProvider.GetRequiredService<Singleton>();
                    Console.WriteLine("Singleton instance = " + instance.GetHashCode());

                    var scopedResolver = scope.ServiceProvider.GetRequiredService<ScopedResolver>();
                    Console.WriteLine("Scoped resolver = " + scopedResolver.GetHashCode());
                    Console.WriteLine("Scoped resolver provider = " + scopedResolver.GetProviderHashCode());

                    scopedResolver = scope.ServiceProvider.GetRequiredService<ScopedResolver>();
                    Console.WriteLine("Scoped resolver = " + scopedResolver.GetHashCode());
                    Console.WriteLine("Scoped resolver provider = " + scopedResolver.GetProviderHashCode());

                    instance = scopedResolver.Resolve<Transient>();
                    Console.WriteLine("Transient instance = " + instance.GetHashCode());
                    
                    instance = scopedResolver.Resolve<Transient>();
                    Console.WriteLine("Transient instance = " + instance.GetHashCode());

                    instance = scopedResolver.Resolve<Scoped>();
                    Console.WriteLine("Scoped instance = " + instance.GetHashCode());
                    
                    instance = scopedResolver.Resolve<Scoped>();
                    Console.WriteLine("Scoped instance = " + instance.GetHashCode());

                    instance = scopedResolver.Resolve<Singleton>();
                    Console.WriteLine("Singleton instance = " + instance.GetHashCode());
                    
                    instance = scopedResolver.Resolve<Singleton>();
                    Console.WriteLine("Singleton instance = " + instance.GetHashCode());
                    
                    Console.WriteLine();
                }
            }
        }
    }

    public class Transient
    {
    }

    public class Scoped
    {
    }

    public class Singleton
    {
    }

    public abstract class Resolver
    {
        private readonly IServiceProvider provider;

        protected Resolver(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public T Resolve<T>()
        {
            return this.provider.GetRequiredService<T>();
        }

        public int GetProviderHashCode()
        {
            return this.provider.GetHashCode();
        }
    }

    public class SingletonResolver : Resolver
    {
        public SingletonResolver(IServiceProvider provider) : base(provider) { }
    }

    public class ScopedResolver : Resolver
    {
        public ScopedResolver(IServiceProvider provider) : base(provider) { }
    }
}
