using Microsoft.VisualStudio.TestTools.UnitTesting;
using Application.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Configuration;
using Application.Utility.Translators;
using Application.Manager;
using Application.Manager.Implementation;
using Application.Manager.Conversion;
using Application.Utility.Logging;
using Compiler.Core;
using Compiler.Core.Processing;
using Compiler.Core.Processing.Languages;
using System.Collections;
using Compiler.Core.Processing.Languages.Internal;
using Compiler.Core.Decompilation;
using System.Web.Http.Hosting;
using System.Web.Http;
using Application.Messages;
using System.Net;
using Application.DTO.Conversion;

namespace Application.WebApi.Tests
{

    [TestClass()]
    public class CompilerControllerTests
    {
        private ICompileManager _compileManager { get; set; }
        private IActionTaskBusinessManager _actionTaskManager { get; set; }
        private string ServiceBaseURL { get; set; }
        [TestInitialize]
        public void Initialize()
        {
            ServiceBaseURL = ConfigurationManager.AppSettings["ServiceBaseUrl"];
            IEntityTranslatorService translatorService = new EntityTranslatorService();
            translatorService.RegisterEntityTranslator(new CompilationResultTranslator());
            translatorService.RegisterEntityTranslator(new CompilationArgumentTranslator());
            translatorService.RegisterEntityTranslator(new ActionTaskTranslator());
            translatorService.RegisterEntityTranslator(new ActionTasklistTranslator());
            IFeatureDiscovery VBFeatureDiscovery = new VBNetFeatureDiscovery();
            IFeatureDiscovery CFeatureDiscovery = new CSharpFeatureDiscovery();
            IRoslynLanguage VbLanguage = new VBNetLanguage(VBFeatureDiscovery);
            IRoslynLanguage CLanguage = new CSharpLanguage(CFeatureDiscovery);
            List<IRoslynLanguage> languages = new List<IRoslynLanguage>();
            languages.Add(VbLanguage);
            languages.Add(CLanguage);
            IDecompiler VBDecompiler = new VBNetDecompiler();
            IDecompiler CDEcompiler = new CSharpDecompiler();
            IDecompiler ILCompiler = new ILDecompiler();
            List<IDecompiler> decompilers = new List<IDecompiler>();
            decompilers.Add(VBDecompiler);
            decompilers.Add(CDEcompiler);
            decompilers.Add(ILCompiler);
            ICodeProcessor processor = new CodeProcessor(languages,decompilers);

      //      _actionTaskManager = new ActionTaskManager(new ActionTaskRepository(), translatorService, new CrucialLogger());

            _compileManager = new CompileManager(processor,translatorService, _actionTaskManager);
        }

        [TestMethod()]
        public void CompilationTest()
        {
            var controller = new CompilerController(_compileManager)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "api/compilation")
                }
            };
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            CompilationArguments args = new CompilationArguments()
            {
                Code = "x + y",
                OptimizationsEnabled = false,
                SourceLanguage = Utility.LanguageIdentifier.CSharpScript,
                TargetLanguage = Utility.LanguageIdentifier.CSharpScript
            };
            var response = controller.Compilation(args);
            Assert.Equals(response.StatusCode, "OK");
        }
    }
}