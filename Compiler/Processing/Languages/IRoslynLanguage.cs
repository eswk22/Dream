using Application.Utility;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;

namespace Compiler.Core.Processing.Languages {
    [ThreadSafe]
    public interface IRoslynLanguage {
        LanguageIdentifier Identifier { get; }
        SyntaxTree ParseText(string code, SourceCodeKind kind);
        Compilation CreateLibraryCompilation(string assemblyName, bool optimizationsEnabled);

		object execute(string code,object globals,int timeout);
    }
}