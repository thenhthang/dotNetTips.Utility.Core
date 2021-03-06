﻿// ***********************************************************************
// Assembly         : dotNetTips.Utility.Standard
// Author           : David McCarter
// Created          : 10-09-2020
//
// Last Modified By : David McCarter
// Last Modified On : 10-21-2020
// ***********************************************************************
// <copyright file="Mailer.cs" company="David McCarter - dotNetTips.com">
//     McCarter Consulting (David McCarter)
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using dotNetTips.Utility.Standard.Common;
using dotNetTips.Utility.Standard.Extensions;
using dotNetTips.Utility.Standard.OOP;

namespace dotNetTips.Utility.Standard.Net
{
    /// <summary>
    /// Class Mailer. Implements the <see cref="System.IDisposable" />.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    [Information(nameof(Mailer), "David McCarter", "10/09/2020", "10/15/2020", UnitTestCoverage = 0, Status = Status.Available)]
    public class Mailer : IDisposable
    {
        /// <summary>
        /// The disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// The mail server.
        /// </summary>
        private SmtpClient _mailServer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mailer"/> class.
        /// </summary>
        /// <param name="host">SMTP host address (required).</param>
        public Mailer(string host)
        {
            this._mailServer = new SmtpClient(host);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mailer"/> class.
        /// </summary>
        /// <param name="host">SMTP host address (required).</param>
        /// <param name="port">SMTP host port (required).</param>
        public Mailer(string host, int port)
        {
            this._mailServer = new SmtpClient(host, port);
        }

        /// <summary>
        /// Gets or sets the timeout for the SMTP server.
        /// </summary>
        /// <value>The timeout.</value>
        public int Timeout
        {
            get { return this._mailServer.Timeout; }
            set { this._mailServer.Timeout = value.EnsureMinimumValue(100); }
        }

        /// <summary>
        /// Creates list of email addresses.
        /// </summary>
        /// <param name="emailAddressType">Type of the email address.</param>
        /// <param name="emailAddresses">Array of email addresses.</param>
        /// <returns>List&lt;EmailAddress&gt;.</returns>
        public static EmailAddress[] CreateEmailAddressList(EmailAddressType emailAddressType, params string[] emailAddresses)
        {
            Encapsulation.TryValidateParam(emailAddressType, nameof(emailAddressType));
            Encapsulation.TryValidateParam(emailAddresses, nameof(emailAddresses));

            var addresses = new List<EmailAddress>();
            addresses.AddRange(emailAddresses.Select(address => new EmailAddress(address)
            { EmailAddressType = emailAddressType }));

            return addresses.ToArray();
        }

        /// <summary>
        /// Sends email with the users default email application.
        /// </summary>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="message">Email message.</param>
        /// <param name="sendToAddress">Email address to send to.</param>
        /// <exception cref="ArgumentException">sendToAddress.</exception>
        /// <exception cref="System.ArgumentException">sendToAddress.</exception>
        public static void SendMailWithDefaultProgram(string subject, string message, EmailAddress sendToAddress)
        {
            Encapsulation.TryValidateParam(subject, nameof(subject));
            Encapsulation.TryValidateParam(message, nameof(message));
            Encapsulation.TryValidateParam<ArgumentInvalidException>(sendToAddress.IsAddressValid() == false);

            using (var mailProcess = new Process())
            {
                var processInfo = new ProcessStartInfo()
                {
                    FileName = string.Format(CultureInfo.InvariantCulture, "mailto:{0}?subject={1}&body={2}", sendToAddress.Address, subject, message),
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Normal,
                };

                mailProcess.StartInfo = processInfo;
                Process.Start(processInfo);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Sends mail.
        /// </summary>
        /// <param name="message">Message object.</param>
        public void SendMail(MailMessage message)
        {
            this.SendMailMessage(message, null);
        }

        /// <summary>
        /// Sends mail.
        /// </summary>
        /// <param name="fromAddress">From email address.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="message">Message.</param>
        /// <param name="bodyHtml">Sets message is HTML.</param>
        /// <param name="sendAddresses">Send email addresses.</param>
        public void SendMail(EmailAddress fromAddress, string subject, string message, bool bodyHtml, EmailAddress[] sendAddresses)
        {
            Encapsulation.TryValidateParam<ArgumentNullException>(fromAddress.IsNotNull(), nameof(fromAddress));
            Encapsulation.TryValidateParam(subject, nameof(subject));
            Encapsulation.TryValidateParam(message, nameof(message));
            Encapsulation.TryValidateParam(sendAddresses, nameof(sendAddresses));

            // 'Set default types, just in case.
            fromAddress.EmailAddressType = EmailAddressType.SendFrom;

            using (var tempMessage = CreateMailMessage(fromAddress, subject, message, bodyHtml, sendAddresses))
            {
                this.SendMailMessage(tempMessage, null);
            }
        }

        /// <summary>
        /// Sends mail.
        /// </summary>
        /// <param name="fromAddress">From email address.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="message">Message.</param>
        /// <param name="bodyHtml">Sets message is HTML.</param>
        /// <param name="sendToAddress">Send to email address.</param>
        public void SendMail(EmailAddress fromAddress, string subject, string message, bool bodyHtml, EmailAddress sendToAddress)
        {
            Encapsulation.TryValidateParam<ArgumentNullException>(fromAddress.IsNotNull(), nameof(fromAddress));
            Encapsulation.TryValidateParam(subject, nameof(subject));
            Encapsulation.TryValidateParam(message, nameof(message));
            Encapsulation.TryValidateParam<ArgumentNullException>(sendToAddress.IsNotNull(), nameof(sendToAddress));

            // 'Set default types, just in case.
            sendToAddress.EmailAddressType = EmailAddressType.SendTo;
            fromAddress.EmailAddressType = EmailAddressType.SendFrom;

            this.SendMail(fromAddress, subject, message, bodyHtml, sendToAddress);
        }

        /// <summary>
        /// Sends mail async.
        /// </summary>
        /// <param name="message">Message object.</param>
        public async Task SendMailAsync(MailMessage message)
        {
            Encapsulation.TryValidateParam<ArgumentNullException>(message.IsNotNull(), nameof(message));

            await this._mailServer.SendMailAsync(message).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends mail async.
        /// </summary>
        /// <param name="fromAddress">From email address.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="message">Message.</param>
        /// <param name="bodyHtml">Sets message is HTML.</param>
        /// <param name="sendAddresses">Send to email addresses.</param>
        public async Task SendMailAsync(EmailAddress fromAddress, string subject, string message, bool bodyHtml, EmailAddress[] sendAddresses)
        {
            Encapsulation.TryValidateParam<ArgumentNullException>(fromAddress.IsNotNull(), nameof(fromAddress));
            Encapsulation.TryValidateParam(subject, nameof(subject));
            Encapsulation.TryValidateParam(message, nameof(message));
            Encapsulation.TryValidateParam(sendAddresses, nameof(sendAddresses));

            // 'Set default types, just in case.
            fromAddress.EmailAddressType = EmailAddressType.SendFrom;

            using (var tempMessage = CreateMailMessage(fromAddress, subject, message, bodyHtml, sendAddresses))
            {
                await this._mailServer.SendMailAsync(tempMessage).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Sends mail async.
        /// </summary>
        /// <param name="fromAddress">From email address.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="message">Message.</param>
        /// <param name="bodyHtml">Sets message is HTML.</param>
        /// <param name="userToken">User token for async.</param>
        /// <param name="sendToAddress">Send to email address.</param>
        public void SendMailAsync(EmailAddress fromAddress, string subject, string message, bool bodyHtml, object userToken, EmailAddress sendToAddress)
        {
            Encapsulation.TryValidateParam<ArgumentNullException>(fromAddress.IsNotNull(), nameof(fromAddress));
            Encapsulation.TryValidateParam(subject, nameof(subject));
            Encapsulation.TryValidateParam(message, nameof(message));
            Encapsulation.TryValidateParam<ArgumentNullException>(sendToAddress.IsNotNull(), nameof(sendToAddress));

            // 'Set default types, just in case.
            sendToAddress.EmailAddressType = EmailAddressType.SendTo;

            this.SendMailAsync(fromAddress, subject, message, bodyHtml, userToken, sendToAddress);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged
        /// resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed == false)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    this.DisposeFields();
                }
            }

            this._disposed = true;
        }

        /// <summary>
        /// Creates the mail message.
        /// </summary>
        /// <param name="fromAddress">From address.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="message">The message.</param>
        /// <param name="bodyHtml">if set to <c>true</c> [body HTML].</param>
        /// <param name="sendAddresses">The send addresses.</param>
        /// <returns>MailMessage.</returns>
        private static MailMessage CreateMailMessage(EmailAddress fromAddress, string subject, string message, bool bodyHtml, EmailAddress[] sendAddresses)
        {
            var mailMessage = new MailMessage();

            foreach (var tempSendTo in sendAddresses.Where(p => p.IsAddressValid()).ToArray())
            {
                switch (tempSendTo.EmailAddressType)
                {
                    case EmailAddressType.SendBcc:
                        {
                            mailMessage.Bcc.Add(new MailAddress(tempSendTo.Address, tempSendTo.Name));
                            break;
                        }

                    case EmailAddressType.SendCC:
                        {
                            mailMessage.CC.Add(new MailAddress(tempSendTo.Address, tempSendTo.Name));
                            break;
                        }

                    default:
                        {
                            mailMessage.To.Add(new MailAddress(tempSendTo.Address, tempSendTo.Name));
                            break;
                        }
                }
            }

            mailMessage.From = new MailAddress(fromAddress.Address, fromAddress.Name);
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = bodyHtml;

            return mailMessage;
        }

        /// <summary>
        /// Sends the mail message.
        /// </summary>
        /// <param name="tempMessage">The temporary message.</param>
        /// <param name="userToken">The user token.</param>
        private void SendMailMessage(MailMessage tempMessage, object userToken)
        {
            if (userToken.IsNotNull())
            {
                this._mailServer.SendAsync(tempMessage, userToken);
                return;
            }

            this._mailServer.Send(tempMessage);
        }
    }
}
