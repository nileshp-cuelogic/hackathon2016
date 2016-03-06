using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Activation;
using System.Web;

namespace LogErrorsService.Model
{
    [DataContract]
    [AspNetCompatibilityRequirements(RequirementsMode =
            AspNetCompatibilityRequirementsMode.Allowed)]
    public class LogError
    {
        [DataMember]
        public int ApplicationId { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string ModuleName { get; set; }
        [DataMember]
        public string MethodName { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
        [DataMember]
        public string StackTrace { get; set; }
        [DataMember]
        public string Url { get; set; }
    }
}