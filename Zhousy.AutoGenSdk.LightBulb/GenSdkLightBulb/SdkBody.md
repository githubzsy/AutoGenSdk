【LevelSharp】 【Index】 【ClassName】.【MethodName】 方法{#【ClassName】_【MethodName】}
> 简介 

&emsp;【Description】
包含处理项
【Details】

> 接口说明

- 【ClassName】.【MethodName】

- 入参

参数 | 类型 | 说明 
:---|:---|:---
【ParametersInTable】

- 返回值

类型 | 说明
:---|:---
【ReturnValue】

> 扩展说明

扩展之前项目程序集中引用平台dll文件`Mysoft.Map6.Core.dll`                                                                               
类文件中请先引用`using Mysoft.Map6.Core.Pipeline.ServicePlugin;`

- Before扩展（扩展的方法在标准产品方法前执行）

- After扩展（扩展的方法在标准产品方法后执行）

> 示例

- Before扩展

```csharp
using Mysoft.Map6.Core.Pipeline.ServicePlugin;
using Mysoft.Cbxt.【MngName】.【NameSpace】s;
using Mysoft.Cbxt.【MngName】.Model;

namespace Mysoft.Cbxt.【MngName】.Plugin.【NameSpace】s
{
    public class 【ClassName】Plugin: PluginBase<【ClassName】>
    {
        [PluginMethod(nameof(【ClassName】.【MethodName】), PluginMode.Before)]
        public void 【MethodName】Before(【ParametersInLine】)
        {
            
        }
    }
}
```
