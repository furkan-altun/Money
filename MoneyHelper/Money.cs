using System.Globalization;

namespace MoneyHelper
{
    /// <summary>
    /// Money model is a formatter appearance of any decimal value.    
    /// </summary>
    public class Money
    {
        /// <summary>
        /// Integral part of a decimal amount ex: 1034.567 --> 1034
        /// </summary>
        public int IntegralPart { get; set; }

        /// <summary>
        /// Integral part of a decimal amount. ex: 1034.567 --> 567
        /// It depends on nDigit parameter of FormatDecimalAmountToMoney method, if you send 2 to the nDigit, you get 56.
        /// </summary>
        public int DecimalPart { get; set; }

        /// <summary>
        /// It is created for the integrity of the money model.
        /// Also, it determines the currency symbol. If you send USD, you can get $ sign.
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// The currency symbol of currency code. It is determined by currency code value.
        /// </summary>
        public string CurrencySymbol { get; set; }

        /// <summary>
        /// Whole number of amount value.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Formatted string value of amount.
        /// It depends on DecimalSeperatorType Enum Value. 
        /// Default value is dot seperator of integral part, comma seperator of numbers. ex: 1,034.567
        /// Text value can be changable by comma seperator of decimal part, dot seperator of numbes. ex: 1.034,567
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// DecimalSeperatorType determines the text value of Money Model.
        /// Dot seperates decimal part via '.' like 1,034.567
        /// Comma seperates decimal part via ',' like 1.034,567
        /// </summary>
        public enum DecimalSeperatorType
        {
            Dot,
            Comma
        }

        /// <summary>
        /// It formats decimal amount to a specified Money model.
        /// </summary>
        /// <param name="request">decimal amount value</param>
        /// <param name="request">currency code of amount(It can sent null or empty string)</param>
        /// <returns>Money model including integral part, decimal part, currency code&symbol, amount, formatted amount by culturel appearance</returns>
        public static Money FormatDecimalAmountToMoney(decimal amount, string currencyCode)
        {
            return FormatDecimalAmountToMoney(amount, currencyCode, default, DecimalSeperatorType.Comma);
        }

        /// <summary>
        /// It formats decimal amount to a specified Money model.
        /// </summary>
        /// <param name="request">decimal amount value</param>
        /// <param name="request">currency code of amount(It can sent null or empty string)</param>
        /// <param name="request">nDigit determines the length of decimal part. If you don't send or default you get whole decimal part of amount.</param>
        /// <returns>Money model including integral part, decimal part, currency code&symbol, amount, formatted amount by culturel appearance</returns>
        public static Money FormatDecimalAmountToMoney(decimal amount, string currencyCode, int nDigit)
        {
            return FormatDecimalAmountToMoney(amount, currencyCode, nDigit, DecimalSeperatorType.Comma);
        }

        /// <summary>
        /// It formats decimal amount to a specified Money model.
        /// </summary>
        /// <param name="request">decimal amount value</param>
        /// <param name="request">currency code of amount(It can sent null or empty string)</param>
        /// <param name="request">seperatorType determines Text value format. ex: 1,043.567 (DecimalSeperatorType.Dot) or 1.043,567 (DecimalSeperatorType.Comma) </param>
        /// <returns>Money model including integral part, decimal part, currency code&symbol, amount, formatted amount by culturel appearance</returns>
        public static Money FormatDecimalAmountToMoney(decimal amount, string currencyCode, DecimalSeperatorType seperatorType)
        {
            return FormatDecimalAmountToMoney(amount, currencyCode, default, seperatorType);
        }

        /// <summary>
        /// It formats decimal amount to a specified Money model.
        /// </summary>
        /// <param name="request">decimal amount value</param>
        /// <param name="request">currency code of amount(It can sent null or empty string)</param>
        /// <param name="request">nDigit determines the length of decimal part. If you don't send or default you get whole decimal part of amount.</param>
        /// <param name="request">seperatorType determines Text value format. ex: 1,043.567 (DecimalSeperatorType.Dot) or 1.043,567 (DecimalSeperatorType.Comma) </param>
        /// <returns>Money model including integral part, decimal part, currency code&symbol, amount, formatted amount by culturel appearance</returns>
        public static Money FormatDecimalAmountToMoney(decimal amount, string currencyCode, int nDigit, DecimalSeperatorType seperatorType)
        {
            NumberFormatInfo culturalInfo = new CultureInfo("en-US", false).NumberFormat;

            var integralPart = Math.Truncate(amount);
            var decimalPart = amount - integralPart;
            var formattedDecimalPart = decimalPart.ToString(culturalInfo).Replace("-", "").Replace("0.", "");

            if (nDigit > 0)
            {
                culturalInfo.NumberDecimalDigits = nDigit;
            }
            else
            {
                culturalInfo.NumberDecimalDigits = formattedDecimalPart.Length;
            }

            var response = new Money
            {
                IntegralPart = (int)integralPart,
                DecimalPart = int.TryParse(formattedDecimalPart, out var value) ? value : 0,
                Amount = amount,
                CurrencyCode = currencyCode
            };

            if (seperatorType == DecimalSeperatorType.Comma)
            {
                culturalInfo = new CultureInfo("tr-TR", false).NumberFormat;
            }

            response.Text = amount.ToString("N", culturalInfo);
            response.CurrencySymbol = TryGetCurrencySymbol(currencyCode, out var currency) ? currency : null;

            return response;
        }

        public static bool TryGetCurrencySymbol(string ISOCurrencySymbol, out string symbol)
        {

            symbol = CultureInfo
            .GetCultures(CultureTypes.AllCultures)
            .Where(c => !c.IsNeutralCulture)
            .Select(culture =>
            {
                try
                {
                    return new RegionInfo(culture.Name);
                }
                catch
                {
                    return null;
                }
            })
            .Where(ri => ri != null && ri.ISOCurrencySymbol == ISOCurrencySymbol)
            .Select(ri => ri.CurrencySymbol)
            .FirstOrDefault();

            return symbol != null;
        }
    }
}