using System;
using System.IO;
using System.Text;
using System.Web.Hosting;

namespace HansKindberg.Web.Simulation.Hosting
{
    public class SimulatedVirtualFile : VirtualFile
    {
        #region Fields

        private readonly string _content;
        private readonly Encoding _encoding;

        #endregion

        #region Constructors

        public SimulatedVirtualFile(string virtualPath, string fileContent, Encoding encoding) : base(virtualPath)
        {
            if(fileContent == null)
                throw new ArgumentNullException("fileContent");

            if(encoding == null)
                throw new ArgumentNullException("encoding");

            this._content = fileContent;
            this._encoding = encoding;
        }

        #endregion

        #region Properties

        public virtual string Content
        {
            get { return this._content; }
        }

        public virtual Encoding Encoding
        {
            get { return this._encoding; }
        }

        #endregion

        #region Methods

        public override Stream Open()
        {
            byte[] data = this.Encoding.GetBytes(this.Content);
            return new MemoryStream(data);
        }

        #endregion
    }
}