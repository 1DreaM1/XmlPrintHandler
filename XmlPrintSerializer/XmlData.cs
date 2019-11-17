using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlPrintHandler
{
    public struct XmlData
    {
        /// <summary>
        /// Počet znakov na riadok --> Normal = 48, Small = 42
        /// </summary>
        public ReceiptLength ReceiptLength { get; set; }

        /// <summary>
        /// Reverzná tlač ceny dokladu
        /// </summary>
        public Reverse UseReverse { get; set; }

        /// <summary>
        /// Typ dokladu
        /// </summary>
        public string ReceiptTypeValue { get; set; }

        /// <summary>
        /// Int. číslo dokladu
        /// </summary>
        public string ReceiptNumberValue { get; set; }

        /// <summary>
        /// UUID
        /// </summary>
        public string UuidValue { get; set; }

        /// <summary>
        /// Celková cena
        /// </summary>
        public string AmountValue { get; set; }

        /// <summary>
        /// Základ 20% dane
        /// </summary>
        public string TaxBaseBasicValue { get; set; }

        /// <summary>
        /// Základ zníženej (10%) dane
        /// </summary>
        public string BasicVatAmountValue { get; set; }

        /// <summary>
        /// Bez dane
        /// </summary>
        public string ReducedVatAmountValue { get; set; }

        /// <summary>
        /// Daň (20%)
        /// </summary>
        public string TaxBaseReducedValue { get; set; }

        /// <summary>
        /// Znížená (10%) daň
        /// </summary>
        public string TaxFreeAmountValue { get; set; }

        /// <summary>
        /// Číslo paragónu
        /// </summary>
        public string ParagonNumberValue { get; set; }

        /// <summary>
        /// Číslo faktúry
        /// </summary>
        public string InvoiceNumberValue { get; set; }

        /// <summary>
        /// Výmenný poukaz
        /// </summary>
        public string VoucherValue { get; set; }


        /// <summary>
        /// Vystavil
        /// </summary>
        public string VystavilValue { get; set; }

        /// <summary>
        /// Zákazník
        /// </summary>
        public string ZakaznikValue { get; set; }


        /// <summary>
        /// Pole položiek
        /// </summary>
        public string[,] items { get; set; }

        /// <summary>
        /// Zaplatené hotovosťou
        /// </summary>
        public string PayCash { get; set; }

        /// <summary>
        /// Zaplatené kartou
        /// </summary>
        public string PayCard { get; set; }

        /// <summary>
        /// Zaplatené stravenkami
        /// </summary>
        public string PayStr { get; set; }

        /// <summary>
        /// Zaplatené výmenným poukazom
        /// </summary>
        public string PayVmp { get; set; }

        /// <summary>
        /// Text pred hlavičkou dokladu
        /// </summary>
        public string BeforeHeaderValue { get; set; }

        /// <summary>
        /// Text za hlavičkou dokladu
        /// </summary>
        public string AfterHeaderValue { get; set; }

        /// <summary>
        /// Text pred pätičkou dokladu
        /// </summary>
        public string BeforeFooterValue { get; set; }

        /// <summary>
        /// Text na konci dokladu
        /// </summary>
        public string AfterFooterValue { get; set; }
    }
}
