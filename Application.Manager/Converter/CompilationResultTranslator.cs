using Application.Messages;
using Application.Utility.Translators;
using Compiler.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Application.Utility;

namespace Application.Manager.Conversion
{
    public class CompilationResultTranslator : EntityMapperTranslator<CompilationResult, ProcessingResult>
    {
        public override ProcessingResult BusinessToService(IEntityTranslatorService service, CompilationResult value)
        {

			//No use case for Business to Service
			throw new NotImplementedException();
        }

        public override CompilationResult ServiceToBusiness(IEntityTranslatorService service, ProcessingResult value)
        {
            CompilationResult _CompilationResult = null;
            if (value != null)
            {
                _CompilationResult = new CompilationResult();
                _CompilationResult.Decompiled = value.Decompiled;
                _CompilationResult.Errors = value.GetDiagnostics(DiagnosticSeverity.Error.ToString()).Select( m => this.DiagnosticTranslator(m));
                _CompilationResult.Infos = value.GetDiagnostics(DiagnosticSeverity.Info.ToString()).Select(m => this.DiagnosticTranslator(m));
				_CompilationResult.Warnings = value.GetDiagnostics(DiagnosticSeverity.Warning.ToString()).Select(m => this.DiagnosticTranslator(m));
				_CompilationResult.IsSuccess = value.IsSuccess;

			}
            return _CompilationResult;
        }

		private CompilationResultDiagnostic DiagnosticTranslator(ProcessingResultDiagnostic value)
		{
			CompilationResultDiagnostic _CompilationResultDiagnostic = null;
			if (value != null)
			{
				_CompilationResultDiagnostic = new CompilationResultDiagnostic();
				_CompilationResultDiagnostic.End = this.DiagnosticLocationTranslator(value.End);
				_CompilationResultDiagnostic.Id = value.Id;
				_CompilationResultDiagnostic.Message = value.Message;
				_CompilationResultDiagnostic.Severity =(DiagnosticSeverity) Enum.Parse(typeof(DiagnosticSeverity),value.Severity);
				_CompilationResultDiagnostic.Start = this.DiagnosticLocationTranslator(value.Start);

			}
			return _CompilationResultDiagnostic;
		}

		private CompilationResultDiagnosticLocation DiagnosticLocationTranslator(ProcessingResultDiagnosticLocation value)
		{
			CompilationResultDiagnosticLocation _DiagnosticLocation = null;
			if (value != null)
			{
				_DiagnosticLocation = new CompilationResultDiagnosticLocation();
				_DiagnosticLocation.Column = value.Column;
				_DiagnosticLocation.Line = value.Line;

			}
			return _DiagnosticLocation;
		}

	}
}
