﻿using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
namespace ControllerAnalyzers
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class ControllerNamingConventionAnalyzer : DiagnosticAnalyzer
	{
		public const string DiagnosticId = "WA0001";
		internal const string Description = "Controller type name should end in 'Controller'";
		internal const string MessageFormat = "Type name '{0}' does not end in Controller";
		internal const string Category = "Naming";

		private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Description, MessageFormat, Category, DiagnosticSeverity.Warning, true);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

		public override void Initialize(AnalysisContext context)
		{
			// context.RegisterCompilationAction(Analyzer);
			// context.RegisterSyntaxNodeAction(Analyzer, SyntaxKind.ClassDeclaration);
			context.RegisterSymbolAction(Analyzer, SymbolKind.NamedType);
		}

		private void Analyzer(SymbolAnalysisContext context)
		{
			var symbol = (INamedTypeSymbol)context.Symbol;
			if (symbol.BaseType == null)
			{
				return;
			}

			if ((symbol.BaseType.Name == "Controller" || symbol.BaseType.Name == "ApiController") &&
					  !symbol.Name.EndsWith("Controller"))
			{
				var diagnostic = Diagnostic.Create(Rule, symbol.Locations[0], symbol.Name);
				context.ReportDiagnostic(diagnostic);
			}
		}
	}
}
