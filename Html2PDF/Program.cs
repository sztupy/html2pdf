/*
 * Copyright (c) 2009, Peter Nelson (charn.opcode@gmail.com)
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 * 
 * * Redistributions of source code must retain the above copyright notice, 
 *   this list of conditions and the following disclaimer.
 * * Redistributions in binary form must reproduce the above copyright notice, 
 *   this list of conditions and the following disclaimer in the documentation 
 *   and/or other materials provided with the distribution.
 *   
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE 
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
 * POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace WebKitBrowserTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
           // Application.EnableVisualStyles();
           // Application.SetCompatibleTextRenderingDefault(false);
          if (args.Length < 2) {
            Console.WriteLine("Usage: html2pdf url pdf_filename");
          } else {
            // create input and output file paths
            var bzpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"PDF Writer\Bullzip PDF Printer\runonce.ini");
            var tmpname = Path.Combine(Path.GetTempPath(),String.Format("{0}{1}.tmp", System.IO.Path.GetRandomFileName(), Guid.NewGuid().ToString()));
            using (FileStream fs = new FileStream(bzpath, FileMode.Create))
            {
              using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
              {
                sw.WriteLine("[PDF Printer]");
                sw.WriteLine(String.Format("Output={0}",args[1]));
                sw.WriteLine("ShowPDF=no");
                sw.WriteLine("ConfirmOverwrite=no");
                sw.WriteLine(String.Format("StatusFile={0}",tmpname));
                sw.WriteLine("ShowSettings=never");
              }
            }

            // load browser and print page
            try
            {
              var m = new MainForm(args[0], tmpname);
              Application.Run(m);
              // output the status message sent from bullzip
              using (FileStream fs = new FileStream(tmpname, FileMode.Open))
              {
                using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.Unicode))
                {
                  System.Console.WriteLine(sr.ReadToEnd());
                }
              }
              File.Delete(tmpname);
            }
            catch (Exception e)
            {
              Console.WriteLine("A possible error occured: {0}",e.Message);
            }
          }
        }
    }
}
