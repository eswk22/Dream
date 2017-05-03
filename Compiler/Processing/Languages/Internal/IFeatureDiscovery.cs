using System.Collections.Generic;

namespace Compiler.Core.Processing.Languages.Internal {
    public interface IFeatureDiscovery {
        IReadOnlyCollection<string> SlowDiscoverAll();
    }
}