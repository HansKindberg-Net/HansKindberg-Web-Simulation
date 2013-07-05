using System;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace HansKindberg.Web.Simulation.IO
{
    public class FileInfoEqualityComparer : IEqualityComparer<FileInfoBase>
    {
        #region Fields

        private readonly IEqualityComparer<string> _fullNameEqualityComparer;

        #endregion

        #region Constructors

        public FileInfoEqualityComparer(IEqualityComparer<string> fullNameEqualityComparer)
        {
            if(fullNameEqualityComparer == null)
                throw new ArgumentNullException("fullNameEqualityComparer");

            this._fullNameEqualityComparer = fullNameEqualityComparer;
        }

        #endregion

        #region Methods

        public virtual bool Equals(FileInfoBase x, FileInfoBase y)
        {
            if(x != null)
            {
                if(y == null)
                    return false;

                return this._fullNameEqualityComparer.Equals(x.FullName, y.FullName);
                ;
            }

            if(y != null)
                return false;

            return true;
        }

        public virtual int GetHashCode(FileInfoBase obj)
        {
            return obj != null ? obj.FullName.GetHashCode() : 0;
        }

        #endregion
    }
}