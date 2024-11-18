using BuisnessLogicLayer.Master.CodeMaster;
using System;
using System.Web.Script.Services;
using System.Web.Services;

namespace MotorClaimProcessingSystem
{
    [WebService(Namespace = "http://yournamespace.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [System.ComponentModel.ToolboxItem(false)]
    public class CurrencyExchange : WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public PremiumResult CalculatePremiumAndVAT(decimal netFcPremium)
        {
            
            decimal exchangeRate = 3.67M; 
            decimal vatRate = 0.05M;

            decimal netLcPremium = netFcPremium * exchangeRate;
            decimal vatFcAmount = netFcPremium * vatRate;
            decimal vatLcAmount = vatFcAmount * exchangeRate;

            return new PremiumResult
            {
                NetLcPremium = netLcPremium,
                VatFcAmount = vatFcAmount,
                VatLcAmount = vatLcAmount
            };
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public EstimateResult CalculateEstimateAmounts(decimal estFcAmt, string currencyCode)
        {
            CodeMasterManager codeMasterManager = new CodeMasterManager();

            decimal exchangeRate = codeMasterManager.FindCurrencyExRate(currencyCode);
            decimal vatRate = 0.05M;      

            decimal estLcAmt = estFcAmt * exchangeRate;        
            decimal estVatFcAmt = estFcAmt * vatRate;          // VAT in foreign currency
            decimal estVatLcAmt = estVatFcAmt * exchangeRate;  // VAT in local currency

            return new EstimateResult
            {
                EstLcAmt = estLcAmt,
                EstVatFcAmt = estVatFcAmt,
                EstVatLcAmt = estVatLcAmt
            };
        }

        public class PremiumResult
        {
            public decimal NetLcPremium { get; set; }
            public decimal VatFcAmount { get; set; }
            public decimal VatLcAmount { get; set; }
        }

        public class EstimateResult
        {
            public decimal EstLcAmt { get; set; }
            public decimal EstFcAmt { get; set; }
            public decimal EstVatFcAmt { get; set; }
            public decimal EstVatLcAmt { get; set; }
        }
    }
}
