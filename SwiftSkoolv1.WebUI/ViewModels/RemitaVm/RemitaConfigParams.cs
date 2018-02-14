namespace SwiftSkoolv1.WebUI.ViewModels.RemitaVm
{
    public static class RemitaConfigParams
    {

        //public const string SCHOOLFEESERVICETYPE = "4430731";
        //public const string ACCEPTANCESERVICETYPE = "4430731";
        //public const string SUPPLEMENTARYSERVICETYPE = "4430731";
        //public const string HOSTELAPPLICATIONSERVICETYPE = "4430731";
        //public const string ACCOMODATIONSERVICETYPE = "4430731";

        //public const string MERCHANTID = "2547916";
        //public const string APIKEY = "1946";
        public const string CHECKSTATUSURL = "http://www.remitademo.net/remita/ecomm";
    }

    public class RemitaRePostVm
    {
        public string merchantId { get; set; }
        public string hash { get; set; }
        public string rrr { get; set; }
        public string responseurl { get; set; }
    }
}