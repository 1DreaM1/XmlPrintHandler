using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml;

namespace XmlPrintSerializer
{
    /// <summary>
    /// XmlPrintSerializer, Generovanie XML pre tlač fiškálneho dokadu
    /// </summary>
    public class XmlHandler
    {
        /*
         * 
         * XmlPrintSerializer
         * Powered by @DreaM (Daniel Kristl)
         * (https://dream-official.ml/)
         * Email: dakristl123@gmail.com
         * 
         */

        public XmlHandler()
        {
            SetCulture();
        }

        internal void SetCulture()
        {
            CultureInfo nonInvariantCulture = new CultureInfo("en-US");
            nonInvariantCulture.NumberFormat.NumberDecimalSeparator = ".";
            //nonInvariantCulture.NumberFormat.NumberGroupSeparator = " ";
            Thread.CurrentThread.CurrentCulture = nonInvariantCulture;
        }

        /// <summary>
        /// Generuje XML pripravené pre odoslanie na tlač
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public string BuildXML(XmlData xmlData)
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            try
            {
                string namespaceValue = "http://financnasprava.sk/ekasa/schema/v2";
                XmlElement RegisterReceiptRequest = doc.CreateElement("RegisterReceiptRequest");
                XmlAttribute ekasa = doc.CreateAttribute("xmlns:ekasa");
                ekasa.Value = namespaceValue;
                RegisterReceiptRequest.Attributes.Append(ekasa);
                doc.AppendChild(RegisterReceiptRequest);

                XmlElement Header = doc.CreateElement("Header");
                RegisterReceiptRequest.AppendChild(Header);

                XmlElement PrintParams = doc.CreateElement("PrintParams");

                if (xmlData.BeforeHeaderValue != null || xmlData.BeforeHeaderValue != String.Empty) {
                    XmlAttribute BeforeHeader = doc.CreateAttribute("BeforeHeader");
                    BeforeHeader.Value = xmlData.BeforeHeaderValue;
                    PrintParams.Attributes.Append(BeforeHeader);
                }

                if (xmlData.AfterHeaderValue != null || xmlData.AfterHeaderValue != String.Empty)
                {
                    XmlAttribute AfterHeader = doc.CreateAttribute("AfterHeader");
                    AfterHeader.Value = xmlData.AfterHeaderValue;
                    PrintParams.Attributes.Append(AfterHeader);
                }

                if (xmlData.BeforeFooterValue != null || xmlData.BeforeFooterValue != String.Empty)
                {
                    XmlAttribute BeforeFooter = doc.CreateAttribute("BeforeFooter");
                    BeforeFooter.Value = xmlData.BeforeFooterValue;
                    PrintParams.Attributes.Append(BeforeFooter);
                }

                if (xmlData.AfterFooterValue != null || xmlData.AfterFooterValue != String.Empty)
                {
                    XmlAttribute AfterFooter = doc.CreateAttribute("AfterFooter");
                    AfterFooter.Value = xmlData.AfterFooterValue;
                    PrintParams.Attributes.Append(AfterFooter);
                }

                if (xmlData.VystavilValue != null || xmlData.VystavilValue != String.Empty)
                {
                    XmlAttribute Vystavil = doc.CreateAttribute("Vystavil");
                    Vystavil.Value = xmlData.VystavilValue;
                    PrintParams.Attributes.Append(Vystavil);
                }

                if (xmlData.ZakaznikValue != null || xmlData.ZakaznikValue != String.Empty)
                {
                    XmlAttribute Zakaznik = doc.CreateAttribute("Zakaznik");
                    Zakaznik.Value = xmlData.ZakaznikValue;
                    PrintParams.Attributes.Append(Zakaznik);
                }

                XmlAttribute Total1 = doc.CreateAttribute("Total1");
                double baseReduced = xmlData.TaxBaseReducedValue.Equals(String.Empty) ? 0.00 : double.Parse(xmlData.TaxBaseReducedValue, CultureInfo.InvariantCulture);
                double vatReduced = xmlData.ReducedVatAmountValue.Equals(String.Empty) ? 0.00 : double.Parse(xmlData.ReducedVatAmountValue, CultureInfo.InvariantCulture);
                Total1.Value = (baseReduced + vatReduced).ToString("N2").Replace(",", "");

                XmlAttribute Total2 = doc.CreateAttribute("Total2");
                double baseNormal = xmlData.TaxBaseBasicValue.Equals(String.Empty) ? 0.00 : double.Parse(xmlData.TaxBaseBasicValue, CultureInfo.InvariantCulture);
                double vatNormal = xmlData.BasicVatAmountValue.Equals(String.Empty) ? 0.00 : double.Parse(xmlData.BasicVatAmountValue, CultureInfo.InvariantCulture);
                Total2.Value = (baseNormal + vatNormal).ToString("N2").Replace(",", "");

                PrintParams.Attributes.Append(Total1);
                PrintParams.Attributes.Append(Total2);

                if (xmlData.PayCash != null || xmlData.PayCash != String.Empty)
                {
                    XmlAttribute PayCash = doc.CreateAttribute("Paycash");
                    PayCash.Value = double.Parse(xmlData.PayCash, CultureInfo.InvariantCulture).ToString("N2").Replace(",", "");
                    PrintParams.Attributes.Append(PayCash);
                }

                if (xmlData.PayCard != null || xmlData.PayCard != String.Empty)
                {
                    XmlAttribute PayCard = doc.CreateAttribute("Paycard");
                    PayCard.Value = double.Parse(xmlData.PayCard, CultureInfo.InvariantCulture).ToString("N2").Replace(",", "");
                    PrintParams.Attributes.Append(PayCard);
                }

                RegisterReceiptRequest.AppendChild(PrintParams);

                XmlElement ReceiptData = doc.CreateElement("ReceiptData");

                XmlAttribute Amount = doc.CreateAttribute("Amount");
                Amount.Value = xmlData.AmountValue;
                XmlAttribute ReceiptType = doc.CreateAttribute("ReceiptType");
                ReceiptType.Value = xmlData.ReceiptTypeValue;

                if (xmlData.ReceiptTypeValue.Equals("VK") || xmlData.ReceiptTypeValue.Equals("VY")) { }
                else if (xmlData.ReceiptTypeValue.Equals("UF"))
                {
                    XmlAttribute InvoiceNumber = doc.CreateAttribute("InvoiceNumber");
                    InvoiceNumber.Value = xmlData.InvoiceNumberValue;
                    ReceiptData.Attributes.Append(InvoiceNumber);

                    if (xmlData.ParagonNumberValue != String.Empty)
                    {
                        XmlAttribute IssueDate = doc.CreateAttribute("IssueDate");
                        IssueDate.Value = getDate();
                        XmlAttribute Paragon = doc.CreateAttribute("Paragon");
                        Paragon.Value = "true";
                        XmlAttribute ParagonNumber = doc.CreateAttribute("ParagonNumber");
                        ParagonNumber.Value = xmlData.ParagonNumberValue;

                        ReceiptData.Attributes.Append(IssueDate);
                        ReceiptData.Attributes.Append(Paragon);
                        ReceiptData.Attributes.Append(ParagonNumber);
                    }
                }
                else
                {
                    XmlAttribute BasicVatAmount = doc.CreateAttribute("BasicVatAmount");
                    BasicVatAmount.Value = xmlData.BasicVatAmountValue;
                    XmlAttribute ReducedVatAmount = doc.CreateAttribute("ReducedVatAmount");
                    ReducedVatAmount.Value = xmlData.ReducedVatAmountValue;
                    XmlAttribute TaxBaseBasic = doc.CreateAttribute("TaxBaseBasic");
                    TaxBaseBasic.Value = xmlData.TaxBaseBasicValue;
                    XmlAttribute TaxBaseReduced = doc.CreateAttribute("TaxBaseReduced");
                    TaxBaseReduced.Value = xmlData.TaxBaseReducedValue;
                    XmlAttribute TaxFreeAmount = doc.CreateAttribute("TaxFreeAmount");
                    TaxFreeAmount.Value = xmlData.TaxFreeAmountValue;

                    if (xmlData.ParagonNumberValue != String.Empty)
                    {
                        XmlAttribute IssueDate = doc.CreateAttribute("IssueDate");
                        IssueDate.Value = getDate();
                        XmlAttribute Paragon = doc.CreateAttribute("Paragon");
                        Paragon.Value = "true";
                        XmlAttribute ParagonNumber = doc.CreateAttribute("ParagonNumber");
                        ParagonNumber.Value = xmlData.ParagonNumberValue;

                        ReceiptData.Attributes.Append(IssueDate);
                        ReceiptData.Attributes.Append(Paragon);
                        ReceiptData.Attributes.Append(ParagonNumber);
                    }

                    ReceiptData.Attributes.Append(BasicVatAmount);
                    ReceiptData.Attributes.Append(ReducedVatAmount);
                    ReceiptData.Attributes.Append(TaxBaseBasic);
                    ReceiptData.Attributes.Append(TaxBaseReduced);
                    ReceiptData.Attributes.Append(TaxFreeAmount);
                }

                ReceiptData.Attributes.Append(Amount);
                ReceiptData.Attributes.Append(ReceiptType);

                XmlElement Items = doc.CreateElement("Items");
                ReceiptData.AppendChild(Items);

                int lineLength = 72;
                switch (xmlData.ReceiptLength)
                {
                    case ReceiptLength.Small:
                        lineLength = 63;
                        break;
                }

                for (int i = 0; i < xmlData.items.GetLength(0); i++)
                {
                    XmlElement Item = doc.CreateElement("Item");

                    XmlAttribute Name = doc.CreateAttribute("Name");
                    Name.Value = GetLine(xmlData.items[i, 0], GetLine(xmlData.items[i, 7], xmlData.items[i, 4], 11), lineLength);
                    XmlAttribute ItemType = doc.CreateAttribute("ItemType");
                    ItemType.Value = xmlData.items[i, 1];
                    XmlAttribute Quantity = doc.CreateAttribute("Quantity");
                    Quantity.Value = xmlData.items[i, 2];
                    XmlAttribute VatRate = doc.CreateAttribute("VatRate");
                    VatRate.Value = xmlData.items[i, 3];

                    if (xmlData.items[i, 1].Equals("V"))
                    {
                        XmlAttribute ReferenceReceiptId = doc.CreateAttribute("ReferenceReceiptId");
                        ReferenceReceiptId.Value = xmlData.items[i, 6];
                        Item.Attributes.Append(ReferenceReceiptId);
                    }

                    XmlAttribute Price = doc.CreateAttribute("Price");
                    Price.Value = countPrice(xmlData.items[i, 4], xmlData.items[i, 2]);

                    Item.Attributes.Append(Name);
                    Item.Attributes.Append(ItemType);
                    Item.Attributes.Append(Quantity);
                    Item.Attributes.Append(VatRate);
                    Item.Attributes.Append(Price);

                    Items.AppendChild(Item);
                }

                RegisterReceiptRequest.AppendChild(ReceiptData);

                StringWriter stringWriter = new StringWriter();
                XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);

