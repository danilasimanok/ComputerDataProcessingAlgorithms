using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Pathfinder_joke_
{
    public class PDFGenerator
    {
        String dot, output;

        public PDFGenerator(String dot, String output) {
            this.dot = dot;
            this.output = output;
        }

        public void GeneratePDF() {
            String tmp = this.output + ".tmp";
            using (StreamWriter writer = File.CreateText(tmp))
                writer.Write(this.dot);
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "dot";
                process.StartInfo.Arguments = "-Tpdf -o" + output + " " + tmp;
                process.Start();
                while (!process.HasExited)
                    process.Refresh();
            }
            File.Delete(tmp);
        }
    }
}
