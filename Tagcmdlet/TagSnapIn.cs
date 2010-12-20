using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Management.Automation;

namespace Tagcmdlet
{
    [RunInstaller(true)]
    public class TagSnapIn : PSSnapIn
    {
        public override string Description
        {
            get { return "This snap-in provide commands to work on sound file tags."; }
        }

        public override string Name
        {
            get { return "TagSnapIn";  }
        }

        public override string Vendor
        {
            get { return "Twinside";  }
        }
    }
}
