# XMlPrintSerializer

[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

```csharp
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



License
----

Apache-2.0
