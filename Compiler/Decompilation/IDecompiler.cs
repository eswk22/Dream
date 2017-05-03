using System.IO;
using JetBrains.Annotations;
using Application.Utility;

namespace Compiler.Core.Decompilation {
    [ThreadSafe]
    public interface IDecompiler {
        LanguageIdentifier Language { get; }
        void Decompile(Stream assemblyStream, TextWriter codeWriter);
    }
}