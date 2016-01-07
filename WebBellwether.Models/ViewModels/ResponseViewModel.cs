namespace WebBellwether.Models.ViewModels
{
    public class ResponseViewModel<TModel>
    {
        public TModel Data { get; set; }
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
    }
}
