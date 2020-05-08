using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TransitiveClosure
{
    public static class DotMediator
    {
        public static void CreateDot(Boolean[][] origin, Boolean[][] result, String output) {
            int i, j;
            List<String> lines = new List<String>() { "digraph G {" };
            for (i = 0; i < result.Length; ++i)
                lines.Add(i.ToString() + " ;");
            for (i = 0; i < result.Length; ++i)
                for (j = 0; j < result[0].Length; ++j)
                    if (result[i][j].value)
                        lines.Add(i.ToString() + " -> " + j.ToString() + " [" + (origin[i][j].value ? "" : "color=red") + "];");
            lines.Add("}");
            File.WriteAllLines(output, lines);
        }

        public static void ProcessDot(String dotfile, String output) {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "dot";
                process.StartInfo.Arguments = "-Tpdf -o" + output + " " + dotfile;
                if (process.Start())
                    process.WaitForExit();
                else
                    throw new Exception("Can not start process.");
            }
        }
    }
}
