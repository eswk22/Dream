using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Messages;
using Compiler.Core;
using Application.Utility.Translators;
using Application.Manager.Conversion;
using Application.Common;
using Application.DTO;

namespace Application.Manager.Implementation
{
    public class CompileManager : ICompileManager
    {
        #region GlobalDeclaration

        private readonly ICodeProcessor _processor;
        private IEntityTranslatorService _translatorService;
        private readonly IActionTaskBusinessManager _actiontaskManager;

        #endregion GlobalDeclaration

        #region Constructor
        public CompileManager(ICodeProcessor processor,
            IEntityTranslatorService translatorService,
            IActionTaskBusinessManager actiontaskmanager)
        {
            _translatorService = translatorService;
            _processor = processor;
            _actiontaskManager = actiontaskmanager;

        }

        #endregion Constructor


        public CompilationResult Compile(CompilationArguments Compileargs)
        {
            CompilationResult result = null;
            try
            {
                result = _translatorService.Translate<CompilationResult>(
                    _processor.Process(Compileargs.Code,
                    _translatorService.Translate<ProcessingOptions>(Compileargs)));
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public ActionTaskResponseMessage execute(ActionTaskCallerMessage actiontaskCaller)
        {
            ActionTaskResponseMessage response = null;
            try
            {
                ActionTaskDTO actiontask = _actiontaskManager.GetbyId(actiontaskCaller.ActionTaskId);
                foreach (var input in actiontaskCaller.Inputs)
                {
             //       actiontask.Inputs[input.Key] = input.Value;
                }
                var globals = new Globals()
                {
          //          INPUTS = actiontask.Inputs,
                    OUTPUTS = new DictionaryWithDefault<string, dynamic>(),
                    RESULTS = new DictionaryWithDefault<string, dynamic>()
                };
                _processor.Execute(actiontask.LocalCode, globals, actiontask.Timeout);
                response = new ActionTaskResponseMessage()
                {
                    ActionIdInRunBook = actiontaskCaller.ActionIdInRunBook,
                    ActionTaskId = actiontaskCaller.ActionTaskId,
                    IncidentId = actiontaskCaller.IncidentId//,
                                                            //Outputs = globals.OUTPUTS,
                                                            //Results = globals.RESULTS
                };
            }
            catch (Exception ex)
            {
            }
            return response;
        }



    }

    public class Globals
    {
        public DictionaryWithDefault<string, dynamic> INPUTS { get; set; }
        public DictionaryWithDefault<string, dynamic> OUTPUTS { get; set; }
        public DictionaryWithDefault<string, dynamic> RESULTS { get; set; }

    }
}
