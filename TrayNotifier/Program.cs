namespace TrayNotifier
{
    using System;
    using System.Windows.Forms;
    using Business;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.Resolvers.SpecializedResolvers;
    using Castle.Windsor;
    using Domain;
    using Domain.DependencyRegistration;
    using OpenFileSystem.IO;
    using OpenFileSystem.IO.FileSystems.Local;

    public static class Program
    {
        private static IWindsorContainer _container;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SetupContainer();
            var notificationSystems = _container.ResolveAll<AbstractNotificationSystem>();
            var window = new NotificationWindow(notificationSystems);
            Application.Run(window);
            _container.Release(notificationSystems);
        }

        private static void SetupContainer()
        {
            _container = new WindsorContainer();
            _container.Kernel.Resolver.AddSubResolver(new ArrayResolver(_container.Kernel));
            _container.Register(Component.For<AbstractConfigurationDetails>().ImplementedBy<ConfigurationBasedNotificationConfigurationDetails>());
            _container.Register(Component.For<IFileSystem>().Instance(LocalFileSystem.Instance));
            _container.RegisterAllPlugins(_container.Resolve<AbstractConfigurationDetails>());
        }
    }
}