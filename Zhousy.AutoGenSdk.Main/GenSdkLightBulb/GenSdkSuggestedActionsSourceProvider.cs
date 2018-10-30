using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Zhousy.AutoGenSdk.Main.GenSdkLightBulb
{
    [Export(typeof(ISuggestedActionsSourceProvider))]
    [Name("Auto Generate Sdk")]
    [ContentType("text")]
    internal class GenSdkSuggestedActionsSourceProvider : ISuggestedActionsSourceProvider
    {
        public ISuggestedActionsSource CreateSuggestedActionsSource(ITextView textView, ITextBuffer textBuffer)
        {
            throw new NotImplementedException();
        }
    }
}
