using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace TheStory.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            List<string> test = new List<string>
            {
                "aa", "bb", "cc", "dd", "ee"
            };

            var newList = new List<List<string>>();

            for(int i = 0; i < test.Count; ++i)
            {
                if(i % 2 == 0)
                {
                    var n = new List<string>();

                    n.Add(test[i]);

                    newList.Add(n);
                }
                else
                {
                    var index = i / 2;

                    newList[index].Add(test[i]);
                }
            }

            ViewBag.MyList = newList;


            var x = TempData;
            return View();
        }

        public IActionResult Test()
        {
            return View();
        }
    }
}
