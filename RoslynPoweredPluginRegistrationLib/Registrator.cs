namespace RoslynPoweredPluginRegistrationLib
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Roslyn.Compilers;
    using Roslyn.Compilers.CSharp;

    public class Registrator
    {

        public RegistrationInstruction GetRegistrationInstructions(string path)
        {
            var syntaxTree = SyntaxTree.ParseFile(path);

            var arrayCreation =
                (ArrayCreationExpressionSyntax)
                syntaxTree.GetRoot().DescendantNodes().First(n => n.Kind == SyntaxKind.ArrayCreationExpression);

            var arrayInitialization =
                (InitializerExpressionSyntax)
                arrayCreation.DescendantNodes().First(x => x.Kind == SyntaxKind.ArrayInitializerExpression);

            var expressions = arrayInitialization.Expressions;

            foreach (var e in expressions)
            {
                var memberAccess = (MemberAccessExpressionSyntax)e;

                var name = memberAccess.Name.ToString();
            }

            var statement = arrayCreation.GetText();
            //return statement;

            return new RegistrationInstruction();
        }
    }
}
