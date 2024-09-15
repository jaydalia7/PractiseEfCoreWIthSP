namespace PractiseEfCoreWIthSP.Models.ViewModels
{
    public class WebApiReponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public object Data { get; set; }
    }
}