                doc.WriteTo(xmlTextWriter);
                return stringWriter.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + Environment.NewLine + e.GetType());
                return String.Empty;
            }
        }

        internal static string countPrice(string priceString, string qtyString)
        {
            try
            {
                double price = double.Parse(priceString, CultureInfo.InvariantCulture);
                double qty = double.Parse(qtyString, CultureInfo.InvariantCulture);

                double count = price * qty;
                return count.ToString("N2").Replace(",", "");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + Environment.NewLine + e.GetType());
                return String.Empty;
            }
        }

        internal string GetLine(string leftText, string rightText, int lineHeight)
        {
            int betweenTextLine = lineHeight - (leftText.Length + rightText.Length);
            string line = leftText;

            if (betweenTextLine < 0)
            {
                double i = leftText.Length / lineHeight;
                int overflowTextLength = leftText.Length - ((int)Math.Floor(i) * lineHeight);
                betweenTextLine = lineHeight - (overflowTextLength + rightText.Length);
            }

            for (int x = 0; x < betweenTextLine; x++)
            {
                line += " ";
            }
            line += rightText;
            return line;
        }

        internal string getDate()
        {
            DateTime date = DateTime.Now;
            return date.ToString("yyyy-MM-ddTHH:mm:ssK");
        }
    }
}
