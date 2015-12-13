using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFO.Commander {
    public interface IDayProgramExporter {
        void exportDayProgram(DateTime date, string outFile);
    }

    public class DayProgramHtmlExporter : IDayProgramExporter {
        public void exportDayProgram(DateTime date, string outFile) {
            throw new NotImplementedException();
        }
    }
}
