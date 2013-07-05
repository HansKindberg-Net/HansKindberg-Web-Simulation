using System.Diagnostics.CodeAnalysis;

namespace HansKindberg.Web.Simulation.Hosting
{
    public interface IApplicationHostFactory
    {
        //ApplicationHost<TRequestResult> Create(Type applicationHostType, string virtualPath, string physicalDirectoryPath);

        #region Methods

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        TApplicationHost Create<TApplicationHost, TRequestResult>(string virtualPath, string physicalDirectoryPath) where TApplicationHost : ApplicationHost<TRequestResult> where TRequestResult : RequestResult, new();

        #endregion
    }
}