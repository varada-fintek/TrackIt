using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Configuration;
using System.Web.Configuration;

namespace TrackIT.Common
{
    public static class MailHelper
    {

        /// <summary>
        /// Sends an mail message
        /// </summary>
        /// <param name="sFromEmailID">Sender address</param>
        /// <param name="sToEmailID">Recepient address</param>
        /// <param name="sBCCEmailID">Bcc recepient</param>
        /// <param name="sCCEmailID">Cc recepient</param>
        /// <param name="sSubject">Subject of mail message</param>
        /// <param name="sBody">Body of mail message</param>
        public static void SendMailMessage(string sFromEmailID, string sToEmailID, string sBCCEmailID, string sCCEmailID, string sSubject, string sBody)
        {
            // Instantiate a new instance of SmtpClient
            SmtpClient mSmtpClient = new SmtpClient();
            NetworkCredential NetworkCred = new NetworkCredential();
            try
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");

                // Instantiate a new instance of MailMessage
                MailMessage mMailMessage = new MailMessage();

                // Set the sender address of the mail message
                if (string.IsNullOrEmpty(sFromEmailID))
                    mMailMessage.From = new MailAddress(settings.Smtp.From);
                else
                    mMailMessage.From = new MailAddress(sFromEmailID);

                // Set the recepient address of the mail message
                mMailMessage.To.Add(new MailAddress(sToEmailID));

                // Check if the bcc value is null or an empty string
                if (!StringFunctions.IsNullOrEmpty(sBCCEmailID))
                {
                    string[] _sBCCEmailID = sBCCEmailID.Split(';');
                    if (_sBCCEmailID.Length > 0)
                    {
                        foreach (string sEmailIDBCC in _sBCCEmailID)
                        {
                            mMailMessage.Bcc.Add(new MailAddress(sEmailIDBCC));     // Set the Bcc address of the mail message
                        }
                    }
                    else
                        mMailMessage.Bcc.Add(new MailAddress(sBCCEmailID));         // Set the Bcc address of the mail message
                }

                // Check if the cc value is null or an empty value
                if (!StringFunctions.IsNullOrEmpty(sCCEmailID))
                {
                    string[] _sCCEmailID = sCCEmailID.Split(';');
                    if (_sCCEmailID.Length > 0)
                    {
                        foreach (string sEmailIDCC in _sCCEmailID)
                        {
                            mMailMessage.CC.Add(new MailAddress(sEmailIDCC));       // Set the CC address of the mail message
                        }
                    }
                    else
                        mMailMessage.CC.Add(new MailAddress(sCCEmailID));         // Set the CC address of the mail message
                }
                // Set the subject of the mail message
                mMailMessage.Subject = sSubject;

                // Set the body of the mail message
                mMailMessage.Body = sBody;

                // Set the format of the mail message body as HTML
                mMailMessage.IsBodyHtml = true;

                // Set the priority of the mail message to normal
                mMailMessage.Priority = MailPriority.Normal;

                //SMTP Settings
                NetworkCred.UserName = settings.Smtp.Network.UserName;
                NetworkCred.Password = settings.Smtp.Network.Password;
                mSmtpClient.Credentials = NetworkCred;
                mSmtpClient.Host = settings.Smtp.Network.Host;
                mSmtpClient.Port = settings.Smtp.Network.Port;

                // Send the mail message
                mSmtpClient.Send(mMailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}