namespace TrayNotifier
{
    using System;
    using System.Windows.Forms;
    using Business;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.Resolvers.SpecializedResolvers;
    using Castle.Windsor;

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
            var assemblies = AllTypes.FromAssemblyInDirectory(new AssemblyFilter(AppDomain.CurrentDomain.BaseDirectory));
            _container.Register(assemblies.BasedOn<Form>().Configure(c => c.LifeStyle.Transient));
            _container.Register(assemblies.BasedOn<AbstractNotificationSystem>().WithService.Base().Configure(c => c.LifeStyle.Transient));
        }
    }
}