using System.IO.Abstractions;

namespace HansKindberg.Web.Simulation.Hosting
{
    public interface IFileTransferItem
    {
        #region Properties

        FileInfoBase Destination { get; }
        FileInfoBase Source { get; }

        #endregion
    }
}