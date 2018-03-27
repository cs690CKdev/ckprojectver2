using System.Collections.Generic;

namespace ck_project.Models
{
    public class ProjectSummary
    {
        public lead Lead { get; set; }
        public customer Customer { get; set; }
        public List<branch> Branch { get; set; }
        public total_cost TotalCost { get; set; }
        public string AmtDueAtSignProposal { get; set; }
        public string AmtDueUponStartWork { get; set; }
        public string AmtDueUponCompletion { get; set; }
        public string TotalAmt { get; set; }
        public Dictionary<string, Dictionary<string, double>> InstallCategoryCostMap { get; set; }
        public Dictionary<string, Dictionary<string, Dictionary<string, double>>> InstallSubCategoryCostMap { get; set; }
        public Dictionary<string, List<product>> ProductMapList1 { get; set; }
        public Dictionary<string, List<product>> ProductMapList2 { get; set; }
        public Dictionary<string, double> ProductTotalMap { get; set; }
        public double TotalInstallationDays { get; set; }
        public double PaidTravelTimeOneWay { get; set; }
        public double TwoWayMilesToJob { get; set; }
        public double TotalApplicableTravelHours { get; set; }
        public double OperationalExp { get; set; }
        public address JobsiteAddress { get; set; }
        public address AlternativeAddress { get; set; }
    }
}