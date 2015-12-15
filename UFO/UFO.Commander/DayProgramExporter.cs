

using IronPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.Commander {

    public interface IDayProgramExporter {
        string exportDayProgram(DateTime date, string outFile);
        string exportHtmlSource(DateTime date);
    }

    public class DayProgramHtmlExporter : IDayProgramExporter {

        private IPerformanceService pS = SharedServices.Instance.PerformanceService;
        private IVenueService vS = SharedServices.Instance.VenueService;
        private IArtistService aS = SharedServices.Instance.ArtistService;
        private ICategoryService catS = SharedServices.Instance.CategoryService;
        private ICountryService couS = SharedServices.Instance.CountryService;
        private DateTime date;

        public string exportDayProgram(DateTime date, string outName) {
            this.date = date;
            string fileName = System.IO.Path.GetTempPath() + outName;
            string rootedName = "";
            using (StreamWriter w = new StreamWriter(fileName)) {
                w.Write(GenerateHeader());
                w.Write(GenerateBody(date));
                w.Flush();
                rootedName = MediaManager.Instance.RootHtml(fileName);
            }
            File.Delete(fileName);
            return rootedName;
        }

        public string exportHtmlSource(DateTime date) {
            String exportHtml = GenerateHeader();
            exportHtml += GenerateBody(date);
            return exportHtml;
        }

        public string exportDayProgram(DateTime date) {
            return exportDayProgram(date, String.Format("DayProgram_{0}_{1}_{2}.html", date.Year, date.Month, date.Day));
        }

        private String GenerateHeader() {
            String header = "<!DOCTYPE html><html lang='en'>";
            header += "<head><meta charset = 'UTF - 8' >\n<link rel = 'stylesheet' type = 'text/css' href = 'http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css'>";
            header += "<style>.tableCellCenter th,td{ text-align: center;}.table > tbody > tr > td {vertical-align: middle;}</style>";
       
           // header += "<link rel = 'stylesheet' type = 'text / css' href = 'css/costum.css'>";
            header += "<title>UFO-Commander Program</title>";
            header += "</head>";
            return header;
        }

        private String GenerateBody(DateTime date) {
            String body = "\n<body><div class='container'>";

            body += "\n<h3>Program for " + date.Day + "." + date.Month + "." + date.Year + ":</h3>";
            body += GenerateTable(date);
            body += GenerateLegend();
            body += "\n</div><body>";
            return body;
        }

        private String GenerateTable(DateTime date) {
            String table = "";
            var performances = pS.GetPerformancesForDay(date);
            if (performances.Count != 0) {
                var beginHour = performances[0].Date.Hour;
                var endHour = performances[performances.Count - 1].Date.Hour;
                table += "<table class='table table-bordered '>";
                table += GenerateTableHeader(date, performances, beginHour, endHour);
                table += GenerateTableBody(performances, beginHour, endHour);
                table += "</table>";
            }
            return table;
        }

        private String GenerateTableHeader(DateTime date, List<Performance> performances, int beginHour, int endHour) {
            String tableHeader = "<thead><tr class='info h4 tableCellCenter small'>";

            if (performances.Count != 0) {
                tableHeader += "<th class=''>Location | Time</th>";
                for (int i = beginHour; i <= endHour; i++) {
                    tableHeader += "<th>" + i + "-" + (i + 1) + " Uhr</th>";
                }
            }
            tableHeader += "</tr></thead>";
            return tableHeader;
        }

        private String GenerateTableBody(List<Performance> performances, int beginHour, int endHour) {

            String tableContent = "<tbody>";
            performances.Sort((perf1, perf2) => {
                Venue v1 = vS.GetVenueById(perf1.VenueId);
                Venue v2 = vS.GetVenueById(perf2.VenueId);
                int result = v1.Shortcut.CompareTo(v2.Shortcut);
                if (result != 0) {
                    return result;
                }
                return perf1.Date.CompareTo(perf2.Date);
            });
            uint curVId = performances[0].VenueId;

            Performance[] curVenuePerf = new Performance[endHour - beginHour + 1];
            var last = performances[performances.Count - 1];
            foreach (var p in performances) {
                
                if (p.VenueId == curVId) {
                    curVenuePerf[p.Date.Hour - beginHour] = p;
                } else {
                    tableContent += GenerateTableRow(vS.GetVenueById(curVId), curVenuePerf);
                    for (int i = 0; i < curVenuePerf.Length; i++) {
                        curVenuePerf[i] = null;
                    }
                    curVId = p.VenueId;
                    curVenuePerf[p.Date.Hour - beginHour] = p;
                }
                if(last == p) {
                    tableContent += GenerateTableRow(vS.GetVenueById(curVId), curVenuePerf);
                }
            }
            tableContent += "</tbody>";
            return tableContent;
        }

        private String GenerateTableRow(Venue venue, Performance[] performances) {
            String tableRow = "<tr class='h4 tableCellCenter small'>";
            tableRow += "<td class='warning '><strong>" + HttpUtility.HtmlEncode(venue.Shortcut) + " <br>" + HttpUtility.HtmlEncode(venue.Name) + "</strong></td>";
            for (var i = 0; i < performances.Length; i++) {
                if (performances[i] == null) {
                    tableRow += "<td></td>";
                } else {
                    Artist a = aS.GetArtistById(performances[i].ArtistId);
                    tableRow += "<td><strong>" + HttpUtility.HtmlEncode(a.Name) + " / "+ HttpUtility.HtmlEncode(catS.GetCategoryById(a.CategoryId).Shortcut)+ "</strong></br>(" + HttpUtility.HtmlEncode(couS.GetCountryById(a.CountryId).Name)+")</td>";
                }

            }
            tableRow += "</tr>";
            return tableRow;
        }

        private String GenerateLegend() {
            String legend = "<h4>Category-Legend:</h4>";
            legend += "<ul class='list-group '>";
            foreach (var category in catS.GetAllCategories()) {
                legend += "<li class='list-group-item'>" + category.Shortcut + ": " + category.Name + "</li>";
            }
            legend += "</ul>";
            return legend;
        }
    }

    public class DayProgramPdfExporter : IDayProgramExporter {
        private DayProgramHtmlExporter htmlExporter = new DayProgramHtmlExporter();

        public string exportHtmlSource(DateTime date) {
            return htmlExporter.exportHtmlSource(date);
        }

        public string exportDayProgram(DateTime date, string outName) {
            string htmlSource = exportHtmlSource(date);

            string fileName = System.IO.Path.GetTempPath() + outName;
            HtmlToPdf HtmlToPdf = new IronPdf.HtmlToPdf();

            PdfResource pdf = HtmlToPdf.RenderHtmlAsPdf(htmlSource);
            pdf.SaveAs(fileName);
            
            return MediaManager.Instance.RootPdf(fileName);
        }

        public string exportDayProgram(DateTime date) {
            return exportDayProgram(date, String.Format("DayProgram_{0}_{1}_{2}.pdf", date.Year, date.Month, date.Day));
        }
    };

}
