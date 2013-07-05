namespace HansKindberg.Web.Simulation.Hosting
{
    public interface IFileTransfer
    {
        #region Methods

        void AddFile(string sourceFilePath);
        void AddFile(string sourceFilePath, string destinationPath);
        void Reset();
        void Transfer();

        #endregion
    }
}