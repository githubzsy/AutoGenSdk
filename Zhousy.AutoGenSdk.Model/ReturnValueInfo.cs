using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhousy.AutoGenSdk.Model
{
    /// <summary>
    /// 返回值
    /// </summary>
    public class ReturnValueInfo
    {
        /// <summary>
        /// 返回值类型
        /// </summary>
        public string TypeName { set; get; }

        /// <summary>
        /// 返回值描述
        /// </summary>
        public string Description { set; get; }
    }
}
