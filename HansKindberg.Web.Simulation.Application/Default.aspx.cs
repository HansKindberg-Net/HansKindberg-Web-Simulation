using System.Diagnostics.CodeAnalysis;
using HansKindberg.Web.Simulation.Application.Business.Web.Mvp.UI;
using HansKindberg.Web.Simulation.Application.Models;
using HansKindberg.Web.Simulation.Application.Views.Default;

namespace HansKindberg.Web.Simulation.Application
{
    [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
    public partial class Default : MvpPage<DefaultModel>, IDefaultView {}
}