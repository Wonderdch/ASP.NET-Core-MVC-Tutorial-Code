using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Heavy.Web.Filters
{
    public class LogResourceFilter : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            Console.WriteLine("Executing Resource Filter!");
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            Console.WriteLine("Executed Resource Filter...");
        }
    }
}