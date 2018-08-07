using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;
using Analyzer4;

namespace Analyzer4.Test
{
    [TestClass]
    public class UnitTest : CodeFixVerifier
    {

        [TestMethod]
        public void EmptyFileShouldNotRaiseDiagnostic()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void VariableWithOneCharacterShouldRaiseDiagnostic()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {  
			void foo()
			{
				var x = 1;
			}
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "Analyzer4",
                Message = $"Type name 'x' is too short",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 15, 9)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }


		[TestMethod]
		public void ForeachVariableWithOneCharacterShouldRaiseDiagnostic()
		{
			var test = @"
    namespace ConsoleApplication1
    {
        class TypeName
        {  
			void foo()
			{
				foreach(var x in new int[0])
				{
					Console.WriteLine(x);
				}
			}
        }
    }";
			var expected = new DiagnosticResult
			{
				Id = "Analyzer4",
				Message = $"Type name 'x' is too short",
				Severity = DiagnosticSeverity.Warning,
				Locations =
					new[] {
							new DiagnosticResultLocation("Test0.cs", 8, 17)
						}
			};

			VerifyCSharpDiagnostic(test, expected);
		}

		[TestMethod]
		public void VariableWithMoreThanOneCharacterShouldntRaiseDiagnostic()
		{
			var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {  
			void foo()
			{
				var x1 = 1;
			}
        }
    }";

			VerifyCSharpDiagnostic(test);
		}

		protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Analyzer4Analyzer();
        }
    }
}
