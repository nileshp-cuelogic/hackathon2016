using LogErrorsService.Model;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace ErrorLogService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IErrorLogService" in both code and config file together.
    [ServiceContract]
    public interface IErrorLogService
    {
        [OperationContract]
        [WebInvoke(Method = "OPTIONS", UriTemplate = "*")]
        void GetOptions();
        [OperationContract]
        [WebInvoke(Method = "*", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        bool LogError(int ApplicationId, string ModuleName, string FileName, string MethodName, string ErrorMessage, string StackTrace = "", string Url = "");
        [OperationContract(Name = "LogErrorUsingObject")]
        [WebInvoke(Method = "*", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        bool LogError(LogError objLogError);
    }
}
