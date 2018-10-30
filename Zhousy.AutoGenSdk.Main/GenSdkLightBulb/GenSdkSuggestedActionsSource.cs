using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;

namespace Zhousy.AutoGenSdk.Main.GenSdkLightBulb
{
    internal class GenSdkSuggestedActionsSource : ISuggestedActionsSource
    {
        private readonly GenSdkSuggestedActionsSourceProvider m_factory;
        private readonly ITextBuffer m_textBuffer;
        private readonly ITextView m_textView;

        public GenSdkSuggestedActionsSource(GenSdkSuggestedActionsSourceProvider provider, ITextView textView, ITextBuffer textBuffer)
        {
            m_factory = provider;
            m_textBuffer = textBuffer;
            m_textView = textView;
        }

        public event EventHandler<EventArgs> SuggestedActionsChanged;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SuggestedActionSet> GetSuggestedActions(ISuggestedActionCategorySet requestedActionCategories, SnapshotSpan range, CancellationToken cancellationToken)
        {
            TextExtent extent;
            if (TryGetWordUnderCaret(out extent) && extent.IsSignificant)
            {
                ITrackingSpan trackingSpan = range.Snapshot.CreateTrackingSpan(extent.Span, SpanTrackingMode.EdgeInclusive);
                var upperAction = new UpperCaseSuggestedAction(trackingSpan);
                var lowerAction = new LowerCaseSuggestedAction(trackingSpan);
                return new SuggestedActionSet[] { new SuggestedActionSet(new ISuggestedAction[] { upperAction, lowerAction }) };
            }
            return Enumerable.Empty<SuggestedActionSet>();
        }

        public Task<bool> HasSuggestedActionsAsync(ISuggestedActionCategorySet requestedActionCategories, SnapshotSpan range, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public bool TryGetTelemetryId(out Guid telemetryId)
        {
            throw new NotImplementedException();
        }
    }
}
