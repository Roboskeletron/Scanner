using System;
using System.Collections.Generic;
using System.Text;

namespace Scanner.Services
{
    public interface IFileSystem
    {
        void SavePdf(byte[] data);
    }
}
