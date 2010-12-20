using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Management.Automation;
using System.IO;

namespace Tagcmdlet.Command
{
    [Cmdlet("Import","TagPicture")]
    public class ImportTagPictureCommand : PSCmdlet
    {
        private string[] paths;

        [Parameter( Position=0
                  , Mandatory=true
                  , ValueFromPipeline= true
                  , ValueFromPipelineByPropertyName=true
                  , HelpMessage="Image filenames to be loaded")]
        [Alias("PSPath")]
        public string[] Path
        {
            get { return paths; }
            set { paths = value; }
        }

        private void loadImages(List<TagLib.IPicture> lst, string filename)
            { lst.Add(new TagLib.Picture(filename)); }

        protected override void ProcessRecord()
        {
            List<TagLib.IPicture> lst = new List<TagLib.IPicture>(4);

            foreach (var s in paths)
            {
                if (WildcardPattern.ContainsWildcardCharacters(s))
                {
                    ProviderInfo pInfo;
                    var resolvedPaths = SessionState.Path.GetResolvedProviderPathFromPSPath(s, out pInfo);

                    foreach (var file in resolvedPaths)
                        loadImages(lst, GetUnresolvedProviderPathFromPSPath(file));
                }
                else loadImages(lst, GetUnresolvedProviderPathFromPSPath(s));
            }
            WriteObject( lst.ToArray() );
        } 
    }
}
