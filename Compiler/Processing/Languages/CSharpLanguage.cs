﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Compiler.Core.Processing.Languages.Internal;
using Application.Utility;
using System.Threading;
using System.Threading.Tasks;

namespace Compiler.Core.Processing.Languages
{
    [ThreadSafe]
    public class CSharpLanguage : IRoslynLanguage
    {
        private static readonly LanguageVersion MaxLanguageVersion = Enum
            .GetValues(typeof(LanguageVersion))
            .Cast<LanguageVersion>()
            .Max();
        private static readonly IReadOnlyCollection<string> PreprocessorSymbols = new[] { "__DEMO_EXPERIMENTAL__" };

        // ReSharper disable once AgentHeisenbug.FieldOfNonThreadSafeTypeInThreadSafeType
        private readonly IReadOnlyCollection<MetadataReference> _references = new[] {
            MetadataReference.CreateFromFile(typeof(Binder).Assembly.Location)
        };
        private readonly IReadOnlyDictionary<string, string> _features;

        public CSharpLanguage(IFeatureDiscovery featureDiscovery)
        {
            _features = featureDiscovery.SlowDiscoverAll().ToDictionary(f => f, f => (string)null);
        }

        public LanguageIdentifier Identifier => LanguageIdentifier.CSharp;

        public SyntaxTree ParseText(string code, SourceCodeKind kind)
        {
            var options = new CSharpParseOptions(
                kind: kind,
                languageVersion: MaxLanguageVersion,
                preprocessorSymbols: PreprocessorSymbols
            ).WithFeatures(_features);
            return CSharpSyntaxTree.ParseText(code, options);
        }

        public Compilation CreateLibraryCompilation(string assemblyName, bool optimizationsEnabled)
        {
            var options = new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                optimizationLevel: optimizationsEnabled ? OptimizationLevel.Release : OptimizationLevel.Debug,
                allowUnsafe: true
            );

            return CSharpCompilation.Create(assemblyName, options: options, references: _references);
        }

        public object execute(string code, object globals, int timeout)
        {
            try
            {
                var scriptOptions = GetScriptOptions();
                CancellationToken token = new CancellationTokenSource(timeout * 1000).Token;
                var task = CSharpScript.RunAsync(code, options: scriptOptions, globals: globals, cancellationToken: token);
                return task.Result;
            }
            catch (TaskCanceledException ex)
            {
                // Task was canceled before running.
                throw new TimeoutException();
            }
            catch (OperationCanceledException ex)
            {
                // Task was canceled while running.
                throw new TimeoutException();
            }
        }


        private static ScriptOptions GetScriptOptions()
        {
            return ScriptOptions
                .Default
                .AddReferences(SurfaceArea.GetAssemblies())
                .AddImports(SurfaceArea.GetNamespaces());
        }


    }
}
