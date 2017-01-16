using Newtonsoft.Json;

namespace Dashboard
{
    public class PlanResponse 
    {
        [JsonProperty("plans")]
        public PlanPlansResponse Plans { get; set; }
    }

    public class PlanPlansResponse
    {
        [JsonProperty("plan")]
        public PlanPlanResponse[] Plan { get; set; }
    }

    public class PlanPlanResponse
    {
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("shortName")]
        public string ShortName { get; set; }
    }

    public class SpecificPlanResponse
    {
        [JsonProperty("branches")]
        public BranchesResponse Branches { get; set; }
    }

    public class BranchesResponse 
    {
        [JsonProperty("branch")]
        public BranchResponse[] Branch { get; set; }
    }

    public class BranchResponse 
    {
        [JsonProperty("shortName")]
        public string ShortName { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
    }

    public class StatusResponse
    {
        [JsonProperty("results")]
        public StatusResultsResponse Results { get; set; }
    }

    public class StatusResultsResponse
    {
        [JsonProperty("result")]
        public StatusResultResponse[] Result { get; set; }
    }

    public class StatusResultResponse
    {
        [JsonProperty("state")]
        public string State { get; set; }
    }
}