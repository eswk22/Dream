using System;
using JetBrains.Annotations;

namespace Compiler.Core {
    [ThreadSafe]
    public interface ICodeProcessor : IDisposable {
        [NotNull]
        ProcessingResult Process([NotNull] string code, [CanBeNull] ProcessingOptions options = null);

		object Execute(string code, object globals,int timeout);
	}
}