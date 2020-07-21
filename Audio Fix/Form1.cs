using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Audio_Fix
{
    public partial class frmIDSearch : Form
    {
        public frmIDSearch()
        {
            InitializeComponent();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string strPath = txtPath.Text;
            int counter = 0;

            //Regex
            string pattern = @"IdExibicao=""(\w+)""";
            


            string target = "IdExibicao=";
            int iFrom;
            string line, firstPart, secondPart;
            List<string> result = new List<string>();

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(@"D:\OneDrive - Globo Comunicação e Participações sa\03.Projetos\Globo - Audio Fix\getRedeIds\A) EXIBICAO - GATEWAY - RIO HD_2020-01-12_00-00-00.txt");
            while ((line = file.ReadLine()) != null)
            {
                //string input = line;
                //MatchCollection collection = Regex.Matches(input, pattern);
                //foreach (Match m in collection)
                //{
                //    result.Add(m.Value);
                //}
                if (line.IndexOf(target) > 1)
                {
                    iFrom = line.IndexOf("IdExibicao=");
                    firstPart = line.Substring(iFrom + target.Length).Substring(1);
                    secondPart = firstPart.Substring(0, firstPart.IndexOf("\""));
                    result.Add(secondPart);
                }
                counter++;
            }

            file.Close();

            List<string> uniqueList = result.Distinct().ToList();

            uniqueList.ForEach(i => Console.WriteLine($"{i}"));
            System.Console.WriteLine("Count of ID: {0}", result.Count);
            System.Console.WriteLine("Count of ID: {0}", uniqueList.Count);
            System.Console.WriteLine("There were {0} lines.", counter);

            var source = new BindingSource
            {
                DataSource = uniqueList
            };
            dataGridView1.DataSource = source;

            //var list = new BindingList<string>(uniqueList);
            //dataGridView1.DataSource = list.;
            // Suspend the screen.  
            System.Console.ReadLine();
        }

        private void BtnConvert_Click(object sender, EventArgs e)
        {
            string output = $"output-{DateTime.Now.Ticks}.mp4";
            string input = "sample.mxf";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "ffmpeg.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = $"-i {input} {output}";
            using (Process exeProcess = Process.Start(startInfo))
            {
                exeProcess.WaitForExit();
            }
            string argument = "/select, \"" + output + "\"";
            Process.Start("explorer.exe", argument);
        }

        private void BtnProbe_Click(object sender, EventArgs e)
        {
            string input = "sample.mxf";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "CMD.EXE";
            startInfo.Arguments = $"/k ffprobe.exe -i {input} -show_streams -select_streams a:0";
            Process p = Process.Start(startInfo);
            p.WaitForExit();
     
        }
    }
}
