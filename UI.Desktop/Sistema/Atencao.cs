﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI.Desktop
{
    public partial class Atencao : Form
    {
        public Atencao()
        {
            InitializeComponent();
        }

        private void Atencao_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = "Bem vindo ao sistema Help Desk";
        }
    }
}
