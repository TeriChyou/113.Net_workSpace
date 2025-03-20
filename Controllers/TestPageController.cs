using Microsoft.AspNetCore.Mvc;

namespace mvcPlayground.Controllers
{
    public class TestPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Calculator()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Calculate(double number1, double number2, string operation)
        {
            double result = 0;
            switch (operation)
            {
                case "Add":
                    result = number1 + number2;
                    break;
                case "Subtract":
                    result = number1 - number2;
                    break;
                case "Multiply":
                    result = number1 * number2;
                    break;
                case "Divide":
                    if (number2 != 0)
                    {
                        result = number1 / number2;
                    }
                    else
                    {
                        ViewBag.Error = "Cannot divide by zero.";
                        return View("Calculator");
                    }
                    break;
                default:
                    ViewBag.Error = "Invalid operation.";
                    return View("Calculator");
            }

            ViewBag.Result = result;
            return View("Calculator");
        }

        public String TestLa()
        {
            return "はああ？";
        }
    }
}