using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Language.Intellisense;

namespace Zhousy.AutoGenSdk.Main.GenSdkLightBulb
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

    internal class MethodInfo
    {
        /// <summary>
        /// 所在类名称
        /// </summary>
        internal string ClassName { set; get; }

        /// <summary>
        /// 当前序号
        /// </summary>
        internal string Index { set; get; }

        /// <summary>
        /// 当前层级
        /// </summary>
        internal int Level => Index.Count(a => a == '.') + 1;

        /// <summary>
        /// 方法名称
        /// </summary>
        internal string MethodName { set; get; }

        /// <summary>
        /// 方法描述(注释)
        /// </summary>
        internal string Description { set; get; }

        /// <summary>
        /// 方法参数集合
        /// </summary>
        internal List<Parameter> Parameters { set; get; }

        /// <summary>
        /// 返回值
        /// </summary>
        internal ReturnValue ReturnValue { set; get; }

        /// <summary>
        /// 方法代码详细解读
        /// </summary>
        internal List<string> Details { set; get; }

        /// <summary>
        /// 内部方法
        /// </summary>
        internal List<MethodInfo> Children { set; get; }

        /// <summary>
        /// 是不是顶级方法(顶级方法会作为整个文档的标识)
        /// </summary>
        internal bool IsHead { set; get; }

        /// <summary>
        /// 模块名称
        /// </summary>
        internal string MngName { set; get; }

        /// <summary>
        /// 命名空间(DomainService/AppService)
        /// </summary>
        internal string NameSpace { set; get; }

        internal StringBuilder GenSdk()
        {
            StringBuilder docText = new StringBuilder();
            #region 替换头部文本
            if (IsHead)
            {
                string temp = File.ReadAllText("\\SdkHead.md").Replace("【DocName】", MethodName).Replace("【DocDescription】", Description);

                StringBuilder methodListText = new StringBuilder();
                SetMethodListText(methodListText, this);
                docText.Append(methodListText);
            }
            #endregion

            StringBuilder bodyText = new StringBuilder();
            GetBodyText(bodyText, this);
            docText.Append(bodyText);

            return docText;
        }

        /// <summary>
        /// markdown中缩进的写法
        /// </summary>
        static string mdTab = "&emsp;";

        static string mdLevel = "#";

        /// <summary>
        /// 递归获取扩展接口清单
        /// </summary>
        /// <param name="methodListText">文本</param>
        /// <param name="method">方法</param>
        static void SetMethodListText(StringBuilder methodListText,MethodInfo method)
        {
            string tab = string.Empty;
            for (int i = 1; i < method.Level; i++)
            {
                tab += mdTab;
            }
            methodListText.AppendLine(string.Format("|[{0}{1} {2}.{3}](#{2}_{3})|{4}", tab, method.Index, method.ClassName, method.MethodName, method.Description));

            if (method.Children.Count > 0)
            {
                foreach (var child in method.Children)
                {
                    SetMethodListText(methodListText, child);
                }
            }
        }

        /// <summary>
        /// 递归获取所有方法的md文本
        /// </summary>
        /// <param name="bodyText"></param>
        /// <param name="method"></param>
        static void GetBodyText(StringBuilder bodyText,MethodInfo method)
        {
            #region 获取当前方法的markdown主体信息
            #region 设定层级字符
            string levelSharp = string.Empty;
            for (int i = 0; i < method.Level; i++)
            {
                levelSharp += "#";
            }
            levelSharp += "##"; //初始就是3级
            if (levelSharp.Count() > 4) levelSharp = "####";    //大于4级的就以4级展示(大于4级会字太小看不清)
            #endregion

            #region 设定方法详细描述文本
            StringBuilder detailsText = new StringBuilder();
            foreach (var item in method.Details)
            {
                detailsText.AppendLine("- " + item);
            }
            #endregion

            #region 设定参数相关
            StringBuilder parametersInTable = new StringBuilder();
            StringBuilder parametersInLine = new StringBuilder();
            foreach (var parameter in method.Parameters)
            {
                // entity | HtfkApply | 合同付款申请 
                parametersInTable.AppendLine(string.Format("{0} | {1} | {2}", parameter.Name, parameter.TypeName, parameter.Description));
                parametersInLine.Append(parameter.TypeName + " " + parameter.Name + ",");
            }
            #endregion

            #region 设定返回值相关
            string returnValue = string.Empty;
            if (method.ReturnValue != null)
            {
                returnValue = method.ReturnValue.TypeName + " | " + method.ReturnValue.Description;
            }
            #endregion

            string temp = File.ReadAllText("\\SdkBody.md").Replace("【LevelSharp】", levelSharp).Replace("【Index】", method.Index).Replace("【ClassName】", method.ClassName).Replace("【MethodName】", method.MethodName).Replace("【Description】", method.Description).Replace("【Details】", detailsText.ToString()).Replace("【ParametersInTable】", parametersInTable.ToString()).Replace("【ReturnValue】", returnValue).Replace("【MngName】", method.MngName).Replace("【NameSpace】", method.NameSpace).Replace("【ParametersInLine】", parametersInLine.ToString().TrimEnd(','));
            bodyText.Append(temp);
            #endregion


            #region 有子级时要去递归获取子级方法的markdown文本
            if (method.Children.Count > 0)
            {
                foreach (var child in method.Children)
                {
                    GetBodyText(bodyText, child);
                }
            }
            #endregion
        }
    }

    /// <summary>
    /// 方法参数
    /// </summary>
    internal class Parameter
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        internal string Name { set; get; }

        /// <summary>
        /// 参数类型名
        /// </summary>
        internal string TypeName { set; get; }

        /// <summary>
        /// 参数描述
        /// </summary>
        internal string Description { set; get; }
    }

    /// <summary>
    /// 返回值
    /// </summary>
    internal class ReturnValue
    {
        /// <summary>
        /// 返回值类型
        /// </summary>
        internal string TypeName { set; get; }

        /// <summary>
        /// 返回值描述
        /// </summary>
        internal string Description { set; get; }
    }
}
