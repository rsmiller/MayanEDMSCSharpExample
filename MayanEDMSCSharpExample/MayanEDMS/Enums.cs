
namespace MayanEDMSCSharpExample.MayanEDMS
{
    // ALL THESE ENUMS ARE UNIQUE TO MY INSTALLATION. YOU WILL NEED TO UPDATE OT YOUR OWN OR USE A DIFFERENT METHOD TO POPULATE THE ID
    public static class MayanDocumentTypeId
    {
        public const int Default = 1;
        public const int Sales_Order = 7;
        public const int Purchase_Order = 6;
        public const int AR_Invoice = 2;
        public const int AP_Invoice = 8;
        public const int Certificate_of_Conformance = 5;
        public const int DNC_Cert = 15;
        public const int Declaration_of_Conformity = 14;
        public const int Holloway_BOL = 9;
        public const int MTR = 12;
        public const int Mill_Test_Certificate = 13;
        public const int Our_Packing_List = 9;
        public const int Final_Order_Packet = 16;
        public const int Vendor_Packing_List = 10;
    }

    public static class MayanDocumentMetadataTypeId
    {
        public const int Order_Num = 2;
        public const int PO_Num = 4;
        public const int AP_Num = 11;
        public const int AR_Num = 1;
        public const int Customer_Name = 5;
        public const int Manufacturer_Name = 16;
        public const int Vendor_Name = 10;
        public const int Supplier_Name = 13;
        public const int Product_Num = 6;
    }

    public static class MayanDocumentAction
    {
        public const int Replace = 1;
        public const int Append = 2;
        public const int Keep = 3;
    }

}
