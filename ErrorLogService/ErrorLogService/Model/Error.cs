using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
namespace LogErrorsService.Model
{
    public class Error
    {
        public int ApplicationId { get; set; }
        public string ModuleName { get; set; }
        public string FileName { get; set; }
        public string MethodName { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public string Url { get; set; }
        public string ErrorEmail { get; set; }

        public int ErrorLogId { get; set; }
        public string ApplicationName { get; set; }

        DataAccessLayer.DataAccessLayer.SqlDataAccess sqlDataAccess = new DataAccessLayer.DataAccessLayer.SqlDataAccess();
        public bool Log()
        {

            try
            {
                bool RetVal = false;

                string ProcName = "usp_LogError";

                DataAccessLayerParameterList param = new DataAccessLayerParameterList()
                {
                    new DataAccessLayerParameter()
                    {
                        ParameterName ="@ApplicationId",
                        ParameterDirection= ParameterDirection.Input,
                        ParameterType = SqlDbType.Int,
                        ParameterValue = this.ApplicationId
                    },
                    new DataAccessLayerParameter()
                    {
                        ParameterName ="@Url",
                        ParameterDirection= ParameterDirection.Input,
                        ParameterType = SqlDbType.NVarChar,
                        ParameterValue = this.Url
                    },
                    new DataAccessLayerParameter()
                    {
                        ParameterName ="@ModuleName",
                        ParameterDirection= ParameterDirection.Input,
                        ParameterType = SqlDbType.NVarChar,
                        ParameterValue = this.ModuleName
                    },
                    new DataAccessLayerParameter()
                    {
                        ParameterName ="@FileName",
                        ParameterDirection= ParameterDirection.Input,
                        ParameterType = SqlDbType.NVarChar,
                        ParameterValue = this.FileName
                    },
                    new DataAccessLayerParameter()
                    {
                        ParameterName ="@MethodName",
                        ParameterDirection= ParameterDirection.Input,
                        ParameterType = SqlDbType.NVarChar,
                        ParameterValue = this.MethodName
                    },
                    new DataAccessLayerParameter()
                    {
                        ParameterName ="@ErrorMessage",
                        ParameterDirection= ParameterDirection.Input,
                        ParameterType = SqlDbType.NVarChar,
                        ParameterValue = this.ErrorMessage
                    },
                    new DataAccessLayerParameter()
                    {
                        ParameterName ="@StackTrace",
                        ParameterDirection= ParameterDirection.Input,
                        ParameterType = SqlDbType.NVarChar,
                        ParameterValue = this.StackTrace
                    },
                    new DataAccessLayerParameter()
                    {
                        ParameterName = "@ProcStatus",
                        ParameterDirection = ParameterDirection.Output,
                        ParameterType = SqlDbType.TinyInt
                    },
                    new DataAccessLayerParameter()
                    {
                        ParameterName = "@IsAlertsRequired",
                        ParameterDirection = ParameterDirection.Output,
                        ParameterType = SqlDbType.Bit
                    },
                    new DataAccessLayerParameter()
                    {
                        ParameterName = "@ErrorEmail",
                        ParameterDirection = ParameterDirection.Output,
                        ParameterType = SqlDbType.NVarChar,
                        ParameterSize = 300

                    },
                    new DataAccessLayerParameter()
                    {
                        ParameterName = "@ApplicationName",
                        ParameterDirection = ParameterDirection.Output,
                        ParameterType = SqlDbType.NVarChar,
                        ParameterSize = 500
                    },
                    new DataAccessLayerParameter()
                    {
                        ParameterName = "@ErrorLogId",
                        ParameterDirection = ParameterDirection.Output,
                        ParameterType = SqlDbType.BigInt
                    }
                };

                this.sqlDataAccess.SaveDataToDatabase(ProcName, ref param);

                DataAccessLayerParameter procStatusParam = param.First(x => x.ParameterName == "@ProcStatus");

                RetVal = (Convert.ToInt16(procStatusParam.ParameterValue) == 1);

                DataAccessLayerParameter isAlertsRequiredParam = param.First(x => x.ParameterName == "@IsAlertsRequired");

                DataAccessLayerParameter errorEmailParam = param.First(x => x.ParameterName == "@ErrorEmail");

                DataAccessLayerParameter applicationNameParam = param.First(x => x.ParameterName == "@ApplicationName");

                DataAccessLayerParameter errorLogIdParam = param.First(x => x.ParameterName == "@ErrorLogId");

                this.ErrorEmail = errorEmailParam.ParameterValue.ToString();
                this.ApplicationName = applicationNameParam.ParameterValue.ToString();
                this.ErrorLogId = Convert.ToInt32(errorLogIdParam.ParameterValue);
                bool isAlertsRequired = Convert.ToBoolean(isAlertsRequiredParam.ParameterValue);

                if (RetVal && isAlertsRequired)
                {
                    SendErrorEmail(this.ErrorEmail);
                }

                return RetVal;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool SendErrorEmail(string ToEmail)
        {
            bool RetVal = false;

            string subject = "Error - " + this.ApplicationName;
            string bodyText = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["ErrorEmailFormat"]);

            bodyText = bodyText
                        .Replace("{{ModuleName}}", this.ModuleName)
                        .Replace("{{FileName}}", this.FileName)
                        .Replace("{{MethodName}}", this.MethodName)
                        .Replace("{{ErrorMessage}}", this.ErrorMessage)
                        .Replace("{{StackTrace}}", this.StackTrace)
                        .Replace("{{Url}}", this.Url);



            List<string> recipients = new List<string>() { ToEmail.Trim() };
            try
            {
                if (SendEmail(
                    subject,
                    bodyText,
                    recipients,
                    null,
                    null,
                    true
                    ))
                {

                    string ProcName = "usp_ErrorMailSent";

                    DataAccessLayerParameterList param = new DataAccessLayerParameterList()
                    {

                        new DataAccessLayerParameter()
                        {
                            ParameterName ="@ErrorLogId",
                            ParameterDirection= ParameterDirection.Input,
                            ParameterType = SqlDbType.Int,
                            ParameterValue = this.ErrorLogId
                        },
                        new DataAccessLayerParameter()
                        {
                            ParameterName = "@ProcStatus",
                            ParameterDirection = ParameterDirection.Output,
                            ParameterType = SqlDbType.TinyInt
                        }

                    };

                    this.sqlDataAccess.SaveDataToDatabase(ProcName, ref param);

                    DataAccessLayerParameter procStatusParam = param.First(x => x.ParameterName == "@ProcStatus");

                    RetVal = (Convert.ToInt16(procStatusParam.ParameterValue) == 1);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while send confirmation email : " + ex.Message);
            }

            return RetVal;
        }

        public static bool SendEmail(
            string subject,
            string bodyText,
            List<string> recipients,
            List<string> ccRecipients = null,
            List<string> bccRecipients = null,
            bool IsBodyHTML = false
            )
        {
            bool RetVal = false;

            try
            {
                string FromMail = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["FromEmail"]);

                MailMessage mail = new MailMessage();

                mail.From = new MailAddress(FromMail);
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Subject = subject;
                mail.Body = bodyText;
                mail.IsBodyHtml = IsBodyHTML;

                foreach (string emailAddress in recipients)
                    mail.To.Add(new MailAddress(emailAddress));

                SmtpClient SmtpServer = new SmtpClient(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SmtpClient"]));

                SmtpServer.Port = Convert.ToInt32(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Port"]));
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(
                    Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SmtpUserName"]),
                    Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SmtpPassword"]));

                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                RetVal = true;

            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine("SmtpException : " + smtpEx.Message + " " + smtpEx.StackTrace);
                //
            }
            catch (Exception ex)
            {
                Console.WriteLine("SmtpException : " + ex.Message + " " + ex.StackTrace);
                //
            }
            return RetVal;
        }
    }
}