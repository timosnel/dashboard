using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Dashboard.Controllers
{
    [Route("api/[controller]")]
    public class BambooController : Controller
    {
        private readonly IOptions<BambooOptions> _bambooOptions;

        public BambooController(IOptions<BambooOptions> bambooOptions)
        {
            if (bambooOptions == null) throw new ArgumentNullException(nameof(bambooOptions));

            _bambooOptions = bambooOptions;
        }

        [HttpGet("buildplan")]
        public async Task<IActionResult> BuildPlan()
        {
            string response = null;

            using(var httpClient = GetAuthenticatedHttpClient()) 
            {
                response = await httpClient.GetStringAsync("plan.json?max-results=500&os_authType=basic");
            }

            var deserialized = JsonConvert.DeserializeObject<PlanResponse>(response);

            var buildPlans = new List<BuildPlan>();
                
            foreach (var item in deserialized.Plans.Plan) 
            {
                if (!item.Key.StartsWith("ECM") && !item.Key.StartsWith("FUN")) continue;
                
                using(var httpClient = GetAuthenticatedHttpClient()) 
                {
                    response = await httpClient.GetStringAsync("plan/" + item.Key + "/branch.json?enabledOnly&os_authType=basic");
                }

                var specificPlanResponse = JsonConvert.DeserializeObject<SpecificPlanResponse>(response);

                if (specificPlanResponse.Branches.Branch.Count() == 0) continue; 

                var developBranch = specificPlanResponse.Branches.Branch.FirstOrDefault(b => b.ShortName == "develop");
                
                if (developBranch == null) continue;

                buildPlans.Add(new BuildPlan
                {
                    Key = developBranch.Key,
                    Name = item.ShortName
                });
            }

            return new JsonResult(buildPlans);
        }

        [HttpGet("buildplan/{key}/status")]
        public async Task<IActionResult> Status(string key) 
        {
            string response = null;
            using(var httpClient = GetAuthenticatedHttpClient()) 
            {
                response = await httpClient.GetStringAsync("result/" + key + ".json?os_authType=basic");
            }

            var deserialized = JsonConvert.DeserializeObject<StatusResponse>(response);

            return new JsonResult(deserialized.Results.Result.First().State);
        }

        private HttpClient GetAuthenticatedHttpClient() 
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_bambooOptions.Value.BambooUrl + "/rest/api/latest/");

            var credentials = $"{_bambooOptions.Value.BambooUsername}:{_bambooOptions.Value.BambooPassword}";
            var credentialsInBytes = Encoding.UTF8.GetBytes(credentials);
            var credentialsBase64 = System.Convert.ToBase64String(credentialsInBytes);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentialsBase64);

            return httpClient;
        }
    }
}
