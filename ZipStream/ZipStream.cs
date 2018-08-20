using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace ZipStream
{
    public class ZipStream
    {
        //We pick a value that is the largest multiple of 4096 that is still smaller than the large object heap threshold (85K).
        // The CopyTo/CopyToAsync buffer is short-lived and is likely to be collected at Gen0, and it offers a significant
        // improvement in Copy performance.
        private const int _DefaultCopyBufferSize = 81920;

        private string _originalFile;
        private string _newFile;

        public ZipStream(string originalFile, string newFile)
        {
            _originalFile = originalFile;
            _newFile = newFile;
        }

        public void Compress()
        {
            if(File.Exists(_newFile))
            {
                try { File.Delete(_newFile); } catch { }
            }

            using (FileStream originalFileStream = new FileStream(_originalFile, FileMode.Open,FileAccess.Read,FileShare.ReadWrite))
            {
                using (FileStream compressedFileStream = new FileStream(_newFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (GZipStream compressionStream = new GZipStream(compressedFileStream,CompressionMode.Compress))
                    {
                        byte[] buffer = new byte[_DefaultCopyBufferSize];
                        int read;
                        while ((read = originalFileStream.Read(buffer, 0, buffer.Length)) != 0)
                            compressionStream.Write(buffer, 0, read);
                    }
                }
            }
        }

        public void Decompress()
        {
            if (File.Exists(_newFile))
            {
                try { File.Delete(_newFile); } catch { }
            }

            using (FileStream originalFileStream = new FileStream(_originalFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (FileStream decompressedFileStream = new FileStream(_newFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        byte[] buffer = new byte[_DefaultCopyBufferSize];
                        int read;
                        while ((read = decompressionStream.Read(buffer, 0, buffer.Length)) != 0)
                            decompressedFileStream.Write(buffer, 0, read);
                    }
                }
            }
        }
    }
}
