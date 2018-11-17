using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.FindSymbols;
using Zhousy.AutoGenSdk.Model;

namespace Analyzer1
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(Analyzer1CodeFixProvider)), Shared]
    public class Analyzer1CodeFixProvider : CodeFixProvider
    {
        private const string title = "Make uppercase";

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(Analyzer1Analyzer.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            // TODO: Replace the following code with your own analysis, generating a CodeAction for each fix to suggest
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().First();
            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: title,
                    createChangedSolution: c => MakeUppercaseAsync(context.Document, declaration, c),
                    equivalenceKey: title),
                diagnostic);
        }

        private async Task<Solution> MakeUppercaseAsync(Document document, MethodDeclarationSyntax typeDecl, CancellationToken cancellationToken)
        {
            // Compute new uppercase name.
            var identifierToken = typeDecl.Identifier;
            var newName = identifierToken.Text.ToUpperInvariant();

            // Get the symbol representing the type to be renamed.
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
            var typeSymbol = semanticModel.GetDeclaredSymbol(typeDecl, cancellationToken);
            foreach (var item in typeDecl.Body.ChildNodes())
            {
                if (item.IsKind(SyntaxKind.IfStatement))
                {
                    var temp = (IfStatementSyntax)item;
                    if (temp.Statement.IsKind(SyntaxKind.Block))
                    {
                        var statements = ((BlockSyntax)temp.Statement).Statements;
                        foreach (var statement in statements)
                        {
                            var expression = ((ExpressionStatementSyntax)statement).Expression;
                            if (expression.IsKind(SyntaxKind.InvocationExpression))
                            {
                                ((InvocationExpressionSyntax)expression).
                            }
                        }
                    }
                }
                else if (item.IsKind(SyntaxKind.MethodKeyword))
                {
                    //添加到子方法中
                }
            }

            // Produce a new solution that has all references to that type renamed, including the declaration.
            var originalSolution = document.Project.Solution;
            var optionSet = originalSolution.Workspace.Options;
            var newSolution = await Renamer.RenameSymbolAsync(document.Project.Solution, typeSymbol, newName, optionSet, cancellationToken).ConfigureAwait(false);

            // Return the new solution with the now-uppercase type name.
            return newSolution;
        }

        //private async Task<Solution> GenSdkAsync(Document document, MethodDeclarationSyntax method, CancellationToken cancellationToken)
        //{
        //    List<MethodInfo> methodInfos = new List<MethodInfo>();
        //    methodInfos.Add(new MethodInfo()
        //    {
        //        ClassName=method.Parent.GetText().ToString(),
        //        Description
        //    })
        //    // 获取方法标识
        //    if (method?.Body != null)
        //    {
        //        foreach (var item in method.Body.ChildNodes)
        //        {
                    
        //        }  
        //    }
        //}

        //private MethodInfo GetMethod(MethodDeclarationSyntax method)
        //{
        //    MethodInfo info = new MethodInfo();
        //    info.ClassName = method.Parent.GetText().ToString();
        //    info.
        //}
    }
}
