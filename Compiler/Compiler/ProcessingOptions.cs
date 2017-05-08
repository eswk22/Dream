﻿using Application.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Core {
    [Serializable]
    public class ProcessingOptions {
        public bool OptimizationsEnabled { get; set; }
        public bool ScriptMode { get; set; }
        public LanguageIdentifier SourceLanguage { get; set; }
        public LanguageIdentifier TargetLanguage { get; set; }
    }
}
