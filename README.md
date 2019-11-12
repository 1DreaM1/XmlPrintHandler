# XMlPrintSerializer

[![Build Status](https://travis-ci.org/1DreaM1/XmlPrintHandler.svg?branch=master)](https://travis-ci.org/1DreaM1)

Xml Builder pre fiskálnu a nefiskálnu tlač pre driver **TaxIS.dll**.

Vstupné hodnoty:
```csharp
using XmlPrintSerializer;

// POLE POLOŽIEK
string[,] receiptItems = new string[2 /*Musí udávať presný počet položiek --> Inak Error: System.NullReferenceException !*/, 8 /*Nemeniť !*/];
for (int index = 0; index < 2; index++) {
    receiptItems[index, 0] = "Položka 1";   // Názov položky
    receiptItems[index, 1] = "K";           // Typ položky
    receiptItems[index, 2] = "2";           // Množstvo
    receiptItems[index, 3] = "20.00";       // Sadzba dane
    receiptItems[index, 4] = "2.50";        // Jednotková cena
    receiptItems[index, 5] = "5";           // Celková cena
    receiptItems[index, 6] = "5";           // Referenčné ID
    receiptItems[index, 7] = "ks";          // Merná jednotka
}                                           /* !!! Dôležité je zachovať poradie druhých indexov v poli !!! */

XmlHandler xmlHandler = new XmlHandler();
string xml = xmlHandler.BuildXML(new XmlData()
{
    ReceiptLength = ReceiptLength.Normal,   // Počet znakov na riadok --> Normal = 48, Small = 42
    ReceiptTypeValue = "PD",                // Typ dokladu
    UuidValue = "56as4d65a4sd",             // UUID
    AmountValue = "154.24",                 // Celková cena
    TaxBaseBasicValue = "20",               // Základ 20% dane
    TaxBaseReducedValue = "54",             // Základ zníženej (10%) dane
    TaxFreeAmountValue = "5",               // Bez dane
    BasicVatAmountValue = "10",             // Daň (20%)
    ReducedVatAmountValue = "4",            // Znížená (10%) daň
    ParagonNumberValue = "6541",            // Číslo paragónu
    InvoiceNumberValue = "asda6s54d",       // Číslo faktúry
    VystavilValue = "Pre 1",                // Vystavil
    ZakaznikValue = "Zak 1",                // Zákazník
    PayCard = "12",                         // Zaplatené kartou
    PayCash = "845",                        // Zaplatené hotovosťou
    AfterFooterValue = "Ďakujeme",          // Text na konci dokladu
    items = receiptItems                    // Pole položiek
});
```

Vygenerované XML:

```xml
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<RegisterReceiptRequest xmlns:ekasa="http://financnasprava.sk/ekasa/schema/v2">
	<Header />
	<PrintParams 
		BeforeHeader="" 
		AfterHeader="" 
		BeforeFooter="" 
		AfterFooter="Ďakujeme" 
		Vystavil="Pre 1" 
		Zakaznik="Zak 1" 
		Total1="58.00" 
		Total2="30.00" 
		Paycash="845.00" 
		Paycard="12.00" />
	<ReceiptData 
		IssueDate="2019-10-30T16:18:25+01:00" 
		Paragon="true" 
		ParagonNumber="6541" 
		BasicVatAmount="10" 
		ReducedVatAmount="4" 
		TaxBaseBasic="20" 
		TaxBaseReduced="54" 
		TaxFreeAmount="5" 
		Amount="154.24" 
		ReceiptType="PD">
		<Items>
			<Item Name="Položka 1   ks     2.50" ItemType="K" Quantity="2" VatRate="20.00" Price="5.00" />
			<Item Name="Položka 1   ks     2.50" ItemType="K" Quantity="2" VatRate="20.00" Price="5.00" />
		</Items>
	</ReceiptData>
</RegisterReceiptRequest>
```



License
----

Apache-2.0
