using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ninject;
using Ninject.Parameters;
using Ninject.Syntax;
using System.Configuration;
using EssentialTools.Models;

namespace EssentialTools.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernal;

        public NinjectDependencyResolver()
        {
            kernal = new StandardKernel();
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernal.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernal.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernal.Bind<IValueCalculator>().To<LinqValueCalculator>();
            kernal.Bind<IDiscountHelper>()
                .To<DefaultDiscountHelper>().WithConstructorArgument("discountParam", 50M);
            kernal.Bind<IDiscountHelper>().To<FlexibleDiscountHelper>()
                .WhenInjectedInto<LinqValueCalculator>();
        }
    }
}