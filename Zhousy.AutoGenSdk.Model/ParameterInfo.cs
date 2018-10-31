using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhousy.AutoGenSdk.Model
{
    /// <summary>
    /// 方法参数
    /// </summary>
    public class ParameterInfo
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 参数类型名
        /// </summary>
        public string TypeName { set; get; }

        /// <summary>
        /// 参数描述
        /// </summary>
        public string Description { set; get; }
    }
}
