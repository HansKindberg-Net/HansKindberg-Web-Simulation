using System.Web.Mvc;
using HansKindberg.Web.Simulation.Applications.Logic.IoC.StructureMap;
using StructureMap;

namespace HansKindberg.Web.Mvc.Simulation.Application.Business.Web.Mvc
{
    public class MvcDependencyResolver : StructureMapDependencyResolver, IDependencyResolver
    {
        #region Constructors

        public MvcDependencyResolver(IContainer container) : base(container) {}

        #endregion
    }
}