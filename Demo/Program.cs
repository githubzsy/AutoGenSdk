using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using System.Composition;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            // 从源代码生成语法树
            SyntaxTree tree = CSharpSyntaxTree.ParseText(
@"using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, World!"");
        }
    }
}");
            // 获得语法树的根结点root，root.KindText为CompliationUnit。注意Using语句通过root.Usings属性返回，而不是作为root的子节点
            var root = (CompilationUnitSyntax)tree.GetRoot();
            // root只有一个namespace子节点，代表了整个namespace语句，也就是从namespace开始到最后的反大括号之间所有代码，KindText属性值为NamespaceDeclaration
            var firstMember = root.Members[0];
            // Members列表的默认类型为MemberDeclarationSyntax，这里转换为NamespaceDeclarationSyntax
            var helloWorldDeclaration = (NamespaceDeclarationSyntax)firstMember;
            // namespace节点只有一个class子节点，代表了整个类型声明语句
            var programDeclaration = (ClassDeclarationSyntax)helloWorldDeclaration.Members[0];
            // class节点也只有一个method子节点，代表了整个Main方法
            var mainDeclaration = (MethodDeclarationSyntax)programDeclaration.Members[0];
            // main方法节点的子节点不再通过Members属性获得，ParameterList返回参数节点列表，Body属性返回方法体节点
            var argsParameter = mainDeclaration.ParameterList.Parameters[0];
        }

        //private static async Task RunAsync()
        //{
        //    Workspace workspace = new AdhocWorkspace();
        //    HostServices host = MefHostServices.Create();
        //    CompositionContext context = new CompositionContext();

        //    Solution solution = workspace.CurrentSolution;
            
        //    var project = solution.Projects.First(x => x.Name == "MSTest.Extensions");
        //    var document = project.Documents.First(x =>
        //        x.Name.Equals("ContractTestContext.cs", StringComparison.InvariantCultureIgnoreCase));

        //    var tree = await document.GetSyntaxTreeAsync();
        //    var syntax = tree.GetCompilationUnitRoot();

        //    var visitor = new TypeParameterVisitor();
        //    var node = visitor.Visit(syntax);

        //    var text = node.GetText();
        //    File.WriteAllText(document.FilePath, text.ToString());
        //}
    }

    //class TypeParameterVisitor : CSharpSyntaxRewriter
    //{
    //    public override SyntaxNode VisitTypeParameterList(TypeParameterListSyntax node)
    //    {
    //        var syntaxList = new SeparatedSyntaxList<TypeParameterSyntax>();
    //        syntaxList = syntaxList.Add(SyntaxFactory.TypeParameter("TParameter"));

    //        var lessThanToken = this.VisitToken(node.LessThanToken);
    //        var greaterThanToken = this.VisitToken(node.GreaterThanToken);
    //        return node.Update(lessThanToken, syntaxList, greaterThanToken);
    //    }
    //}
}
