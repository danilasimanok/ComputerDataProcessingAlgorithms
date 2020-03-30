using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TransitiveClosure
{
    static class DotGenerator
    {
        public static void createDot(Boolean[][] origin, Boolean[][] result, String output) {
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
    }
}
