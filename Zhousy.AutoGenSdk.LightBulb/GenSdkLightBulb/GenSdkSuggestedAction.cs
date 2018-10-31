using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Language.Intellisense;
using Zhousy.AutoGenSdk.Model;

namespace Zhousy.AutoGenSdk.LightBulb.GenSdkLightBulb
{
    internal class GenSdkSuggestedAction : ISuggestedAction
    {
        /// <summary>
        /// 解决方案信息
        /// </summary>
        private string m_SolutionInfo;

        /// <summary>
        /// 当前指向的方法信息
        /// </summary>
        private string m_MethodInfo;

        public GenSdkSuggestedAction(string solutionInfo,string methodInfo)
        {
            // 1.读取解决方案信息
            // 2.初始化标题层级为1
            // 3.读取方法体内部代码，一行行去读
            // 4.while方法内部
            // 5.注释信息保留，if语句保留，读到引用其它方法时(public virtual)，通过解决方案穿透到其它方法内部，执行2345，且在初始化标题层级时需要+1

            m_SolutionInfo = solutionInfo;
            m_MethodInfo = methodInfo;
        }

        public bool HasActionSets => false;

        public string DisplayText => "产生此方法的sdk";

        public ImageMoniker IconMoniker => default(ImageMoniker);

        public string IconAutomationText => null;

        public string InputGestureText => null;

        public bool HasPreview => false;

        public void Dispose()
        {

        }

        /// <summary>
        /// 获取建议的操作列表
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IEnumerable<SuggestedActionSet>> GetActionSetsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<SuggestedActionSet>>(null);
        }

        /// <summary>
        /// 获取预览
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<object> GetPreviewAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<object>(null);
        }
        
        /// <summary>
        /// 执行动作
        /// </summary>
        /// <param name="cancellationToken"></param>
        public void Invoke(CancellationToken cancellationToken)
        {
            MethodInfo method = new MethodInfo();
        }

        public bool TryGetTelemetryId(out Guid telemetryId)
        {
            throw new NotImplementedException();
            
        }
    }
}
