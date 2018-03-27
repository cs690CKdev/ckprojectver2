using ck_project.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ck_project.Helpers
{
    public class ProjSummaryHelper
    {
        ckdatabase db = new ckdatabase();
        public ProjectSummary CalculateProposalAmtDue(int id, ProjectSummary projSummary)
        {
            var totalCost = db.total_cost.Where(c => c.lead_number == id).FirstOrDefault();

            if (totalCost != null)
            {
                projSummary.TotalCost = totalCost;
                projSummary.TotalAmt = ((double)totalCost.total_cost1).ToString("C", CultureInfo.CurrentCulture);
                projSummary.AmtDueAtSignProposal = ((double)totalCost.total_cost1 * 0.5).ToString("C", CultureInfo.CurrentCulture);
                projSummary.AmtDueUponStartWork = ((double)totalCost.total_cost1 * 0.4).ToString("C", CultureInfo.CurrentCulture);
                projSummary.AmtDueUponCompletion = ((double)totalCost.total_cost1 * 0.1).ToString("C", CultureInfo.CurrentCulture);
            }

            return projSummary;
        }

        public ProjectSummary CalculateInstallCategoryCostMap(lead lead, ProjectSummary projectSummary)
        {
            Dictionary<string, Dictionary<string, Dictionary<string, double>>> installSubCatCostMap = new Dictionary<string, Dictionary<string, Dictionary<string, double>>>();
            Dictionary<string, Dictionary<string, double>> installCatCostMap = new Dictionary<string, Dictionary<string, double>>();

            foreach (var item in lead.installations)
            {
                //get category list
                ArrayList catList = new ArrayList();
                foreach (var task in item.tasks_installation)
                {
                    string currCat = task.task.task_main_cat;
                    if (!catList.Contains(currCat))
                    {
                        catList.Add(currCat);
                    }
                }

                //get sub category list
                Dictionary<string, ArrayList> catMap = new Dictionary<string, ArrayList>();
                foreach (string cat in catList)
                {
                    ArrayList subCatList = new ArrayList();
                    foreach (var task in item.tasks_installation)
                    {
                        string currSubCat = task.task.task_sub_cat;
                        if (cat.Equals(task.task.task_main_cat))
                        {
                            if (!subCatList.Contains(currSubCat))
                            {
                                subCatList.Add(currSubCat);
                            }
                        }
                    }
                    catMap.Add(cat, subCatList);
                }

                //calculate each category total cost and hours
                foreach (string cat in catList)
                {
                    double catCost = 0;
                    double catHours = 0;
                    double catLaborCost = 0;
                    double catMaterialRetail = 0;
                    double catMaterialCost = 0;

                    Dictionary<string, double> costCatMap = new Dictionary<string, double>();
                    Dictionary<string, Dictionary<string, double>> subCatMap = new Dictionary<string, Dictionary<string, double>>();
                    ArrayList subCatList = catMap[cat];
                    foreach (string subCat in subCatList)
                    {

                        Dictionary<string, double> costSubMap = new Dictionary<string, double>();
                        double catSubCost = 0.0;
                        double catSubHours = 0.0;
                        double catSubLaborCost = 0.0;
                        double catSubMaterialRetail = 0.0;
                        double catSubMaterialCost = 0.0;

                        foreach (var task in item.tasks_installation)
                        {
                            string taskCat = task.task.task_main_cat;
                            string taskSubCat = task.task.task_sub_cat;
                            if (cat.Equals(taskCat) && subCat.Equals(taskSubCat))
                            {
                                //calculate total costs per subcategories
                                catSubHours += task.hours;
                                catSubLaborCost += task.labor_cost;  
                                catSubMaterialCost += task.m_cost;
                                catSubMaterialRetail += task.material_retail_cost != null ? (double)task.material_retail_cost : 0.0;
                                catSubCost = catSubLaborCost + catSubMaterialRetail;
                            }
                        }

                        //calculate costs per categories
                        catHours += catSubHours;
                        catLaborCost += catSubLaborCost;
                        catMaterialCost += catSubMaterialCost;
                        catMaterialRetail += catSubMaterialRetail;
                        catCost += catSubCost;

                        costSubMap.Add(Constants.install_hours, catSubHours);
                        costSubMap.Add(Constants.install_laborCost, catSubLaborCost);
                        costSubMap.Add(Constants.install_materialCost, catSubMaterialCost);
                        costSubMap.Add(Constants.install_materialRetail, catSubMaterialRetail);
                        costSubMap.Add(Constants.install_totalCost, catSubCost);
                        subCatMap.Add(subCat, costSubMap);
                    }
                    costCatMap.Add(Constants.install_hours, catHours);
                    costCatMap.Add(Constants.install_laborCost, catLaborCost);
                    costCatMap.Add(Constants.install_materialCost, catMaterialCost);
                    costCatMap.Add(Constants.install_materialRetail, catMaterialRetail);
                    costCatMap.Add(Constants.install_totalCost, catCost);
                    installCatCostMap.Add(cat, costCatMap);
                    installSubCatCostMap.Add(cat, subCatMap);
                }
            }

            projectSummary.InstallCategoryCostMap = installCatCostMap;
            projectSummary.InstallSubCategoryCostMap = installSubCatCostMap;

            return projectSummary;
        }

        public ProjectSummary GetProductCategoryList(lead lead, ProjectSummary projectSummary)
        {
            projectSummary.ProductMapList1 = this.GetProductMapList(Constants.catDefList1, lead);
            projectSummary.ProductMapList2 = this.GetProductMapList(Constants.catDefList2, lead);

            return projectSummary;
        }

        public Dictionary<string, List<product>> GetProductMapList(IList<string> catList, lead lead)
        {
            Dictionary<string, List<product>> prodMap = new Dictionary<string, List<product>>();
            if (lead.products != null)
            {
                foreach (string cat in catList)
                {
                    List<product> prodList = new List<product>();
                    foreach (var item in lead.products)
                    {
                        if (cat.Equals(item.cat_anme, StringComparison.OrdinalIgnoreCase))
                        {
                            prodList.Add(item);
                        }
                    }
                    prodMap.Add(cat, prodList);
                }
            }
            return prodMap;
        }

        public ProjectSummary GetProductTotalMap(lead lead, ProjectSummary projectSummary)
        {
            Dictionary<string, double> prodTotalMap = new Dictionary<string, double>();
            if (lead.products != null)
            {
                foreach (string cat in Constants.catDefList)
                {
                    double total = 0.0;
                    foreach (var item in lead.products)
                    {
                        if (cat.Equals(item.cat_anme, StringComparison.OrdinalIgnoreCase))
                        {
                            total = total + (item.price * item.quantity);
                        }
                    }
                    prodTotalMap.Add(cat, total);
                }
            }
            projectSummary.ProductTotalMap = prodTotalMap;

            return projectSummary;
        }

        public ProjectSummary CalculateInstallationsData(lead lead, ProjectSummary projectSummary)
        {
            double operationExp = 0.0;
            InstallationCalculationHelper calHelper = new InstallationCalculationHelper();
            foreach (var item in lead.installations)
            {
                projectSummary.TwoWayMilesToJob = Math.Round(calHelper.CalculateBothWayMilesToJob(item.oneway_mileages_to_destination), 2);
                projectSummary.TotalInstallationDays = Math.Round(calHelper.CalculateTotalInstallationDays(item.installation_days, item.tile_installation_days), 2);
                projectSummary.PaidTravelTimeOneWay = Math.Round(calHelper.CalculatePaidTravelTimeOneWay(item.travel_time_one_way), 2);
                projectSummary.TotalApplicableTravelHours = Math.Round(calHelper.CalculateTotalApplicableTravelHours(item.installation_days, item.travel_time_one_way, item.recommendation), 2);
                operationExp = item.total_operational_expenses != null ? (double)item.total_operational_expenses : 0;
            }

            projectSummary.OperationalExp = operationExp == 0 ? new FeeCalculationHelper().CalculateTotalOperationalExpense(lead) : operationExp;

            return projectSummary;
        }

        public ProjectSummary SetCustomerData(lead lead, ProjectSummary projectSummary)
        {
            customer c = lead.customer;
            if (lead.project_status.project_status_name.Equals(Constants.proj_Status_Closed, StringComparison.OrdinalIgnoreCase))
            {
                var archived_lead = db.archive_leads.Where(a => a.lead_number == lead.lead_number).FirstOrDefault();
                if (archived_lead != null)
                {
                    c.customer_firstname = archived_lead.customer_firstname;
                    c.customer_lastname = archived_lead.customer_lastname;
                    c.email = archived_lead.customer_email;
                }
            }
            projectSummary.Customer = c;

            return projectSummary;
        }

        public ProjectSummary SetAddresses(lead lead, ProjectSummary projectSummary)
        {
            if (db.addresses.Where(a => a.deleted == false && a.lead_number == lead.lead_number).Any())
            {
                var addressList = db.addresses.Where(a => a.deleted == false && a.lead_number == lead.lead_number).ToList();
                foreach (var address in addressList)
                {
                    if (address.address_type.Equals(Constants.address_type_jobsite))
                    {
                        projectSummary.JobsiteAddress = address;
                    }
                    else
                    {
                        projectSummary.AlternativeAddress = address;
                    }
                }
            }

            return projectSummary;
        }
    }
}