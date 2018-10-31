using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhousy.AutoGenSdk.Model;

namespace Zhousy.AutoGenSdk.LightBulb.GenSdkLightBulb
{
    /// <summary>
    /// 封装生成sdk文件的操作
    /// </summary>
    internal static class GenSdk
    {
        /// <summary>
        /// markdown中缩进的写法
        /// </summary>
        static string mdTab = "&emsp;";

        static string mdLevel = "#";

        /// <summary>
        /// 自动生成sdk文件到指定文件
        /// </summary>
        /// <param name="method">目标方法</param>
        /// <param name="path">目标位置</param>
        internal static void GenSdkFile(MethodInfo method,string path)
        {
            var docText = GetSdkText(method);
            File.WriteAllText(path, docText.ToString());
        }

        /// <summary>
        /// 获取sdk文本，若方法为顶级方法还会产生头部文本
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        internal static StringBuilder GetSdkText(MethodInfo method)
        {
            StringBuilder docText = new StringBuilder();
            #region 替换头部文本
            if (method.IsHead)
            {
                string temp = File.ReadAllText("\\SdkHead.md").Replace("【DocName】", method.MethodName).Replace("【DocDescription】", method.Description);

                StringBuilder methodListText = new StringBuilder();
                SetMethodListText(methodListText, method);
                docText.Append(methodListText);
            }
            #endregion

            StringBuilder bodyText = new StringBuilder();
            GetBodyText(bodyText, method);
            docText.Append(bodyText);

            return docText;
        }
        
        /// <summary>
        /// 递归获取扩展接口清单
        /// </summary>
        /// <param name="methodListText">文本</param>
        /// <param name="method">方法</param>
        static void SetMethodListText(StringBuilder methodListText, MethodInfo method)
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
        static void GetBodyText(StringBuilder bodyText, MethodInfo method)
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
}
