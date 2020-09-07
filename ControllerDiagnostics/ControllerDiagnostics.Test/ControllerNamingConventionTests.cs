using ControllerAnalyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using TestHelper;
using VerifyCS = ControllerDiagnostics.Test.Verifiers.CSharpCodeFixVerifier<
 ControllerAnalyzers.ControllerNamingConventionAnalyzer, 
 Controller.CodeFixes.ControllerNamingConventionCodeFixProvider>;

namespace ControllerDiagnostics.Test
{
    [TestClass]
    public class UnitTest
    {
		
		[TestMethod]
        public async Task MvcClassWithoutConstructorFixesAsync()
        {
            var source = @"
using System.Web.Mvc;

namespace WebApplicationCS.Controllers
{
    public class HomeControllerTest : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}";
			//var test = new VerifyCS.Test
			//{
			//	TestCode = source
			//};
			//test.ReferenceAssemblies.AddAssemblies(new System.Collections.Immutable.ImmutableArray<string> { "System.Web.Mvc" });

			var expected = VerifyCS.Diagnostic(ControllerNamingConventionAnalyzer.DiagnosticId)
							.WithSpan(6, 18, 6, 36)
							.WithArguments("HomeControllerTest");


			await VerifyCS.VerifyAnalyzerAsync(source, expected);

			//var expected = VerifyCS.Diagnostic(ControllerNamingConventionAnalyzer.DiagnosticId)
			//	.WithSeverity(DiagnosticSeverity.Warning)
			//	.WithMessage($"Type name 'HomeControllerTest' does not end in Controller");
			//var expected = new DiagnosticResult
			//{
			//    Id = ControllerNamingConventionAnalyzer.DiagnosticId,
			//    Message = String.Format("Type name '{0}' does not end in Controller", "HomeControllerTest"),
			//    Severity = DiagnosticSeverity.Warning,
			//    Locations =
			//        new[] {
			//                new DiagnosticResultLocation("Test0.cs", 6, 18)
			//            }
			//};

			//VerifyCSharpDiagnostic(test, expected);

//			var fixtest = @"
//using System.Web.Mvc;

//namespace WebApplicationCS.Controllers
//{
//    public class HomeTestController : Controller
//    {
//        public ActionResult Index()
//        {
//            return View();
//        }
//    }
//}";

			//await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }


        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task MvcClassNotEndingInControllerCreatesDiagnosticsAsync()
        {
            var test = @"
using System.Web.Mvc;

namespace WebApplicationCS.Controllers
{
    public class HomeControllerTest : Controller
    {
		public HomeControllerTest()
		{

		}
        public ActionResult Index()
        {
            return View();
        }
    }
}";
			var expected = VerifyCS.Diagnostic("ControllerAnalyzers")
				.WithLocation(6)
				.WithSeverity(DiagnosticSeverity.Warning)
				.WithMessage($"Type name 'HomeControllerTest' does not end in Controller");

			//var expected = new DiagnosticResult
   //         {
   //             Id = ControllerNamingConventionAnalyzer.DiagnosticId,
   //             Message = String.Format("Type name '{0}' does not end in Controller", "HomeControllerTest"),
   //             Severity = DiagnosticSeverity.Warning,
   //             Locations =
   //                 new[] {
   //                         new DiagnosticResultLocation("Test0.cs", 6, 18)
   //                     }
   //         };

           // await VerifyCS.VerifyAnalyzerAsync(test, expected);

            var fixtest = @"
using System.Web.Mvc;

namespace WebApplicationCS.Controllers
{
    public class HomeTestController : Controller
    {
		public HomeTestController()
		{

		}
        public ActionResult Index()
        {
            return View();
        }
    }
}";
            await VerifyCS.VerifyCodeFixAsync(test, fixtest);
        }

        // Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task MvcClassEndingInControllerDoesNotCreateDiagnosticAsync()
        {
            var test = @"
using System.Web.Mvc;

namespace WebApplicationCS.Controllers
{
    public class HomeTestController : Controller
    {
		public HomeConTest()
		{

		}
        public ActionResult Index()
        {
            return View();
        }
    }
}";
			await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task WebApiClassNotEndingInControllerCreatesDiagnosticsAsync()
        {
            var test = @"
using System.Web.Http;

namespace WebApplicationCS.Controllers
{
    public class HomeConTest : ApiController
    {
		public HomeConTest()
		{

		}
    }
}";
			var expected = VerifyCS.Diagnostic("ControllerAnalyzers")
				.WithLocation(6)
				.WithSeverity(DiagnosticSeverity.Warning)
				.WithMessage($"Type name 'HomeControllerTest' does not end in Controller");


			//var expected = new DiagnosticResult
			//         {
			//             Id = ControllerNamingConventionAnalyzer.DiagnosticId,
			//             Message = String.Format("Type name '{0}' does not end in Controller", "HomeConTest"),
			//             Severity = DiagnosticSeverity.Warning,
			//             Locations =
			//     new[] {
			//                         new DiagnosticResultLocation("Test0.cs", 6, 18)
			//                     }
			//         };

			await VerifyCS.VerifyAnalyzerAsync(test, expected);
            //VerifyCSharpDiagnostic(test, expected);

            var fixtest = @"
using System.Web.Http;

namespace WebApplicationCS.Controllers
{
    public class HomeConTestController : ApiController
    {
		public HomeConTestController()
		{

		}
    }
}";
			await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
            //VerifyCSharpFix(test, fixtest);
        }
    }
}