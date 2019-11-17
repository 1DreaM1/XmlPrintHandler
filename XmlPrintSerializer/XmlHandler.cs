using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml;

namespace XmlPrintHandler
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
                                                
                if (xmlData.BeforeHeaderValue != null)
                {
                    XmlAttribute BeforeHeader = doc.CreateAttribute("BeforeHeader");
                    BeforeHeader.Value = xmlData.BeforeHeaderValue;
                    PrintParams.Attributes.Append(BeforeHeader);
                }

                if (xmlData.AfterHeaderValue != null)
                {
                    XmlAttribute AfterHeader = doc.CreateAttribute("AfterHeader");
                    AfterHeader.Value = xmlData.AfterHeaderValue;
                    PrintParams.Attributes.Append(AfterHeader);
                }

                if (xmlData.BeforeFooterValue != null)
                {
                    XmlAttribute BeforeFooter = doc.CreateAttribute("BeforeFooter");
                    BeforeFooter.Value = xmlData.BeforeFooterValue;
                    PrintParams.Attributes.Append(BeforeFooter);
                }

                if (xmlData.AfterFooterValue != null)
                {
                    XmlAttribute AfterFooter = doc.CreateAttribute("AfterFooter");
                    AfterFooter.Value = "\x0A" + xmlData.AfterFooterValue + "\x0A";
                    PrintParams.Attributes.Append(AfterFooter);
                }

                if (xmlData.VystavilValue != null)
                {
                    XmlAttribute Vystavil = doc.CreateAttribute("Vystavil");
                    Vystavil.Value = "Vystavil: " + xmlData.VystavilValue;
                    PrintParams.Attributes.Append(Vystavil);
                }

                if (xmlData.ZakaznikValue != null)
                {
                    XmlAttribute Zakaznik = doc.CreateAttribute("Zakaznik");
                    Zakaznik.Value = "Zákazník: " + xmlData.ZakaznikValue;
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

                if (xmlData.PayCash != null)
                {
                    XmlAttribute PayCash = doc.CreateAttribute("Paycash");
                    PayCash.Value = xmlData.PayCash;
                    PrintParams.Attributes.Append(PayCash);
                }

                if (xmlData.PayCard != null)
                {
                    XmlAttribute PayCard = doc.CreateAttribute("Paycard");
                    PayCard.Value = xmlData.PayCard;
                    PrintParams.Attributes.Append(PayCard);
                }

                if (xmlData.PayStr != null)
                {
                    XmlAttribute PayStr = doc.CreateAttribute("Paystr");
                    PayStr.Value = xmlData.PayStr;
                    PrintParams.Attributes.Append(PayStr);
                }

                if (xmlData.PayVmp != null)
                {
                    XmlAttribute PayVmp = doc.CreateAttribute("Payvmp");
                    PayVmp.Value = xmlData.PayVmp;
                    PrintParams.Attributes.Append(PayVmp);
                }

                XmlAttribute IntCisloDokladu = doc.CreateAttribute("IntCisloDokladu");
                IntCisloDokladu.Value = xmlData.ReceiptNumberValue;
                PrintParams.Attributes.Append(IntCisloDokladu);

                if (xmlData.UseReverse == Reverse.Enable)
                {
                    XmlAttribute ReverseOn = doc.CreateAttribute("UseReverse");
                    ReverseOn.Value = "\r";
                    PrintParams.Attributes.Append(ReverseOn);
                }

                if (xmlData.VoucherValue != null)
                {
                    XmlAttribute Voucher = doc.CreateAttribute("VoucherNumber");
                    Voucher.Value = xmlData.VoucherValue;
                    PrintParams.Attributes.Append(Voucher);
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

                for (int i = 0; i < xmlData.items.GetLength(0); i++)
                {
                    XmlElement Item = doc.CreateElement("Item");

                    XmlAttribute Name = doc.CreateAttribute("Name");
                    Name.Value = xmlData.items[i, 0];
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
                    Price.Value = xmlData.items[i, 5];

                    Item.Attributes.Append(Name);
                    Item.Attributes.Append(ItemType);
                    Item.Attributes.Append(Quantity);
                    Item.Attributes.Append(VatRate);
                    Item.Attributes.Append(Price);

                    XmlElement ItemPrintParams = doc.CreateElement("ItemPrintParams");

                    XmlAttribute MJ = doc.CreateAttribute("MJ");
                    MJ.Value = xmlData.items[i, 7];
                    ItemPrintParams.Attributes.Append(MJ);

                    XmlAttribute JCena = doc.CreateAttribute("JCena");
                    JCena.Value = xmlData.items[i, 4];
                    ItemPrintParams.Attributes.Append(JCena);

                    Item.AppendChild(ItemPrintParams);

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
                return e.Message + e.StackTrace;
            }
        }
        
        internal string getDate()
        {
            DateTime date = DateTime.Now;
            return date.ToString("yyyy-MM-ddTHH:mm:ssK");
        }
    }
}
