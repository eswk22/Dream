using Application.Messages;
using Application.Utility.Translators;
using Compiler.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Application.Utility;

namespace Application.Manager.Conversion
{
    public class CompilationArgumentTranslator : EntityMapperTranslator<CompilationArguments, ProcessingOptions>
    {
        public override ProcessingOptions BusinessToService(IEntityTranslatorService service, CompilationArguments value)
        {
            ProcessingOptions _ProcessingOptions = null;

            if (value != null)
            {
                _ProcessingOptions = new ProcessingOptions();
                _ProcessingOptions.OptimizationsEnabled = value.OptimizationsEnabled;
                _ProcessingOptions.ScriptMode = value.SourceLanguage.Equals(Utility.LanguageIdentifier.CSharpScript) ||
                    value.SourceLanguage.Equals(Utility.LanguageIdentifier.VBNetScript);
				_ProcessingOptions.SourceLanguage = value.SourceLanguage;

				_ProcessingOptions.TargetLanguage = value.TargetLanguage;
            }
            return _ProcessingOptions;
        }

        public override CompilationArguments ServiceToBusiness(IEntityTranslatorService service, ProcessingOptions value)
        {
            CompilationArguments _CompilationArguments = null;
            if (value != null)
            {
                _CompilationArguments = new CompilationArguments();
                _CompilationArguments.OptimizationsEnabled = value.OptimizationsEnabled;
                _CompilationArguments.SourceLanguage = (Utility.LanguageIdentifier)value.SourceLanguage;
                _CompilationArguments.TargetLanguage = (Utility.LanguageIdentifier)value.TargetLanguage;

            }
            return _CompilationArguments;
        }
    }
}
