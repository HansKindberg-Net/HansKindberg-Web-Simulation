using System;
using System.Diagnostics.CodeAnalysis;
using HansKindberg.Web.Simulation.Applications.Logic.Web.Security;

namespace HansKindberg.Web.Simulation.Applications.Logic.Fakes.Web.Security
{
    public abstract class SimplePrincipal : IPrincipal
    {
        #region Fields

        private readonly Guid _guid = Guid.NewGuid();
        private readonly string _name;

        #endregion

        #region Constructors

        protected SimplePrincipal(string name) : this(Guid.NewGuid(), name) {}

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "guid")]
        protected SimplePrincipal(Guid guid, string name)
        {
            if(guid == null)
                throw new ArgumentNullException("guid");

            if(guid == Guid.Empty)
                throw new ArgumentException("The guid can not be empty.", "guid");

            if(name == null)
                throw new ArgumentNullException("name");

            if(name.Trim().Length == 0)
                throw new ArgumentException("The name can not be empty.", "name");

            this._name = name;
        }

        #endregion

        #region Properties

        public virtual Guid Guid
        {
            get { return this._guid; }
        }

        public virtual string Name
        {
            get { return this._name; }
        }

        #endregion
    }
}