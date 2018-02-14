using System.Runtime.Serialization;

namespace SwiftSkoolv1.WebUI.ViewModels.RemitaVm
{

    public class RemitaResponse
    {
        [DataMember(Name = "orderId")]
        public string OrderId { get; set; }

        [DataMember(Name = "RRR")]
        public string Rrr { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
    }

}