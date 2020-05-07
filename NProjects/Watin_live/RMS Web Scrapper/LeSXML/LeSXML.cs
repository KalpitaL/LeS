using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Xml;
using System.Configuration;
using System.IO;

namespace LeSXML
{
    public class LeSXML
    {
        public string FilePath
        {
            get
            {
                string _path = ConfigurationSettings.AppSettings["XML_PATH"];
                if (String.IsNullOrEmpty(_path)) { _path = Path.GetDirectoryName(@AppDomain.CurrentDomain.BaseDirectory) + @"\XML"; }
                Directory.CreateDirectory(_path);
                return _path;
            }
        }
        public string FileName { get; set; }
        public string DocID { get; set; }
        public string Created_Date { get; set; } // YYYYMMDD
        public string Doc_Type { get; set; }
        public string Dialect { get; set; }
        public string Version { get; set; }
        public string Date_Document { get; set; }
        public string Date_Preparation { get; set; }    
        public string Time_Preparation { get; set; }
        public string Sender_Code { get; set; }
        public string Recipient_Code { get; set; }
        public string Sender_Name { get; set; }
        public string Recipient_Name { get; set; }
        public string DocReferenceID { get; set; } // MessageRefNumber
        public string DocLinkID { get; set; } //MessageNumber
        public string OrigDocReference { get; set; } // OrigSystemRef, FileName, Link
        public string OrigDocFile { get; set; } // Original document file (Attachment)
        public string DocParentId { get; set; } // Parent of Split
        public string Active { get; set; } // Active
        public string Vessel { get; set; } // Vessel
        public string IMONO { get; set; } //IMO No.
        public string BuyerRef { get; set; } // VRNO        
        public string PortCode { get; set; } // Port Code
        public string PortName { get; set; } // Port Name
        public string Currency { get; set; } // Currency
        public string Date_ETA { get; set; } // Vessel ETA
        public string Date_ETD { get; set; } // Vessel ETD
        public string Date_Delivery { get; set; } // RFQ Reply By // YYYYMMDD
        public string Date_Validity { get; set; } // Quote Validity // YYYYMMDD
        public string LeadTimeDays { get; set; } //LeadTimeDays
        public string Remark_DeliveryTerms { get; set; } // ZTC
        public string Remark_PaymentTerms { get; set; } // ZTP
        public string Remark_Sender { get; set; } // PUR , SUR
        public string Remark_Header { get; set; } // Header remarks
        public string Remark_Title { get; set; } // ZAT
        public string Reference_Document { get; set; } // Quote Ref, POC ref
        public string Total_LineItems { get; set; }
        public string Total_LineItems_Discount { get; set; }
        public string Total_LineItems_Net { get; set; }
        public string Total_Additional_Discount { get; set; }
        public string Total_Freight { get; set; }
        public string Total_Other { get; set; }
        public string Total_Net_Final { get; set; }
        public string Equipment { get; set; }
        public string EquipMaker { get; set; }
        public string EquipType { get; set; }
        public string EquipRemarks { get; set; }
        public LineItemCollection LineItems { get; set; }
        public AddressCollection Addresses { get; set; }        

        public LeSXML()
        {
            LineItems = new LineItemCollection();
            Addresses = new AddressCollection();
            DocID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            DocLinkID = DateTime.Now.ToString("yyyyMMddHHmmssfff");           
            Date_Preparation = DateTime.Now.ToString("yyyyMMdd");
            Date_Document = DateTime.Now.ToString("yyyyMMdd");
            Time_Preparation = DateTime.Now.ToString("HH:mm");
            Created_Date = DateTime.Now.ToString("yyyyMMdd");
            Total_LineItems = "0";
            Total_LineItems_Discount = "0";
            Total_LineItems_Net = "0";
            Total_Additional_Discount = "0";
            Total_Freight = "0";
            Total_Other = "0";
            Total_Net_Final = "0";
            Version = "1";
            Active = "1";           
        }

