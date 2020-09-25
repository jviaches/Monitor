
namespace ResourcesLambda.Services.Resources
{
    public class ResourceViewModel
    {
        public string Url { get; set; }
        public string UserId { get; set; }

        /// <summary>
        /// In minutes
        /// </summary>
        public int MonitorPeriod { get; set; }
        public bool IsMonitorActivated { get; set; }
    }
}
