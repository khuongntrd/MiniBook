using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace MiniBook
{
    public class ServiceLocator
    {
        public static ServiceLocator Instance { get; } = new ServiceLocator();

        private IContainer Container { get; set; }

        private ContainerBuilder _containerBuilder;

        public ServiceLocator()
        {
            _containerBuilder = new ContainerBuilder();
        }
        public bool Built { get; private set; }

        public void Build()
        {
            if (Built == false)
            {
                Container = _containerBuilder.Build();
                Built = true;
            }
        }

        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return Container.Resolve(type);
        }

        public void RegisterInstance<TInterface, TImplementation>(TImplementation instance)
           where TImplementation : class, TInterface
        {
            _containerBuilder.RegisterInstance(instance).As<TInterface>();
        }

        public void RegisterInstance<TInterface, TImplementation>() where TImplementation : TInterface
        {
            _containerBuilder.RegisterType<TImplementation>().As<TInterface>().SingleInstance();
        }

        public void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            _containerBuilder.RegisterType<TImplementation>().As<TInterface>().InstancePerLifetimeScope();
        }

        public void Register<T>() where T : class
        {
            _containerBuilder.RegisterType<T>()
                .InstancePerLifetimeScope();
        }
    }
}
