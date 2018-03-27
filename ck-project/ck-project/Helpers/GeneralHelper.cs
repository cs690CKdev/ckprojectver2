using System;
using System.Collections.Generic;
using System.Linq;

namespace ck_project.Helpers
{
    public class GeneralHelper
    {
        ckdatabase db = new ckdatabase();
        static DateTime today = DateTime.Now;
        static DateTime date = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
        public double GetApplicableRate(string rateName)
        {
            double amount = 0;
            if (db.rates.Where(r => r.deleted == false && r.rate_name == rateName && date <= r.end_date && date >= r.start_date).Any())
            {
                var rate = db.rates.Where(r => r.deleted == false && r.rate_name == rateName && date <= r.end_date && date >= r.start_date).First();
                if (rate != null)
                {
                    amount = rate.amount;
                    if (rate.rate_measurement != null && rate.rate_measurement.Equals(Constants.rate_Measurement_Percent))
                    {
                        amount = amount / 100;
                    }
                }
            }

            return amount;
        }

        public double GetBuildingPermitAmount(double buildingPermitCost)
        {
            var buildingPermit = db.building_permit.Where(b => b.deleted == false && b.start_date <= date && b.end_date >= date && b.start_range <= buildingPermitCost && b.end_range >= buildingPermitCost).First();
            if (buildingPermit != null)
            {
                return buildingPermit.adjustable_fee + buildingPermit.fixed_fee;
            }

            return 0;
        }
   
        public double GetInstallationMaterialRate(lead lead)
        {
            foreach (var item in lead.installations)
            {
                if (item.ov_material_rate == null)
                {
                    return this.GetApplicableRate(Constants.rate_Name_Installed_Material);
                }
                else
                {
                    return (double)item.ov_material_rate;
                }
            }

            return 0;
        }

        public double GetInstallationLaborRate(lead lead)
        {
            foreach (var item in lead.installations)
            {
                if (item.ov_labor_rate == null)
                {
                    return this.GetApplicableRate(Constants.rate_Name_Hourly_Lead_Installer) + this.GetApplicableRate(Constants.rate_Name_Hourly_Junior_Installer);
                }
                else
                {
                    return (double)item.ov_labor_rate;
                }
            }

            return 0;
        }

        public double GetInstallationLaborRate(double? ov_labor_rate)
        {
            if (ov_labor_rate == null)
            {
                return this.GetApplicableRate(Constants.rate_Name_Hourly_Lead_Installer) + this.GetApplicableRate(Constants.rate_Name_Hourly_Junior_Installer);
            }
            else
            {
                return (double)ov_labor_rate;
            }
        }

        public lead SetAllInstallationCosts(lead lead)
        {
            InstallationCalculationHelper installHelper = new InstallationCalculationHelper();
            if (lead.installations != null)
            {
                foreach (var item in lead.installations)
                {
                    item.recommendation = installHelper.GetRecommendation(item.oneway_mileages_to_destination);
                    item.estimated_hours = installHelper.CalculateEstimatedHours(lead);
                    item.billable_hours = installHelper.CalculateBillableHours((double)item.estimated_hours);
                    item.installation_days = installHelper.CalculateInstallationDays((double)item.billable_hours);
                    item.tile_installation_days = Math.Round(installHelper.CalculateTileInstallationDays(item.total_tile_cost));
                    item.required_hotel_nights = installHelper.CalculateNumberOfHotelNights((double)item.installation_days, item.recommendation);
                    item.travel_time_one_way = installHelper.CalculateTravelTimeOneWay(item.oneway_mileages_to_destination);
                    item.hotel_round_trip = Math.Round(installHelper.CalculateHotelRoundTrip((double)item.installation_days));
                    item.total_per_diem_cost = Math.Round(installHelper.CalculatePerDiem((double)item.installation_days, item.recommendation), 2);
                    item.total_miles = installHelper.CalculateTotalMiles((double)item.installation_days, item.recommendation, item.oneway_mileages_to_destination);
                    item.installation_labor_only_cost = installHelper.CalculateLaborOnlyExpense((double)item.billable_hours, this.GetInstallationLaborRate(item.ov_labor_rate));
                    item.mileage_expense = installHelper.CalculateMileageExpense((double)item.total_miles, item.recommendation);
                    item.total_travel_cost = installHelper.CalculateTravelExpense((double)item.installation_days, (double)item.travel_time_one_way, item.recommendation);
                    item.hotel_expense = installHelper.CalculateHotelExpense((double)item.required_hotel_nights, item.recommendation);
                    item.total_construction_materials_cost = installHelper.CalculateMaterialRetailPrice(lead);
                    item.building_permit_cost = installHelper.CalculateBuildingPermit(lead);
                    item.total_operational_expenses = new FeeCalculationHelper().CalculateTotalOperationalExpense(lead);
                    item.total_installation_labor_cost = installHelper.CalculateTotalLaborExpense(lead);

                    // round the data before saving in the DB
                    item.estimated_hours = Math.Round((double)item.estimated_hours, 2);
                    item.billable_hours = Math.Round((double)item.billable_hours, 2);
                    item.installation_days = Math.Round((double)item.installation_days);
                    item.required_hotel_nights = Math.Round((double)item.required_hotel_nights);
                    item.travel_time_one_way = Math.Round((double)item.travel_time_one_way, 2);
                    item.total_miles = Math.Round((double)item.total_miles);
                }
            }
            return lead;
        }

