using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using System.IO;
using MimeKit.Utils;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace SurveyAppCore2.Models
{
    public class EmailSender
    {
     
        public EmailSender()
        {
       
        }

        private void ParseImage(string html)
        {
       
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//img"))
            {
                var image_links = link.GetAttributeValue("src","");
            }
        }

        private void ParseImageByte()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            var builder = new BodyBuilder();
            var pathImage = Path.Combine(projectPath, "Image.png");
            var image = builder.LinkedResources.Add(projectPath);

            image.ContentId = MimeUtils.GenerateMessageId();

            builder.HtmlBody = string.Format(@"<p>Hey!</p><img src=""cid:{0}"">", image.ContentId);

            //message.Body = builder.ToMessageBody();
        }

        public async Task SendEmailAsync(
            SmtpOptions smtpOptions,
            string to,
            string from,
            string subject,
            string plainTextMessage,
            string htmlMessage,
            string imagePath,
            string replyTo = null)
        {
            if (string.IsNullOrWhiteSpace(to))
            {
                throw new ArgumentException("no to address provided");
            }

            if (string.IsNullOrWhiteSpace(from))
            {
                throw new ArgumentException("no from address provided");
            }

            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentException("no subject provided");
            }

            var hasPlainText = !string.IsNullOrWhiteSpace(plainTextMessage);
            var hasHtml = !string.IsNullOrWhiteSpace(htmlMessage);
            if (!hasPlainText && !hasHtml)
            {
                throw new ArgumentException("no message provided");
            }

            var m = new MimeMessage();

            m.From.Add(new MailboxAddress("", from));
            if (!string.IsNullOrWhiteSpace(replyTo))
            {
                m.ReplyTo.Add(new MailboxAddress("", replyTo));
            }
            m.To.Add(new MailboxAddress("", to));
            m.Subject = subject;

            //m.Importance = MessageImportance.Normal;
            //Header h = new Header(HeaderId.Precedence, "Bulk");
            //m.Headers.Add()

            BodyBuilder bodyBuilder = new BodyBuilder();
            if (hasPlainText)
            {
                bodyBuilder.TextBody = plainTextMessage;
            }

            if (hasHtml)
            {


                string tmpHTMLBody = htmlMessage;
                // ADDED CODE TO EMBED IMAGE



                //GroupCollection collection = Regex.Match(htmlMessage, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups;
                //foreach (Group item in collection)
                //{
                //    string matchString = item.Value;
                //    await Console.Out.WriteLineAsync(matchString);
                //}

                try
                {
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(htmlMessage);
                    List<HtmlNode> nodes = doc.DocumentNode.SelectNodes("//img").ToList();
                    foreach (HtmlNode link in nodes)
                    {
                        string image_links = link.GetAttributeValue("src", "").ToString();


                        //if (imgTypeList.Contains(image_links))
                        //{


                        // await Console.Out.WriteLineAsync(image_links);



                        string pathImage = (imagePath + image_links.Replace("/", @"\"));

                        //await Console.Out.WriteLineAsync(pathImage);

                        MimeEntity image = bodyBuilder.LinkedResources.Add(pathImage);

                        image.ContentId = MimeUtils.GenerateMessageId();

                        tmpHTMLBody = tmpHTMLBody.Replace(image_links, "cid:" + image.ContentId);

                    }
                }
                catch (Exception e)
                {
                    string error = e.InnerException.Message;
                }


             
                //}


                ////////////////////////////////////////////////////////////////////////////////////////////

                bodyBuilder.HtmlBody = tmpHTMLBody;
            }




            m.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(
                    smtpOptions.Server,
                    smtpOptions.Port,
                    smtpOptions.UseSsl)
                    .ConfigureAwait(false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                if (smtpOptions.RequiresAuthentication)
                {
                    await client.AuthenticateAsync(smtpOptions.User, smtpOptions.Password)
                        .ConfigureAwait(false);
                }

                await client.SendAsync(m).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }

        }

        public async Task SendMultipleEmailAsync(
            SmtpOptions smtpOptions,
            string toCsv,
            string from,
            string subject,
            string plainTextMessage,
            string imagePath,
            string htmlMessage)
        {
            if (string.IsNullOrWhiteSpace(toCsv))
            {
                throw new ArgumentException("no to addresses provided");
            }

            if (string.IsNullOrWhiteSpace(from))
            {
                throw new ArgumentException("no from address provided");
            }

            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentException("no subject provided");
            }

            var hasPlainText = !string.IsNullOrWhiteSpace(plainTextMessage);
            var hasHtml = !string.IsNullOrWhiteSpace(htmlMessage);
            if (!hasPlainText && !hasHtml)
            {
                throw new ArgumentException("no message provided");
            }

            var m = new MimeMessage();
            m.From.Add(new MailboxAddress("", from));
            string[] adrs = toCsv.Split(',');

            foreach (string item in adrs)
            {
                if (!string.IsNullOrEmpty(item)) { m.To.Add(new MailboxAddress("", item)); ; }
            }

            m.Subject = subject;
            m.Importance = MessageImportance.High;

            BodyBuilder bodyBuilder = new BodyBuilder();
            if (hasPlainText)
            {
                bodyBuilder.TextBody = plainTextMessage;
            }

            if (hasHtml)
            {

                string tmpHTMLBody = htmlMessage;
                // ADDED CODE TO EMBED IMAGE

                try
                {
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(htmlMessage);
                    List<HtmlNode> nodes = doc.DocumentNode.SelectNodes("//img").ToList();
                    foreach (HtmlNode link in nodes)
                    {
                        string image_links = link.GetAttributeValue("src", "").ToString();


                        //if (imgTypeList.Contains(image_links))
                        //{


                        // await Console.Out.WriteLineAsync(image_links);



                        string pathImage = (imagePath + image_links.Replace("/", @"\"));

                        //await Console.Out.WriteLineAsync(pathImage);

                        MimeEntity image = bodyBuilder.LinkedResources.Add(pathImage);

                        image.ContentId = MimeUtils.GenerateMessageId();

                        tmpHTMLBody = tmpHTMLBody.Replace(image_links, "cid:" + image.ContentId);

                    }
                    //}
                }
                catch (Exception e)
                {
                    string error = e.InnerException.Message;
                }


                ////////////////////////////////////////////////////////////////////////////////////////////

                bodyBuilder.HtmlBody = tmpHTMLBody;


                //bodyBuilder.HtmlBody = htmlMessage;
            }

            m.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(
                    smtpOptions.Server,
                    smtpOptions.Port,
                    smtpOptions.UseSsl).ConfigureAwait(false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                if (smtpOptions.RequiresAuthentication)
                {
                    await client.AuthenticateAsync(
                        smtpOptions.User,
                        smtpOptions.Password).ConfigureAwait(false);
                }

                await client.SendAsync(m).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }

        }

    }
}
