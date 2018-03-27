using System;
using System.Linq;

namespace ck_project.Helpers
{
    public class TaxCalculationHelper
    {
        ckdatabase db = new ckdatabase();
        address jobsiteAddress = null;
        lead currLead = null;
        static DateTime today = DateTime.Now;
        static DateTime date = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
        FeeCalculationHelper feeHelper = new FeeCalculationHelper();
        public TaxCalculationHelper(lead lead) {
            if (db.addresses.Where(a => a.deleted == false && a.lead_number == lead.lead_number && a.address_type.Equals(Constants.address_type_jobsite)).Any())
            {
                var address = db.addresses.Where(a => a.deleted == false && a.lead_number == lead.lead_number && a.address_type.Equals(Constants.address_type_jobsite)).First();
                jobsiteAddress = address;
            }
            currLead = lead;
        }
        public double CalculateStateTax()
        {
            // not a installed project
            double stateTax = 0;
            if (jobsiteAddress != null)
            {
                string state = jobsiteAddress.state;
                if (state != null)
                {
                    if (db.taxes.Where(t => t.deleted == false && t.tax_anme == Constants.tax_Name_State && date <= t.end_date && date >= t.start_date && t.state == state).Any())
                    {
                        var taxData = db.taxes.Where(t => t.deleted == false && t.tax_anme == Constants.tax_Name_State && date <= t.end_date && date >= t.start_date && t.state == state).First();
                        if (taxData != null)
                        {
                            if (currLead.delivery_status.delivery_status_name.Equals(Constants.deliver_Status_Installed))
                            {
                                // use tax
                                stateTax = taxData.tax_value / 100 * feeHelper.CalculateAvgMaterialCost(currLead);
                            }
                            else
                            {
                                //sale tax
                                stateTax = taxData.tax_value / 100 * feeHelper.CalculateRetailTotalOfAllMaterials(currLead);
                            }
                        }
                    }
                }
            }

            return Math.Round(stateTax, 2);
        }

        public double CalculateCountyTax()
        {
            // not a installed project
            double countyTax = 0;
            if (jobsiteAddress != null)
            {
                string state = jobsiteAddress.state;
                string county = jobsiteAddress.county;
                if (state != null && county != null)
                {
                    if (db.taxes.Where(t => t.deleted == false && t.tax_anme == Constants.tax_Name_County && date <= t.end_date && date >= t.start_date && t.state == state && t.county == county).Any())
                    {
                        var taxData = db.taxes.Where(t => t.deleted == false && t.tax_anme == Constants.tax_Name_County && date <= t.end_date && date >= t.start_date && t.state == state && t.county == county).First();
                        if (taxData != null)
                        {
                            if (currLead.delivery_status.delivery_status_name.Equals(Constants.deliver_Status_Installed))
                            {
                                // installed project
                                countyTax = taxData.tax_value / 100 * feeHelper.CalculateAvgProjectCost(currLead);
                            }
                            else if (currLead.delivery_status.delivery_status_name.Equals(Constants.deliver_Status_Delivery))
                            {
                                //delivery project
                                countyTax = taxData.tax_value / 100 * feeHelper.CalculateRetailTotalOfAllMaterials(currLead);
                            }
                        }
                    }
                }
            }

            return Math.Round(countyTax, 2);
        }

        public double CalculateMunicipalTax()
        {
            // not a installed project
            double municipalTax = 0;
            if (jobsiteAddress != null)
            {
                string state = jobsiteAddress.state;
                string city = jobsiteAddress.city;
                if (state != null && city != null)
                {
                    if (db.taxes.Where(t => t.deleted == false && t.tax_anme == Constants.tax_Name_City && date <= t.end_date && date >= t.start_date && t.state == state && t.city == city).Any())
                    {
                        var taxData = db.taxes.Where(t => t.deleted == false && t.tax_anme == Constants.tax_Name_City && date <= t.end_date && date >= t.start_date && t.state == state && t.city == city).First();
                        if (taxData != null)
                        {
                            if (currLead.in_city)
                            {
                                // use tax - only apply to installed project
                                // sale tax - apply to non-installed project
                                if (currLead.delivery_status.delivery_status_name.Equals(Constants.deliver_Status_Installed))
                                {
                                    municipalTax = taxData.tax_value / 100 * feeHelper.CalculateAvgMaterialCost(currLead);
                                }
                                else
                                {
                                    municipalTax = taxData.tax_value / 100 * feeHelper.CalculateRetailTotalOfAllMaterials(currLead);
                                }
                            }
                            else
                            {
                                // sale tax - only apply to pick-up project
                                if (currLead.delivery_status.delivery_status_name.Equals(Constants.deliver_Status_Pickup))
                                {
                                    municipalTax = taxData.tax_value / 100 * feeHelper.CalculateRetailTotalOfAllMaterials(currLead);
                                }
                            }
                        }
                    }
                }
            }

            return Math.Round(municipalTax, 2);
        }

        public double CalculateBOTax()
        {
            // not a installed project
            double boTax = 0;
            if (jobsiteAddress != null)
            {
                string state = jobsiteAddress.state;
                string city = jobsiteAddress.city;
                if (state != null && city != null)
                {
                    if (db.taxes.Where(t => t.deleted == false && t.tax_anme == Constants.tax_Name_BO && date <= t.end_date && date >= t.start_date && t.state == state && t.city == city).Any())
                    {
                        var taxData = db.taxes.Where(t => t.deleted == false && t.tax_anme == Constants.tax_Name_BO && date <= t.end_date && date >= t.start_date && t.state == state && t.city == city).First();
                        if (taxData != null)
                        {
                            if (currLead.in_city && currLead.delivery_status.delivery_status_name.Equals(Constants.deliver_Status_Installed))
                            {
                                // installed project
                                boTax = taxData.tax_value / 100 * feeHelper.CalculateTotalProjForBO(currLead);
                            }
                        }
                    }
                }
            }

            return Math.Round(boTax, 2);
        }
    }
}