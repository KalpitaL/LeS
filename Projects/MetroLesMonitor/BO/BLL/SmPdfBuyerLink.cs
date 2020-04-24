namespace MetroLesMonitor.Bll
{   
    public partial class SmPdfBuyerLink {
        
        private System.Nullable<int> _mapId;
        
        private System.Nullable<int> _pdfMapid;
        
        private System.Nullable<int> _buyerSuppLinkid;
        
        private System.Nullable<int> _buyerid;
        
        private System.Nullable<int> _supplierid;
        
        private string _docType;
        
        private string _mapping1;
        
        private string _mapping1Value;
        
        private string _mapping2;
        
        private string _mapping2Value;
        
        private string _mapping3;
        
        private string _mapping3Value;

        private string _formatmapcode;//added by kalpita on 19/01/2017
        
        public virtual System.Nullable<int> MapId {
            get {
                return _mapId;
            }
            set {
                _mapId = value;
            }
        }
        
        public virtual System.Nullable<int> PdfMapid {
            get {
                return _pdfMapid;
            }
            set {
                _pdfMapid = value;
            }
        }
        
        public virtual System.Nullable<int> BuyerSuppLinkid {
            get {
                return _buyerSuppLinkid;
            }
            set {
                _buyerSuppLinkid = value;
            }
        }
        
        public virtual System.Nullable<int> Buyerid {
            get {
                return _buyerid;
            }
            set {
                _buyerid = value;
            }
        }
        
        public virtual System.Nullable<int> Supplierid {
            get {
                return _supplierid;
            }
            set {
                _supplierid = value;
            }
        }
        
        public virtual string DocType {
            get {
                return _docType;
            }
            set {
                _docType = value;
            }
        }
        
        public virtual string Mapping1 {
            get {
                return _mapping1;
            }
            set {
                _mapping1 = value;
            }
        }
        
        public virtual string Mapping1Value {
            get {
                return _mapping1Value;
            }
            set {
                _mapping1Value = value;
            }
        }
        
        public virtual string Mapping2 {
            get {
                return _mapping2;
            }
            set {
                _mapping2 = value;
            }
        }
        
        public virtual string Mapping2Value {
            get {
                return _mapping2Value;
            }
            set {
                _mapping2Value = value;
            }
        }
        
        public virtual string Mapping3 {
            get {
                return _mapping3;
            }
            set {
                _mapping3 = value;
            }
        }
        
        public virtual string Mapping3Value {
            get {
                return _mapping3Value;
            }
            set {
                _mapping3Value = value;
            }
        }

        public virtual string FormatMapCode
        {
            get
            {
                return _formatmapcode;
            }
            set
            {
                _formatmapcode = value;
            }
        }
        
        private void Clean() {
            this.MapId = null;
            this.PdfMapid = null;
            this.BuyerSuppLinkid = null;
            this.Buyerid = null;
            this.Supplierid = null;
            this.DocType = string.Empty;
            this.Mapping1 = string.Empty;
            this.Mapping1Value = string.Empty;
            this.Mapping2 = string.Empty;
            this.Mapping2Value = string.Empty;
            this.Mapping3 = string.Empty;
            this.Mapping3Value = string.Empty;
            this.FormatMapCode = string.Empty;
        }
        
        private void Fill(System.Data.DataRow dr) {
            this.Clean();
            if ((dr["MAP_ID"] != System.DBNull.Value)) {
                this.MapId = ((System.Nullable<int>)(dr["MAP_ID"]));
            }
            if ((dr["PDF_MAPID"] != System.DBNull.Value)) {
                this.PdfMapid = ((System.Nullable<int>)(dr["PDF_MAPID"]));
            }
            if ((dr["BUYER_SUPP_LINKID"] != System.DBNull.Value)) {
                this.BuyerSuppLinkid = ((System.Nullable<int>)(dr["BUYER_SUPP_LINKID"]));
            }
            if ((dr["BUYERID"] != System.DBNull.Value)) {
                this.Buyerid = ((System.Nullable<int>)(dr["BUYERID"]));
            }
            if ((dr["SUPPLIERID"] != System.DBNull.Value)) {
                this.Supplierid = ((System.Nullable<int>)(dr["SUPPLIERID"]));
            }
            if ((dr["DOC_TYPE"] != System.DBNull.Value)) {
                this.DocType = ((string)(dr["DOC_TYPE"]));
            }
            if ((dr["MAPPING_1"] != System.DBNull.Value)) {
                this.Mapping1 = ((string)(dr["MAPPING_1"]));
            }
            if ((dr["MAPPING_1_VALUE"] != System.DBNull.Value)) {
                this.Mapping1Value = ((string)(dr["MAPPING_1_VALUE"]));
            }
            if ((dr["MAPPING_2"] != System.DBNull.Value)) {
                this.Mapping2 = ((string)(dr["MAPPING_2"]));
            }
            if ((dr["MAPPING_2_VALUE"] != System.DBNull.Value)) {
                this.Mapping2Value = ((string)(dr["MAPPING_2_VALUE"]));
            }
            if ((dr["MAPPING_3"] != System.DBNull.Value)) {
                this.Mapping3 = ((string)(dr["MAPPING_3"]));
            }
            if ((dr["MAPPING_3_VALUE"] != System.DBNull.Value)) {
                this.Mapping3Value = ((string)(dr["MAPPING_3_VALUE"]));
            }
            if ((dr["FORMAT_MAP_CODE"] != System.DBNull.Value))  {
                this.FormatMapCode = ((string)(dr["FORMAT_MAP_CODE"]));
            }
        }
        
        public static SmPdfBuyerLinkCollection GetAll() {
            MetroLesMonitor.Dal.SmPdfBuyerLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmPdfBuyerLink();
                System.Data.DataSet ds = dbo.SM_PDF_BUYER_LINK_Select_All();
                SmPdfBuyerLinkCollection collection = new SmPdfBuyerLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds)) {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1)) {
                        SmPdfBuyerLink obj = new SmPdfBuyerLink();
                        obj.Fill(ds.Tables[0].Rows[i]);
                        if ((obj != null)) {
                            collection.Add(obj);
                        }
                    }
                }
                return collection;
            }
            catch (System.Exception ) {
                throw;
            }
            finally {
                if ((dbo != null)) {
                    dbo.Dispose();
                }
            }
        }

        public static System.Data.DataSet GetPDFBuyerLinks()
        {
            MetroLesMonitor.Dal.SmPdfBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfBuyerLink();
                //System.Data.DataSet ds = dbo.Select_SMV_PDF_BUYER_LINK_All();
                System.Data.DataSet ds = dbo.Select_SMV_PDF_BUYER_LINK_All_New();
                if (GlobalTools.IsSafeDataSet(ds)) return ds;
                else return null;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if ((dbo != null))
                {
                    dbo.Dispose();
                }
            }
        }

        public static System.Data.DataSet GetDocTypes()
        {
            MetroLesMonitor.Dal.SmPdfBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfBuyerLink();
                System.Data.DataSet ds = dbo.Select_All_DocTypes();
                if (GlobalTools.IsSafeDataSet(ds)) return ds;
                else return null;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if ((dbo != null))
                {
                    dbo.Dispose();
                }
            }
        }
        
        public static SmPdfBuyerLink Load(System.Nullable<int> MAP_ID) {
            MetroLesMonitor.Dal.SmPdfBuyerLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmPdfBuyerLink();
                System.Data.DataSet ds = dbo.SM_PDF_BUYER_LINK_Select_One(MAP_ID);
                SmPdfBuyerLink obj = null;
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        obj = new SmPdfBuyerLink();
                        obj.Fill(ds.Tables[0].Rows[0]);
                    }
                }
                return obj;
            }
            catch (System.Exception ) {
                throw;
            }
            finally {
                if ((dbo != null)) {
                    dbo.Dispose();
                }
            }
        }

        public static SmPdfBuyerLink LoadByPDFMapId(System.Nullable<int> PDF_MAPID)
        {
            MetroLesMonitor.Dal.SmPdfBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfBuyerLink();
                System.Data.DataSet ds = dbo.SM_PDF_BUYER_LINK_Select_One_By_PDF_MAPID(PDF_MAPID);
                SmPdfBuyerLink obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmPdfBuyerLink();
                        obj.Fill(ds.Tables[0].Rows[0]);
                    }
                }
                return obj;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if ((dbo != null))
                {
                    dbo.Dispose();
                }
            }
        }
        
        public virtual void Load() {
            MetroLesMonitor.Dal.SmPdfBuyerLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmPdfBuyerLink();
                System.Data.DataSet ds = dbo.SM_PDF_BUYER_LINK_Select_One(this.MapId);
                if (GlobalTools.IsSafeDataSet(ds)) {
                    if ((ds.Tables[0].Rows.Count > 0)) {
                        this.Fill(ds.Tables[0].Rows[0]);
                    }
                }
            }
            catch (System.Exception ) {
                throw;
            }
            finally {
                if ((dbo != null)) {
                    dbo.Dispose();
                }
            }
        }
        
        public virtual void Insert() {
            MetroLesMonitor.Dal.SmPdfBuyerLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmPdfBuyerLink();
                dbo.SM_PDF_BUYER_LINK_Insert(this.MapId, this.PdfMapid, this.BuyerSuppLinkid, this.Buyerid, this.Supplierid, this.DocType, this.Mapping1, this.Mapping1Value, this.Mapping2, this.Mapping2Value, this.Mapping3, this.Mapping3Value, this.FormatMapCode);
            }
            catch (System.Exception ) {
                throw;
            }
            finally {
                if ((dbo != null)) {
                    dbo.Dispose();
                }
            }
        }

        public virtual void Insert(MetroLesMonitor.Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmPdfBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfBuyerLink(_dataAccess);
                dbo.SM_PDF_BUYER_LINK_Insert(this.MapId, this.PdfMapid, this.BuyerSuppLinkid, this.Buyerid, this.Supplierid, this.DocType, this.Mapping1, this.Mapping1Value, this.Mapping2, this.Mapping2Value, this.Mapping3, this.Mapping3Value,this.FormatMapCode);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        
        public virtual void Delete() {
            MetroLesMonitor.Dal.SmPdfBuyerLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmPdfBuyerLink();
                dbo.SM_PDF_BUYER_LINK_Delete(this.MapId);
            }
            catch (System.Exception ) {
                throw;
            }
            finally {
                if ((dbo != null)) {
                    dbo.Dispose();
                }
            }
        }

        public virtual void Delete(Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmPdfBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfBuyerLink(_dataAccess);
                dbo.SM_PDF_BUYER_LINK_Delete(this.MapId);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        
        public virtual void Update() {
            MetroLesMonitor.Dal.SmPdfBuyerLink dbo = null;
            try {
                dbo = new MetroLesMonitor.Dal.SmPdfBuyerLink();
                dbo.SM_PDF_BUYER_LINK_Update(this.MapId, this.PdfMapid, this.BuyerSuppLinkid, this.Buyerid, this.Supplierid, this.DocType, this.Mapping1, this.Mapping1Value, this.Mapping2, this.Mapping2Value, this.Mapping3, this.Mapping3Value, this.FormatMapCode);
            }
            catch (System.Exception ) {
                throw;
            }
            finally {
                if ((dbo != null)) {
                    dbo.Dispose();
                }
            }
        }

        public virtual void Update(MetroLesMonitor.Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmPdfBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfBuyerLink(_dataAccess);
                dbo.SM_PDF_BUYER_LINK_Update(this.MapId, this.PdfMapid, this.BuyerSuppLinkid, this.Buyerid, this.Supplierid, this.DocType, this.Mapping1, this.Mapping1Value, this.Mapping2, this.Mapping2Value, this.Mapping3, this.Mapping3Value,this.FormatMapCode);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        //ADDED BY KALPITA ON 04/01/2018
        public static System.Data.DataSet Select_SM_PDF_BUYER_LINK_AddressId(string ADDRESSID, string ADDRTYPE)
        {
            MetroLesMonitor.Dal.SmPdfBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfBuyerLink();
                System.Data.DataSet ds = dbo.SM_PDF_BUYER_LINK_AddressId(ADDRESSID, ADDRTYPE);
                return ds;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if ((dbo != null))
                {
                    dbo.Dispose();
                }
            }
        }

        public static string GetNextFormatMapCode(string DOCTYPE, string FORMAT_MAPCODE,Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmPdfBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfBuyerLink(_dataAccess);
                return dbo.GetFormatMapCode(DOCTYPE, FORMAT_MAPCODE);
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if (_dataAccess == null)
                {
                    if ((dbo != null))
                    {
                        dbo.Dispose();
                    }
                }
            }
        }

        public static SmPdfBuyerLinkCollection Get_SM_PDF_BUYER_LINK_AddressId(string ADDRESSID, string ADDRTYPE)
        {
            try
            {
                System.Data.DataSet ds = SmPdfBuyerLink.Select_SM_PDF_BUYER_LINK_AddressId(ADDRESSID, ADDRTYPE);
                SmPdfBuyerLinkCollection collection = new SmPdfBuyerLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmPdfBuyerLink obj = new SmPdfBuyerLink();
                        obj.Fill(ds.Tables[0].Rows[i]);
                        if ((obj != null))
                        {
                            collection.Add(obj);
                        }
                    }
                }
                return collection;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public static System.Data.DataSet Get_PdfBuyerlink_AddressId(string ADDRESSID, string ADDRTYPE)
        {
            try
            {
                return SmPdfBuyerLink.Select_SM_PDF_BUYER_LINK_AddressId(ADDRESSID, ADDRTYPE);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public static System.Data.DataSet Get_PdfBuyerLink(string ADDRESSID)
        {
            MetroLesMonitor.Dal.SmPdfBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfBuyerLink();
                return dbo.GetPdfBuyerLink_By_AddressID(ADDRESSID);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public static SmPdfBuyerLinkCollection Select_PdfBuyerLink_AddressId(int ADDRESSID)
        {
            MetroLesMonitor.Dal.SmPdfBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfBuyerLink();
                System.Data.DataSet ds = Get_PdfBuyerLink(ADDRESSID.ToString());
                SmPdfBuyerLinkCollection collection = new SmPdfBuyerLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmPdfBuyerLink obj = new SmPdfBuyerLink();
                        obj.Fill(ds.Tables[0].Rows[i]);
                        if ((obj != null))
                        {
                            collection.Add(obj);
                        }
                    }
                }
                return collection;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if ((dbo != null))
                {
                    dbo.Dispose();
                }
            }
        }

        public static SmPdfBuyerLinkCollection Get_PDFBuyerLink_By_Mapid(string MAPID)
        {
            MetroLesMonitor.Dal.SmPdfBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfBuyerLink();
                System.Data.DataSet ds = dbo.GetPDFBuyerLinks_By_MAPID(MAPID);
                SmPdfBuyerLinkCollection collection = new SmPdfBuyerLinkCollection();
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    for (int i = 0; (i < ds.Tables[0].Rows.Count); i = (i + 1))
                    {
                        SmPdfBuyerLink obj = new SmPdfBuyerLink();
                        obj.Fill(ds.Tables[0].Rows[i]);
                        if ((obj != null))
                        {
                            collection.Add(obj);
                        }
                    }
                }
                return collection;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if ((dbo != null))
                {
                    dbo.Dispose();
                }
            }
        }

        //ADDED BY KALPITA ON 22/02/2018
        public static System.Data.DataSet SM_PDF_BUYER_LINK_MapCode()
        {
            MetroLesMonitor.Dal.SmPdfBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfBuyerLink();
                System.Data.DataSet ds = dbo.SM_PDF_BUYER_LINK_MapCode();
                return ds;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if ((dbo != null))
                {
                    dbo.Dispose();
                }
            }
        }

        public static SmPdfBuyerLink Load_new(int MAP_ID, MetroLesMonitor.Dal.DataAccess _dataAccess)
        {
            MetroLesMonitor.Dal.SmPdfBuyerLink dbo = null;
            try
            {
                dbo = new MetroLesMonitor.Dal.SmPdfBuyerLink(_dataAccess);
                System.Data.DataSet ds = dbo.SM_PDF_BUYER_LINK_Select_One(MAP_ID);
                SmPdfBuyerLink obj = null;
                if (GlobalTools.IsSafeDataSet(ds))
                {
                    if ((ds.Tables[0].Rows.Count > 0))
                    {
                        obj = new SmPdfBuyerLink();
                        obj.Fill(ds.Tables[0].Rows[0]);
                    }
                }
                return obj;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
             
            }
        }
    }
}
