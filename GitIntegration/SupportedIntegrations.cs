using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Text;

namespace GitIntegration
{
    internal static class SupportedIntegrations
    {
        public const string GitHub = nameof(GitHub);

        public static readonly IEnumerable<string> List = new List<string>{
            GitHub
        };
    }
}