        public void WriteXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(Doc_Type);
            xmlDoc.AppendChild(rootNode);

            XmlNode Node = xmlDoc.CreateElement("DocumentHeader");
            XmlNode subNode = null;
            System.Reflection.PropertyInfo[] _properties = GetType().GetProperties();

            for (int i = 0; i < _properties.Length; i++)
            {
                string pName = _properties[i].Name;
                if (pName != "LineItems" && pName != "Addresses" && pName != "FilePath" && pName != "FileName")
                {
                    subNode = xmlDoc.CreateElement(_properties[i].Name);
                    if (_properties[i].GetValue(this, null) != null)
                    {
                        subNode.InnerText = _properties[i].GetValue(this, null).ToString();
                    }
                    else { subNode.InnerText = ""; }
                    Node.AppendChild(subNode);
                }
            }

            rootNode.AppendChild(Node);

            Node = xmlDoc.CreateElement("LineItems");
            rootNode.AppendChild(Node);

            for (int i = 0; i < LineItems.Count; i++)
            {
                XmlNode itemNode = xmlDoc.CreateElement("Item");
                _properties = LineItems[i].GetType().GetProperties();
                for (int j = 0; j < _properties.Length; j++)
                {
                    subNode = xmlDoc.CreateElement(_properties[j].Name);
                    if (_properties[j].GetValue(LineItems[i], null) != null)
                    {
                        subNode.InnerText = _properties[j].GetValue(LineItems[i], null).ToString();
                    }
                    else { subNode.InnerText = ""; }
                    itemNode.AppendChild(subNode);
                }
                Node.AppendChild(itemNode);
            }

            rootNode.AppendChild(Node);

            Node = xmlDoc.CreateElement("Addresses");
            rootNode.AppendChild(Node);

            for (int i = 0; i < Addresses.Count; i++)
            {
                XmlNode itemNode = xmlDoc.CreateElement("Address");
                _properties = Addresses[i].GetType().GetProperties();
                for (int j = 0; j < _properties.Length; j++)
                {
                    subNode = xmlDoc.CreateElement(_properties[j].Name);
                    if (_properties[j].GetValue(Addresses[i], null) != null)
                    {
                        subNode.InnerText = _properties[j].GetValue(Addresses[i], null).ToString();
                    }
                    else { subNode.InnerText = ""; }
                    itemNode.AppendChild(subNode);
                }
                Node.AppendChild(itemNode);
            }
            xmlDoc.Save(@FilePath + "\\" + FileName);
        }

