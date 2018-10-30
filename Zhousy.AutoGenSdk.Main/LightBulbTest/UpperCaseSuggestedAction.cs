﻿using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Zhousy.AutoGenSdk.Main.LightBulbTest
{
    internal class UpperCaseSuggestedAction : ISuggestedAction
    {
        private ITrackingSpan m_span;
        private string m_upper;
        private string m_display;
        private ITextSnapshot m_snapshot;

        public UpperCaseSuggestedAction(ITrackingSpan span)
        {
            m_span = span;
            m_snapshot = span.TextBuffer.CurrentSnapshot;
            m_upper = span.GetText(m_snapshot).ToUpper();
            m_display = string.Format("Convert '{0}' to upper case", span.GetText(m_snapshot));
        }

        public bool HasActionSets
        {
            get
            {
                return false;
            }
        }

        public string DisplayText
        {
            get
            {
                return m_display;
            }
        }

        public ImageMoniker IconMoniker
        {
            get
            {
                return default(ImageMoniker);
            }
        }

        public string IconAutomationText
        {
            get
            {
                return null;
            }
        }

        public string InputGestureText
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// 有预览吗
        /// </summary>
        public bool HasPreview
        {
            get
            {
                return true;
            }
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// 获取建议的操作
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IEnumerable<SuggestedActionSet>> GetActionSetsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<SuggestedActionSet>>(null);
        }

        /// <summary>
        /// 操作时预览
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<object> GetPreviewAsync(CancellationToken cancellationToken)
        {
            var textBlock = new TextBlock();
            textBlock.Padding = new Thickness(5);
            textBlock.Inlines.Add(new Run() { Text = m_upper });
            return Task.FromResult<object>(textBlock);
        }

        /// <summary>
        /// 执行动作
        /// </summary>
        /// <param name="cancellationToken"></param>
        public void Invoke(CancellationToken cancellationToken)
        {
            m_span.TextBuffer.Replace(m_span.GetSpan(m_snapshot), m_upper);
        }

        public bool TryGetTelemetryId(out Guid telemetryId)
        {
            telemetryId = Guid.Empty;
            return false;
        }
    }
}
