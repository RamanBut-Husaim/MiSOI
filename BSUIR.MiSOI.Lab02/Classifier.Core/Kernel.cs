using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier.Core
{
    public sealed class Kernel
    {
        public ClassificationUnit Unit { get; set; }

        public bool Changed { get; set; }
    }
}