        public void SaveProjectTotal(int leadNbr)
        {
            var lead = db.leads.Where(l => l.lead_number == leadNbr).First();

            if ((lead.products != null && lead.products.Count != 0) || (lead.installations != null && lead.installations.Count != 0))
            {
                // set calculated installation data
                lead = this.SetAllInstallationCosts(lead);

                //set totalcost data
                TotalCostHelper cHelper = new TotalCostHelper();
                InstallationCalculationHelper installHelper = new InstallationCalculationHelper();
                double buildingPermit = 0;
                double installationCost = 0;
                double materialCost = 0;
                foreach (var item in lead.installations)
                {
                    buildingPermit = (double)item.building_permit_cost;
                    installationCost = (double)item.total_installation_labor_cost;
                    materialCost = (double)item.total_construction_materials_cost;
                }

                if (lead.total_cost.Count == 0)
                {
                    total_cost total = new total_cost
                    {
                        lead_number = (int)lead.lead_number,
                        product_cost = cHelper.CalculateProductCost(lead),
                        building_permit_cost = buildingPermit,
                        tax_cost = cHelper.CalculateApplicableTax(lead)
                    };

                    // this is when there is no installation data
                    if (installationCost == 0)
                    {
                        buildingPermit = installHelper.CalculateBuildingPermit(lead);
                        installationCost = cHelper.CalculateInstallationCost(lead);
                    }

                    if (lead.tax_exempt)
                    {
                        total.total_cost1 = total.product_cost + materialCost + total.installation_cost;
                    }
                    else
                    {
                        total.total_cost1 = total.product_cost + materialCost + total.installation_cost + total.tax_cost;
                    }
                    
                    List<total_cost> costList = new List<total_cost>
                    {
                        total
                    };

                    lead.total_cost = costList;
                }
                else
                {
                    foreach (var item2 in lead.total_cost)
                    {
                        item2.product_cost = cHelper.CalculateProductCost(lead);
                        item2.installation_cost = cHelper.CalculateInstallationCost(lead);
                        item2.building_permit_cost = installHelper.CalculateBuildingPermit(lead);
                        item2.tax_cost = cHelper.CalculateApplicableTax(lead);
                        item2.total_cost1 = item2.product_cost + materialCost + item2.installation_cost + item2.tax_cost;

                        if (lead.tax_exempt)
                        {
                            item2.total_cost1 = item2.product_cost + materialCost + item2.installation_cost;
                        }
                        else
                        {
                            item2.total_cost1 = item2.product_cost + materialCost + item2.installation_cost + item2.tax_cost;
                        }
                    }
                }

                db.SaveChanges();
            }
        }
    }
}