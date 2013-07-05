namespace HansKindberg.Web.Simulation.Hosting
{
    public class ApplicationHostFactory : IApplicationHostFactory
    {
        //public virtual ApplicationHost Create(Type applicationHostType, string virtualPath, string physicalDirectoryPath)
        //{
        //    if(applicationHostType == null)
        //        throw new ArgumentNullException("applicationHostType");
        //    if(!typeof(ApplicationHost).IsAssignableFrom(applicationHostType))
        //        throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The type \"{0}\" must inherit from \"{1}\".", applicationHostType, typeof(ApplicationHost)));
        //    return (ApplicationHost) System.Web.Hosting.ApplicationHost.CreateApplicationHost(applicationHostType, virtualPath, physicalDirectoryPath);
        //}

        #region Methods

        public virtual TApplicationHost Create<TApplicationHost, TRequestResult>(string virtualPath, string physicalDirectoryPath) where TApplicationHost : ApplicationHost<TRequestResult> where TRequestResult : RequestResult, new()
        {
            return (TApplicationHost) System.Web.Hosting.ApplicationHost.CreateApplicationHost(typeof(TApplicationHost), virtualPath, physicalDirectoryPath);
        }

        #endregion
    }
}