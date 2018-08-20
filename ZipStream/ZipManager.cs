using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZipStream
{
    public class ZipManager
    {
        public const string Zip = "-zip";
        public const string UnZip = "-unzip";
        public const string Type = "-gz";

        private CompressionAction _action = CompressionAction.Zip;
        private CompressionAlg _compressionAlg = CompressionAlg.Gz;

        public string SourceFile { get; set; }
        public string DestFile { get; set; }

        public ZipManager()
        {
            SourceFile = string.Empty;
            DestFile = string.Empty;
            ParseCommandline(Environment.GetCommandLineArgs());
        }

        public void Execute()
        {
            if (!string.IsNullOrEmpty(SourceFile) && !string.IsNullOrEmpty(DestFile))
            {
                var stream = new ZipStream(SourceFile, DestFile);

                if(_action == CompressionAction.Zip)
                {
                    stream.Compress();
                }
                else if(_action == CompressionAction.UnZip)
                {
                    stream.Decompress();
                }
            }
        }

        private void ParseCommandline(IEnumerable<string> commandlineArgs)
        {
            var enumurator = commandlineArgs.GetEnumerator();
            enumurator.Reset();
            //ignore first argument
            enumurator.MoveNext();
            while (enumurator.MoveNext())
            {
                string lowerCaseToken = enumurator.Current.ToLower();
                switch (lowerCaseToken)
                {
                    case Zip:
                        break;
                    case UnZip:
                        break;
                    case Type:
                        {
                        }
                        break;
                    default:
                        {
                            if(string.IsNullOrEmpty(SourceFile))
                            {
                                SourceFile = Environment.ExpandEnvironmentVariables(enumurator.Current);
                            }
                            else if(string.IsNullOrEmpty(DestFile))
                            {
                                DestFile = Environment.ExpandEnvironmentVariables(enumurator.Current);
                            }
                        }
                        break;
                }
            }

            if (string.IsNullOrEmpty(SourceFile) || string.IsNullOrEmpty(DestFile))
            {
                MessageBox.Show("invalid options", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
