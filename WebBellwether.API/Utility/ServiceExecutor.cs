using System;
using System.Threading.Tasks;
using WebBellwether.API.ExtensionMethods;
using WebBellwether.Models.ViewModels;

namespace WebBellwether.API.Utility
{
    public static class ServiceExecutor
    {
        public static ResponseViewModel<TResponse> Execute<TResponse>(Func<TResponse> serviceExecution)
        {
            try
            {
                var response = serviceExecution();

                return new ResponseViewModel<TResponse>().Valid(response);
            }
            catch (Exception e)
            {
                return new ResponseViewModel<TResponse>().Invalid(e.Message);
            }
        }
        public static async Task<ResponseViewModel<TResponse>> ExecuteAsync<TResponse>(Func<TResponse> serviceExecution)
        {
            try
            {
                var response = await Task.Factory.StartNew(serviceExecution).ConfigureAwait(true);
           
                    //var response = serviceExecution();
                    return new ResponseViewModel<TResponse>().Valid(response);
          
            }
            catch (Exception e)
            {
                return new ResponseViewModel<TResponse>().Invalid(e.Message);
            }
        }
    }
}
