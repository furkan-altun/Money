# **MoneyHelper**


- The nuget package enables to user to get Money model which includes integral, decimal, whole number, currency code, currency symbol, and formatted string amount by different cultural appearances of amount.
- Money package has FormatDecimalAmountToMoney method for converting decimal to Money model.
- It has different overloaded methods which has method parameters like amount, currencyCode, nDigit and seperatorType.

## **Method parametrers**
**amount:** decimal amount value like 1034.56789 \
**currencyCode:**  currency code of amount (It can sent null or empty string). The currency code determines the currency symbol of the Money class. Also, the model includes the currency code value that you send. \
**nDigit:** It determines the length of decimal part. If you don't send or default you get whole decimal part of amount. \
**seperatorType:**  seperatorType determines Text value format. ex: 1,043.567 (DecimalSeperatorType.Dot) or 1.043,567 (DecimalSeperatorType.Comma)

## **Response Model**
**IntegralPart:** Integral part of a decimal amount ex: 1034.567 --> 1034  \
**DecimalPart:** Integral part of a decimal amount. ex: 1034.567 --> 567 It depends on nDigit parameter of FormatDecimalAmountToMoney method, if you send 2 to the nDigit, you get 56.  \
**CurrencyCode:** It is created for the integrity of the money model. Also, it determines the currency symbol. If you send USD, you can get $ sign.  \
**CurrencySymbol:** The currency symbol of currency code. It is determined by currency code value.  \
**Amount:** Whole number of amount value.  \
**Text:** Formatted string value of amount. It depends on DecimalSeperatorType Enum Value. \
Default value is comma seperator of integral part, comma seperator of numbers. ex: 1.034,567  \
Text value can be changable by dot seperator of decimal part, comma seperator of numbes. ex: 1,034.567 


## **Demo:**

```
using MoneyHelper;
using Newtonsoft.Json;

var amount = 1034.5678m;
var moneyModel = Money.FormatDecimalAmountToMoney(amount, "USD");
var moneyModelByCommaSeparator = Money.FormatDecimalAmountToMoney(amount, "USD", Money.DecimalSeperatorType.Dot);

var jsonData = JsonConvert.SerializeObject(moneyModel);
var jsonDataV2 = JsonConvert.SerializeObject(moneyModelByCommaSeparator);

Console.WriteLine(jsonData);
Console.WriteLine(jsonDataV2);
```

## **Result:**

```yaml
{
  "IntegralPart": 1034,
  "DecimalPart": 5678,
  "CurrencyCode": "USD",
  "CurrencySymbol": "$",
  "Amount": 1034.5678,
  "Text": "1.034,568"
}

{
  "IntegralPart": 1034,
  "DecimalPart": 5678,
  "CurrencyCode": "USD",
  "CurrencySymbol": "$",
  "Amount": 1034.5678,
  "Text": "1,034.568"
}
```
