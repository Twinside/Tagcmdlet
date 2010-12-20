using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Management.Automation;
using System.IO;

namespace Tagcmdlet.Command
{
    [Cmdlet(VerbsCommon.Get, "Tags")]
    public class GetTagCommand : PSCmdlet
    {
        bool useWildcard;
        private string[] fileNames;
        private System.IO.FileInfo[] filesInfo;

        [Parameter( Position=0
                  , Mandatory=false
                  , ValueFromPipeline=true
                  , ValueFromPipelineByPropertyName=true
                  , HelpMessage="Filenames to obtain tags from" )]
        [Alias("PSPath")]
        public string[] Path
        {
            get { return fileNames; }
            set { fileNames = value; }
        }

        [Parameter( ValueFromPipeline=true
                  , ValueFromPipelineByPropertyName=true
                  , HelpMessage="File to obtain tags from")]
        public System.IO.FileInfo[]   Files
        {
            get { return filesInfo; }
            set { filesInfo = value; }
        }

        private void ExtractTags(Object obj, string filename)
        {
            try {
                var f = TagLib.File.Create(filename);
                WriteObject(new TagSet(filename,f.Tag));
            }
            catch (TagLib.UnsupportedFormatException e)
            {
                if (!useWildcard)
                {
                    var err = new ErrorRecord(e, "The format is unsupported by get-tags", ErrorCategory.InvalidArgument, obj);
                    WriteError(err);
                }
            }
            catch (System.IO.FileNotFoundException e)
            {
                var err = new ErrorRecord(e, "File doesn't exist", ErrorCategory.ResourceUnavailable, obj);
                WriteError(err);
            }
            catch (TagLib.CorruptFileException e)
            {
                var err = new ErrorRecord(e, "File is corrupted", ErrorCategory.InvalidData, obj);
                WriteError(err);
            }
            catch (Exception e)
            {
                WriteDebug(e.ToString());
            }
        }

        /// <summary>
        /// Where to do stuff in the cmdlet
        /// </summary>
        protected override void ProcessRecord()
        {
            if (filesInfo != null)
            {
                foreach (System.IO.FileInfo f in filesInfo)
                    ExtractTags(f, f.FullName);
            }
            else
            {
                foreach (string s in fileNames)
                {
                    if (WildcardPattern.ContainsWildcardCharacters(s))
                    {
                        useWildcard = true;
                        ProviderInfo pInfo;
                        var resolvedPaths = SessionState.Path.GetResolvedProviderPathFromPSPath(s, out pInfo);

                        foreach (var file in resolvedPaths)
                            ExtractTags(file, file);
                    }
                    else ExtractTags(s, GetUnresolvedProviderPathFromPSPath(s));
                }
            }
        }
    }
}
