using CosmosDBThrottlingTest.Models;
using CosmosDBThrottlingTest.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDBThrottlingTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var course = new CourseRepository("MOL");
            var AllCourses = course.GetCourses(
                new CourseQueryParams()
                {
                    CourseGuid= "4ff38737-26f7-4561-bbd1-8bb3caaac778",
                    DelFlag = false
                }).ToList();

            Console.ReadKey();
        }
    }
}
