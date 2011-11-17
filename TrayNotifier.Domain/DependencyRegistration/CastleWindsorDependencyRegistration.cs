using Castle.MicroKernel.Registration;
using Castle.Windsor;
using TrayNotifier.Business;

namespace TrayNotifier.Domain.DependencyRegistration
{
    public static class CastleWindsorDependencyRegistration
    {
        public static void RegisterAllPlugins(this IWindsorContainer container, AbstractConfigurationDetails configuration)
        {
            var assemblies = AllTypes.FromAssemblyInDirectory(new AssemblyFilter(configuration.PluginDirectory));
            container.Register(assemblies.BasedOn<INotificationRegistration>().Configure(c => c.LifeStyle.Transient));

            var registrations = container.ResolveAll<INotificationRegistration>();
            foreach (var registration in registrations)
                foreach (var component in registration.RegisterComponents())
                    container.Register(Component.For(component.GetType()).Instance(component));

            container.Register(assemblies.BasedOn<AbstractNotificationSystem>().Configure(c => c.LifeStyle.Transient));
        }
    }
}