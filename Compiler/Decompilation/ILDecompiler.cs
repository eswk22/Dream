﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.Disassembler;
using JetBrains.Annotations;
using Mono.Cecil;
using Compiler.Core.Decompilation.Support;
using Application.Utility;

namespace Compiler.Core.Decompilation {
    [ThreadSafe]
    public class ILDecompiler : IDecompiler {
        public void Decompile(Stream assemblyStream, TextWriter codeWriter) {
            var assembly = AssemblyDefinition.ReadAssembly(assemblyStream);

            var output = new CustomizableIndentPlainTextOutput(codeWriter) {
                IndentationString = "    "
            };
            var disassembler = new ReflectionDisassembler(output, false, new CancellationToken());
            disassembler.WriteModuleContents(assembly.MainModule);
        }

        public LanguageIdentifier Language => LanguageIdentifier.IL;
    }
}