        public void ReadXML(string FileName)
        {
            try
            {
                XmlDocument _xmlDoc = new XmlDocument();
                _xmlDoc.Load(FileName);

                this.Doc_Type = Convert.ToString(_xmlDoc.DocumentElement.Name);
                XmlNode _node = _xmlDoc.SelectSingleNode(this.Doc_Type + "/DocumentHeader");
                if (_node != null)
                {
                    System.Reflection.PropertyInfo[] _properties = GetType().GetProperties();
                    for (int i = 0; i < _properties.Length; i++)
                    {
                        if (_properties[i].CanWrite)
                        {
                            string pName = _properties[i].Name;
                            for (int k = 0; k < _node.ChildNodes.Count; k++)
                            {
                                if (_node.ChildNodes[k].Name == pName && _node.ChildNodes[k].InnerText != null)
                                {
                                    _properties[i].SetValue(this, Convert.ToString(_node.ChildNodes[k].InnerText), null);
                                    break;
                                }
                            }
                        }
                    }
                }
                else throw new Exception("Incorrect LeSXML " + this.Doc_Type + " Format !");

                XmlNodeList _nodes = _xmlDoc.SelectNodes(this.Doc_Type + "/LineItems/Item");
                for (int l = 0; l < _nodes.Count; l++)
                {
                    LineItem item = new LineItem();
                    System.Reflection.PropertyInfo[] _properties = item.GetType().GetProperties();
                    for (int j = 0; j < _nodes[l].ChildNodes.Count; j++)
                    {
                        for (int i = 0; i < _properties.Length; i++)
                        {
                            if (_properties[i].CanWrite && _properties[i].Name == _nodes[l].ChildNodes[j].Name)
                            {
                                string value = Convert.ToString(_nodes[l].ChildNodes[j].InnerText);
                                _properties[i].SetValue(item, value, null);
                                break;
                            }
                        }
                    }
                    this.LineItems.Add(item);
                }

                _nodes = _xmlDoc.SelectNodes(this.Doc_Type + "/Addresses/Address");
                for (int l = 0; l < _nodes.Count; l++)
                {
                    Address address = new Address();
                    System.Reflection.PropertyInfo[] _properties = address.GetType().GetProperties();
                    for (int j = 0; j < _nodes[l].ChildNodes.Count; j++)
                    {
                        for (int i = 0; i < _properties.Length; i++)
                        {
                            if (_properties[i].CanWrite && _properties[i].Name == _nodes[l].ChildNodes[j].Name)
                            {
                                _properties[i].SetValue(address, Convert.ToString(_nodes[l].ChildNodes[j].InnerText), null);
                                break;
                            }
                        }
                    }
                    this.Addresses.Add(address);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class Address
    {
        public string Qualifier { get; set; }
        public string AddressName { get; set; }
        public string Identification { get; set; }
        //public string PortCode { get; set; }
        //public string Port { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string eMail { get; set; }
    }

    public class LineItem
    {
        private string _listprice;
        private string _discount;
        private string _netprice;
        private string _itemtotal;
        private string _quantity;

        public string Number { get; set; }  // current doc item no.
        public string OrigItemNumber { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public string Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                _netprice = (Convert.ToDouble(_listprice) - Convert.ToDouble(_discount)).ToString();
                _itemtotal = ((Convert.ToDouble(_listprice) - Convert.ToDouble(_discount)) * Convert.ToDouble(_quantity)).ToString();
            }
        }
        public string Name { get; set; }
        public string ItemRef { get; set; }
        public string Discount
        {
            get { return _discount; }
            set
            {
                _discount = value; _netprice = (Convert.ToDouble(_listprice) - Convert.ToDouble(_discount)).ToString();
                _itemtotal = ((Convert.ToDouble(_listprice) - Convert.ToDouble(_discount)) * Convert.ToDouble(Quantity)).ToString();
            }
        }
        public string ListPrice
        {
            get { return _listprice; }
            set
            {
                _listprice = value;
                _netprice = (Convert.ToDouble(_listprice) - Convert.ToDouble(_discount)).ToString();
                _itemtotal = ((Convert.ToDouble(_listprice) - Convert.ToDouble(_discount)) * Convert.ToDouble(Quantity)).ToString();
            }
        }
        public string NetPrice { get { return _netprice; } }
        public string ItemTotal { get { return _itemtotal; } }
        public string Remark { get; set; }
        public string LeadDays { get; set; }
        public string Equipment { get; set; }
        public string EquipMaker { get; set; }
        public string EquipType { get; set; }
        public string EquipRemarks { get; set; }
        public string OriginatingSystemRef { get; set; }
        public string SystemRef { get; set; }

        public LineItem()
        {
            ListPrice = "0";
            Discount = "0";
            LeadDays = "0";
            Quantity = "0";          
        }
    }

    public class LineItemCollection : System.Collections.ObjectModel.Collection<LineItem>
    {
        public virtual void InsertItem(LineItem item)
        {
            this.Add(item);
        }
    }

    public class AddressCollection : System.Collections.ObjectModel.Collection<Address>
    {
        public virtual void InsertAddress(Address _address)
        {
            this.Add(_address);
        }
    }
}