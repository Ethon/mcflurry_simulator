using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Commander {
    public interface IDayProgramExporter {
        void exportDayProgram(DateTime date, string outFile);
    }

    public class DayProgramHtmlExporter : IDayProgramExporter {
        public void exportDayProgram(DateTime date, string outName) {
            string fileName = System.IO.Path.GetTempPath() + outName;
            using (StreamWriter w = new StreamWriter(fileName)) {

                MediaManager.Instance.RootHtml(fileName);
            }
            File.Delete(fileName);  
        }

        public void exportDayProgram(DateTime date) {
            exportDayProgram(date, String.Format("DayProgram_{0}_{0}_{0}.html", date.Year, date.Month, date.Date));
        }
    }
}
