using System.Web.Security;
using HansKindberg.Web.Simulation.Applications.Logic.Fakes.Web.Security;
using HansKindberg.Web.Simulation.Applications.Logic.Net.Mail;
using HansKindberg.Web.Simulation.Applications.Logic.Web.Security;

namespace HansKindberg.Web.Simulation.Application
{
    public abstract class Registry : StructureMap.Configuration.DSL.Registry
    {
        #region Constructors

        protected Registry()
        {
            this.For<IAccountValidation>().Singleton().Use<AccountValidation>();
            this.For<IEmailValidator>().Singleton().Use<EmailValidator>();
            this.For<IFormsAuthentication>().Singleton().Use<FormsAuthenticationWrapper>();
            this.For<IMembershipProvider>().Singleton().Use<MembershipProviderWrapper>();
            this.For<IPrincipalRepository>().Singleton().Use<SimplePrincipalRepository>();
            this.For<MembershipProvider>().Singleton().Use(() => Membership.Provider);
        }

        #endregion
    }

    public class FakeRegistry : Registry
    {
        #region Constructors

        public FakeRegistry() {}

        #endregion
    }
}