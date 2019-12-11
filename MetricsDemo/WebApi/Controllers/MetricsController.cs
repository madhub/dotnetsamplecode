using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class MetricsController : Controller
    {


        // POST: Metrics/Create
        [HttpPost("metrics/job/{jobname}")]
        public async Task<ActionResult> Post(string jobname)
        {
            Console.WriteLine(jobname);
            var headers = Request.Headers.Select(x =>
           {
               return $"Key={x.Key}, Value = {x.Value.ToString()}";
           });
            Console.WriteLine($" madhu --- Header {string.Join(",",headers)}");
            if (Request.Body != null)
            {
                using (var reader = new StreamReader(Request.Body,Encoding.ASCII))
                {
                    var value = await reader.ReadToEndAsync();
                    Console.WriteLine($"Madhu--{value}");
                }
            }
            
           return new OkResult();
        }
    }
}