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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Printing;
using WebKit;
using WebKit.DOM;
using System.Threading;
using System.IO;

namespace WebKitBrowserTest
{
    public partial class MainForm : Form
    {
        WebKitBrowser browser;
        FileSystemWatcher fsw;
        string temp_file_name;

        public MainForm(string url, string tmpname)
        {
            InitializeComponent();

            browser = new WebKitBrowser();
            browser.Visible = true;
            browser.Dock = DockStyle.Fill;
            browser.Name = "browser";
            browser.IsWebBrowserContextMenuEnabled = false;
            browser.IsScriptingEnabled = false;

            Controls.Add(browser);
            RegisterBrowserEvents();            

            fsw = new FileSystemWatcher(Path.GetDirectoryName(tmpname));
            fsw.Created += new FileSystemEventHandler(fsw_Created);
            fsw.EnableRaisingEvents = true;
            temp_file_name = tmpname;

            browser.Navigate(url);
            browser.Focus();
        }

        void fsw_Created(object sender, FileSystemEventArgs e)
        {
          if (e.FullPath == temp_file_name)
          {
            Close();
          }
        }

        private void RegisterBrowserEvents()
        {
            browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(browser_DocumentCompleted);
        }

 
        void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            browser.PageSettings.Margins.Bottom = 0;
            browser.PageSettings.Margins.Left = 0;
            browser.PageSettings.Margins.Right = 0;
            browser.PageSettings.Margins.Top = 0;
            browser.Print();
        }

        private void UnregisterBrowserEvents()
        {
            browser.DocumentCompleted -= browser_DocumentCompleted;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
          Visible = false;
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
          Visible = false;
        }
    }
}
