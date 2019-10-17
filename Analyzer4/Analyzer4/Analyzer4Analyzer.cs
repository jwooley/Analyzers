using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzer4
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class Analyzer4Analyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "Analyzer4";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId,
																			Title,
																			MessageFormat,
																			Category,
																			DiagnosticSeverity.Warning,
																			isEnabledByDefault: true,
																			description: Description);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

		public override void Initialize(AnalysisContext context)
        {
            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information

            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeLocal, SyntaxKind.LocalDeclarationStatement);
            context.RegisterSyntaxNodeAction(AnalyzeForeach, SyntaxKind.ForEachStatement);
			//context.RegisterSyntaxNodeAction(AnalyzeField, SyntaxKind.FieldDeclaration);

		}

		/// <summary>
		/// Finds single character local variable declarations
		/// </summary>
		/// <example>
		/// // Detects x in:
		/// var x = 1;
		/// </example>
		/// <param name="context"></param>
		private static void AnalyzeLocal(SyntaxNodeAnalysisContext context)
        {
            LocalDeclarationStatementSyntax syntax = (LocalDeclarationStatementSyntax)context.Node;
            var variables = syntax.Declaration?.Variables;
            if (variables == null)
            {
                return;
            }

            foreach (VariableDeclaratorSyntax declarator in variables.Value)
            {
                if (declarator == null)
                {
                    continue;
                }
                var identifier = declarator.Identifier;
                if (identifier.IsMissing)
                {
                    continue;
                }
                if (identifier.ValueText.Length == 1)
                {
                    string name = identifier.ValueText;
                    context.ReportDiagnostic(Diagnostic.Create(Rule, identifier.GetLocation(), name));
                }
            }
        }

		/// <summary>
		/// Finds single character foreach local variable declarations
		/// </summary>
		/// <example>
		/// // Detects x in:
		/// foreach (var x in "test")
		/// </example>
		/// <param name="context"></param>
		private static void AnalyzeForeach(SyntaxNodeAnalysisContext context)
		{
			ForEachStatementSyntax syntax = (ForEachStatementSyntax)context.Node;
			var variable = syntax.Identifier;
			if (variable == null)
			{
				return;
			}
			if (variable.ValueText.Length == 1)
			{
				string name = variable.ValueText;
				context.ReportDiagnostic(Diagnostic.Create(Rule, variable.GetLocation(), name));
			}
		}

		//private static void AnalyzeField(SyntaxNodeAnalysisContext context)
		//{
		//    FieldDeclarationSyntax syntax = (FieldDeclarationSyntax)context.Node;
		//    var variables = syntax.Declaration?.Variables;
		//    if (variables == null)
		//    {
		//        return;
		//    }

		//    foreach (VariableDeclaratorSyntax declarator in variables.Value)
		//    {
		//        if (declarator ==  null)
		//        {
		//            continue;
		//        }
		//        var identifier = declarator.Identifier;
		//        if (identifier.IsMissing)
		//        {
		//            continue;
		//        }
		//        if (identifier.ValueText.Length == 1)
		//        {
		//            string name = identifier.ValueText;
		//            context.ReportDiagnostic(Diagnostic.Create(Rule, identifier.GetLocation(), name));
		//        }
		//    }
		//}
	}
}
