using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Management.Automation;
using System.IO;

namespace Tagcmdlet.Command
{
    [Cmdlet(VerbsCommon.Set, "Tags"
           , SupportsShouldProcess=true)]
    public class SetTagCommand : PSCmdlet
    {
        TagSet[] tags;
        bool passThru = false;
        string filenames = "";

        [Parameter( ValueFromPipeline=true
                  , ValueFromPipelineByPropertyName=true
                  , Mandatory=true
                  , HelpMessage="Tag(s) to apply")]
        public TagSet[] Tags
        {
            get { return tags; }
            set { tags = value; }
        }

        [Parameter( ValueFromPipeline= false
                  , Mandatory=false
                  , Position=0
                  , HelpMessage="On which file apply tags?")]
        [Alias("PSPath")]
        public string Path
        {
            get { return filenames; }
            set { filenames = value; }
        }

        [Parameter( HelpMessage = "If specified, tags will be re-outputted after file tagging" )]
        public SwitchParameter PassThru
        {
            get { return passThru; }
            set { passThru = value; }
        }

        void    ApplyTags( TagSet   s, string filename )
        {
            ShouldProcessReason reason;
            bool sprocess = ShouldProcess("Applying tags to file \"" + filename + "\""
                                         , "Should-we modify \"" + filename + "\" to apply tags to it?"
                                         , "File modification confirmation"
                                         , out reason);

            if (!sprocess) return;

            try {
                var f = TagLib.File.Create(filename);
                s.InjectIn(f.Tag);
                f.Save();

                if (passThru)
                    WriteObject(s);
            }
            catch (TagLib.UnsupportedFormatException e)
            {
                var err = new ErrorRecord(e, "The format is unsupported by get-tags", ErrorCategory.InvalidArgument, filename);
                WriteError(err);
            }
            catch (System.IO.FileNotFoundException e)
            {
                var err = new ErrorRecord(e, "File doesn't exist", ErrorCategory.ResourceUnavailable, filename);
                WriteError(err);
            }
            catch (TagLib.CorruptFileException e)
            {
                var err = new ErrorRecord(e, "File is corrupted", ErrorCategory.InvalidData, filename);
                WriteError(err);
            }
            catch (Exception e)
            {
                WriteDebug(e.ToString());
            }
        }

        protected override void ProcessRecord()
        {
            int i = 0;

            // we don't have filename, we apply the tags
            // to the original filenames.
            if (filenames == "")
            {
                foreach ( var t in tags )
                    ApplyTags( t, t.OriginalFullname );
                return;
            }

            if (WildcardPattern.ContainsWildcardCharacters(filenames))
            {
                ProviderInfo pInfo;
                var resolvedPaths = SessionState.Path.GetResolvedProviderPathFromPSPath( filenames
                                                                                       , out pInfo);

                foreach (var file in resolvedPaths)
                    ApplyTags(tags[i++], file);
            }
            else ApplyTags( tags[0]
                          , GetUnresolvedProviderPathFromPSPath(filenames));
        }
    }
}
