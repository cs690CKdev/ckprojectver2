using System;

namespace ck_project.Helpers
{
    public class FeeCalculationHelper
    {
        GeneralHelper helper = new GeneralHelper();
        public double CalculateRetailTotalOfAllMaterials(lead lead)
        {   
            double totalMaterialsCost = 0;
            if (lead.total_cost != null)
            {
                foreach (var item in lead.total_cost)
                {
                    if (item.product_cost != null)
                    {
                        totalMaterialsCost += (double)item.product_cost;
                    }
                }
            }

            if (lead.installations.Count != 0)
            {
                foreach (var item in lead.installations)
                {
                    if (item.total_construction_materials_cost != null)
                    {
                        totalMaterialsCost += (double)item.total_construction_materials_cost;
                    }
                }
            }

            return totalMaterialsCost;
        }

        public double CalculateContractTotalWithoutOperationCost(lead lead)
        {
            double contractTotal = 0;
            contractTotal += this.CalculateRetailTotalOfAllMaterials(lead);

            if (lead.installations.Count != 0)
            {
                foreach (var item in lead.installations)
                {
                    if (item.total_installation_labor_cost != null)
                    {
                        contractTotal += (double)item.total_installation_labor_cost;
                        if (item.total_operational_expenses != null)
                        {
                            contractTotal -= (double)item.total_operational_expenses;
                        }
                    }
                }
            }
            else
            {
                contractTotal += new InstallationCalculationHelper().CalculateBuildingPermit(lead);
            }

            return contractTotal;
        }

        public double CalculateAvgMaterialCost(lead lead)
        {
            double avgMaterialCost = this.CalculateRetailTotalOfAllMaterials(lead);
            double avgCost = avgMaterialCost * helper.GetApplicableRate(Constants.rate_Name_Avg_Material_Cost);
            return avgCost;
        }

        public double CalculateAvgProjectCost(lead lead)
        {
            double avgProjectCost = this.CalculateContractTotalWithoutOperationCost(lead);
            double avgCost = avgProjectCost * helper.GetApplicableRate(Constants.rate_Name_Avg_Project_Cost);
            return avgCost;
        }

        public double CalculateOperationalAdminExpense(lead lead)
        {
            double operationalAdminExp = this.CalculateAvgMaterialCost(lead);
            double operationalAdExp = operationalAdminExp * helper.GetApplicableRate(Constants.rate_Name_Operational_Admin);
            return operationalAdExp;
        }

        public double CalculateOperationalExpForCreditCard(lead lead)
        {
            double expForCreditCard = this.CalculateContractTotalWithoutOperationCost(lead);
            double operationalExpCC = expForCreditCard * helper.GetApplicableRate(Constants.rate_Name_Operational_CreditCard);
            return operationalExpCC;
        }

        public double CalculateOperationalExpForInstallation(lead lead)
        {
            double expForInstallation = this.CalculateAvgInstallLaborCost(lead);
            if (lead.installations.Count != 0)
            {
                foreach (var item in lead.installations)
                {
                    if (item.total_travel_cost != null)
                    {
                        expForInstallation += (double)item.total_travel_cost;
                    }
                    
                    if (item.hotel_expense != null)
                    {
                        expForInstallation += (double)item.hotel_expense;
                    }

                    if (item.mileage_expense != null)
                    {
                        expForInstallation += (double)item.mileage_expense;
                    }

                    if (item.total_per_diem_cost != null)
                    {
                        expForInstallation += (double)item.total_per_diem_cost;
                    }

                    if (item.building_permit_cost != null)
                    {
                        expForInstallation += (double)item.building_permit_cost;
                    }
                }
            }
            else
            {
                expForInstallation += new InstallationCalculationHelper().CalculateBuildingPermit(lead);
            }

            double operationalExpenseForInstall = expForInstallation * helper.GetApplicableRate(Constants.rate_Name_Operational_Installation);
            return operationalExpenseForInstall;
        }

        public double CalculateAvgInstallLaborCost(lead lead)
        {
            double avgInstallLaborCost = 0.00;
            if (lead.installations.Count != 0)
            {
                foreach (var item in lead.installations)
                {
                    if (item.installation_labor_only_cost != null)
                    {
                        avgInstallLaborCost += (double)item.installation_labor_only_cost;
                    }
                }
            }

            if (avgInstallLaborCost != 0)
            {
                return avgInstallLaborCost * helper.GetApplicableRate(Constants.rate_Name_Avg_Install_Cost);
            }

            return avgInstallLaborCost;
        }

        //include operationalAdminexpense, operationalexpense for creditcard and operational cost for installation when there is installation job
        public double CalculateTotalOperationalExpense(lead lead)
        {
            if (lead.delivery_status.delivery_status_name.Equals(Constants.deliver_Status_Installed))
            {
                return Math.Round(this.CalculateOperationalAdminExpense(lead) + this.CalculateOperationalExpForCreditCard(lead) + this.CalculateOperationalExpForInstallation(lead), 2);
            }

            return 0.00;
        }

        public double CalculateTotalProjForBO(lead lead)
        {
            return (this.CalculateTotalOperationalExpense(lead) + this.CalculateContractTotalWithoutOperationCost(lead)) * helper.GetApplicableRate(Constants.rate_Name_BO);
        }
    }
}