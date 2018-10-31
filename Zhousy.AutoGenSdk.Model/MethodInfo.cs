using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhousy.AutoGenSdk.Model
{
    public class MethodInfo
    {
        /// <summary>
        /// 所在类名称
        /// </summary>
        public string ClassName { set; get; }

        /// <summary>
        /// 当前序号
        /// </summary>
        public string Index { set; get; }

        /// <summary>
        /// 当前层级
        /// </summary>
        public int Level => Index.Count(a => a == '.') + 1;

        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethodName { set; get; }

        /// <summary>
        /// 方法描述(注释)
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// 方法参数集合
        /// </summary>
        public List<ParameterInfo> Parameters { set; get; }

        /// <summary>
        /// 返回值
        /// </summary>
        public ReturnValueInfo ReturnValue { set; get; }

        /// <summary>
        /// 方法代码详细解读
        /// </summary>
        public List<string> Details { set; get; }

        /// <summary>
        /// 内部方法
        /// </summary>
        public List<MethodInfo> Children { set; get; }

        /// <summary>
        /// 是不是顶级方法(顶级方法会作为整个文档的标识)
        /// </summary>
        public bool IsHead => Level == 1;

        /// <summary>
        /// 模块名称
        /// </summary>
        public string MngName { set; get; }

        /// <summary>
        /// 命名空间(DomainService/AppService)
        /// </summary>
        public string NameSpace { set; get; }
    }
}
