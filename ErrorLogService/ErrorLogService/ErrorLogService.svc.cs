using LogErrorsService.Model;

namespace ErrorLogService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ErrorLogService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ErrorLogService.svc or ErrorLogService.svc.cs at the Solution Explorer and start debugging.
    public class ErrorLogService : IErrorLogService
    {
        public bool LogError(LogError objLogError)
        {
            Error objError = new Error()
            {
                ApplicationId = objLogError.ApplicationId,
                FileName = objLogError.FileName,
                ModuleName = objLogError.ModuleName,
                MethodName = objLogError.MethodName,
                ErrorMessage = objLogError.ErrorMessage,
                StackTrace = objLogError.StackTrace,
                Url = objLogError.Url
            };
            return objError.Log();
        }

        public bool LogError(int ApplicationId, string ModuleName, string FileName, string MethodName, string ErrorMessage, string StackTrace = "", string Url = "")
        {
            Error objError = new Error() { ApplicationId = ApplicationId, Url = Url, ModuleName = ModuleName, FileName = FileName, MethodName = MethodName, ErrorMessage = ErrorMessage, StackTrace = StackTrace };
            return objError.Log();
        }
    }
}
