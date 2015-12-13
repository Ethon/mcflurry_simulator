﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UFO.Server;
using UFO.Server.Data;

namespace UFO.Commander {

    

    public interface IDayProgramExporter {
        string exportDayProgram(DateTime date, string outFile);
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

        public string exportDayProgram(DateTime date) {
            return exportDayProgram(date, String.Format("DayProgram_{0}_{1}_{2}.html", date.Year, date.Month, date.Day));
        }
        private String GenerateHeader() {
            String header = "<!DOCTYPE html><html lang='en'>";
            header += "\n<head><meta charset = 'UTF - 8' >\n<link rel = 'stylesheet' type = 'text/css' href = 'css/bootstrap.min.css'>";
            header += "\n<link rel = 'stylesheet' type = 'text / css' href = 'css/costum.css'>";
            header += "\n<title>UFO-Commander Program</title>";
            header += "\n</head>";
            return header;
        }
        private String GenerateBody(DateTime date) {
            String body = "\n<body><div class='container'>";
            
            body += "\n<h2>Program for "+date.Day+"."+date.Month+"."+date.Year+"</h2>";
            body += GenerateTable(date);
            body += "\n</div><body>";
            return body;
        }
        private String GenerateTable(DateTime date) {
            String table = "";
            var performances = pS.GetPerformancesForDay(date);
            if (performances.Count != 0) {
                var beginHour = performances[0].Date.Hour;
                var endHour = performances[performances.Count - 1].Date.Hour;
                table += "<table class='table table-bordered'>";
                table += GenerateTableHeader(date, performances,beginHour,endHour);
                table += GenerateTableBody(performances,beginHour,endHour);
                table += "</table>";
            }
            return table;
        }
        private String GenerateTableHeader(DateTime date,List<Performance>performances, int beginHour, int endHour) {
            String tableHeader = "<thead><tr class='info h4 tableCellCenter'>";
            
            if (performances.Count != 0) {


                tableHeader += "<th class=''>Location</th>";
                for (int i = beginHour; i <= endHour; i++) {
                    tableHeader += "<th>"+i+"-"+(i+1)+" Uhr</th>";
                }                
            }
            tableHeader += "</tr></thead>";
            return tableHeader;
        }
        private String GenerateTableBody(List<Performance>performances,int beginHour,int endHour) {

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




            Performance[] curVenuePerf = new Performance[endHour - beginHour +1];
            foreach (var p in performances) {
               if(p.VenueId == curVId) {
                    curVenuePerf[p.Date.Hour - beginHour] = p;
                } else {
                    tableContent += GenerateTableRow(vS.GetVenueById(curVId),curVenuePerf);
                    for (int i = 0; i < curVenuePerf.Length; i++) {
                        curVenuePerf[i] = null;
                    }
                    curVId = p.VenueId;
                    curVenuePerf[p.Date.Hour - beginHour] = p;
                    
                }               
            }
            return tableContent;
        }
        public String GenerateTableRow(Venue venue,Performance[] performances) {
            String tableRow = "<tr class='h4 tableCellCenter'>";
            tableRow += "<td class='warning'>"+venue.Shortcut+" " + HttpUtility.HtmlEncode(venue.Name) + "</td>";
            
            for(var i= 0; i < performances.Length; i++) {
                if (performances[i] == null) {
                    tableRow += "<td></td>";
                } else {
                    tableRow += "<td>" + HttpUtility.HtmlEncode(aS.GetArtistById(performances[i].ArtistId).Name) + "</td>";
                }

            }
            tableRow += "</tr>";
            return tableRow;

        }
    }

}
