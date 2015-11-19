using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBellwether.API.Results
{
    public class ResultStateContainer
    {
        public ResultStateContainer()
        {

        }
        public ResultStateContainer(ResultState resultState,ResultMessage resultMessage,object resultValue)
        {
            ResultState = resultState;
            ResultMessage = resultMessage;
            ResultValue = resultValue;
        }
        public ResultState ResultState { get; set; }
        public ResultMessage ResultMessage { get; set; }
        public object ResultValue { get; set; }
    }
}