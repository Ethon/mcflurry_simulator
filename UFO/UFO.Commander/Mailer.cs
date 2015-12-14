using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using UFO.Server.Data;

namespace UFO.Commander {
    public interface IMailer {
        void OnDayProgramChanged(DateTime day);
    }

    public abstract class AbstractMailer : IMailer {

        private const string TMPL_BODY = "Templates\\OnProgramChangedMail_en.txt";
        private const string TMPL_ITEM = "Templates\\OnProgramChangedMailItem_en.txt";

        private const string TIME_KEY = "%TIME%";
        private const string VENUE_KEY = "%VENUE%";
        private const string ARTIST_KEY = "%ARTIST%";
        private const string DATE_KEY = "%DATE%";
        private const string ITEMS_KEY = "%PERFORMANCELIST%";

        private string BuildMailBodyItems(string template, List<Performance> performances) {
            List<string> items = new List<string>(performances.Count);
            foreach (var p in performances) {
                Venue venue = SharedServices.Instance.VenueService.GetVenueById(p.VenueId);
                items.Add(template.Replace(TIME_KEY, p.Date.ToLongTimeString())
                                    .Replace(VENUE_KEY, venue != null ? venue.Name : "")
                         );
            }

            return string.Join("\n", items);
        }

        private string BuildMailBody(DateTime day, Artist artist, List<Performance> performances) {
            if (performances.Count == 0) {
                return "";
            }
            string bodyTemplate = File.ReadAllText(Config.Get().MailBodyTemplatePath);
            string itemTemplate = File.ReadAllText(Config.Get().MailBodyItemTemplatePath);
            return bodyTemplate.Replace(ARTIST_KEY, artist.Name)
                                .Replace(DATE_KEY, day.ToLongDateString())
                                .Replace(ITEMS_KEY, BuildMailBodyItems(itemTemplate, performances));
        }

        private string BuildMailSubject(DateTime day) {
            string subjectTemplate = File.ReadAllText(Config.Get().MailSubjectTemplatePath);
            return subjectTemplate.Replace(DATE_KEY, day.ToLongDateString());
        }

        protected abstract void DoSendMail(String recipient, String subject, String body, String attachment);

        public void OnDayProgramChanged(DateTime day) {
            List<Performance> performances = SharedServices.Instance.PerformanceService.GetPerformancesForDay(day);

            Dictionary<uint, List<Performance>> performancesByArtist = new Dictionary<uint, List<Performance>>();
            foreach (var p in performances) {
                List<Performance> list;
                if(!performancesByArtist.TryGetValue(p.ArtistId, out list)) {
                    list = new List<Performance>();
                    performancesByArtist.Add(p.ArtistId, list);
                }
                list.Add(p);
            }

            string dayProgramPdf = new DayProgramPdfExporter().exportDayProgram(day);
            string dayProgramPdfPath = MediaManager.Instance.GetFullPath(dayProgramPdf, MediaType.Pdf);
            foreach (var artistId in performancesByArtist.Keys) {
                Artist artist = SharedServices.Instance.ArtistService.GetArtistById(artistId);
                if(artist == null) {
                    continue;
                }

                DoSendMail( artist.Email,
                            BuildMailSubject(day),
                            BuildMailBody(day, artist, performancesByArtist[artistId]),
                            dayProgramPdfPath);
            }

        }
    }

    public class SmtpMailer : AbstractMailer {

        private string sender;
        private SmtpClient client;

        public SmtpMailer(string sender, string server, int port, string user, string password) {
           this.client = new SmtpClient(server, port) {
                Credentials = new NetworkCredential(user, password),
                EnableSsl = true
            };
            this.sender = sender;
        }

        protected override void DoSendMail(string recipient, string subject, string body, string attachment) {
            MailMessage message = new MailMessage(sender, recipient, subject, body);
            message.IsBodyHtml = body.Contains("<html>");
            if(attachment != null) { 
                message.Attachments.Add(new Attachment(attachment, "application/pdf"));
            }
            try { 
                client.Send(message);
                PlatformService.Instance.ShowInformationMessage("Sent update to " + message.To, "Mail was sent");
            } catch(Exception e) {
                PlatformService.Instance.ShowErrorMessage("Failed to send mail to " + message.To + " failed: " + e.Message,
                    "Sending email failed");
            }
        }
    }
}
