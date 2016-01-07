using WebBellwether.Models.ViewModels;

namespace WebBellwether.API.ExtensionMethods
{
    public static class ResponseExtensionMethods
    {
        public static ResponseViewModel<TModel> Valid<TModel>(this ResponseViewModel<TModel> response, TModel data)
        {
            return new ResponseViewModel<TModel>()
            {
                Data = data,
                IsValid = true
            };
        }

        public static ResponseViewModel<TModel> Invalid<TModel>(this ResponseViewModel<TModel> response, string errorMessage)
        {
            return new ResponseViewModel<TModel>()
            {
                IsValid = false,
                ErrorMessage = errorMessage
            };
        }
    }
}