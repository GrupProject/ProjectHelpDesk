﻿using Library.Class.Models;
using Library.Class.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UI.Business.Interfaces.Repositories;
using UI.Business.Interfaces.Repositories.Business;
using static Library.Class.Enum.EnumSexo;


namespace UI.Desktop
{
    public partial class frmUsuario : Form,ISpeech
    {

        private readonly ControlUsuario _RepositoryControlUsuario;
        private readonly ControlDepartamento _RepositoryControlDepartamento;
        private readonly ControlTipoUsuario _RepositoryControlTipoUsuario;
        private readonly ControlConfigFonte _RepositoryControlConfigFonte;

        public frmUsuario(MenuPrincipal menu)
        {
            InitializeComponent();
            _RepositoryControlUsuario = new ControlUsuario();
            _RepositoryControlDepartamento = new ControlDepartamento();
            _RepositoryControlTipoUsuario = new ControlTipoUsuario();
            _RepositoryControlConfigFonte = new ControlConfigFonte();

            //Configurando o tamanho da fonte
            // new ConfigFont("Arial", 14F))
            var fonte = _RepositoryControlConfigFonte.Pesquisar((int)menu.toolStripMenuItem2.Tag);

            if(fonte != null)
                this.ConfigurarTamanhoFonte(fonte);
        }

        public frmUsuario(int id)
        {
            InitializeComponent();
            _RepositoryControlUsuario = new ControlUsuario();
            _RepositoryControlDepartamento = new ControlDepartamento();
            _RepositoryControlTipoUsuario = new ControlTipoUsuario();
            _RepositoryControlConfigFonte = new ControlConfigFonte();


            if (id != 0)
            {
                //Configurando o tamanho da fonte
                // new ConfigFont("Arial", 14F))
                this.ConfigurarTamanhoFonte(_RepositoryControlConfigFonte.Pesquisar(id));
                
                CompletarCampos(_RepositoryControlUsuario.Pesquisar(id));

                toolStripButton1.Enabled = true;
                salvarToolStripButton1.Enabled = false;
            }
        }

        private void ValidatingTextBox(object sender, CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == null || textBox.Text == "" || textBox.Text == "0")
            {
                errorProvider1.SetError(textBox, helpProvider1.GetHelpString(textBox));
            }
            else
            {

            }

        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(textBox2.Text);
        }

        private void Usuario_Load(object sender, EventArgs e)
        {
            Pesquisar(textBox2.Text);

            btnCadastrar.Enabled = true;
            toolStripButton1.Enabled = true;

            button1.Enabled = false;
            salvarToolStripButton1.Enabled = false;


            comboDepartamento.CarregarCombo<Departamentos>(
                _RepositoryControlDepartamento.PesquisarDepartamento().ToList(),
                "CodigoDepartamento", "Nome");

            comboTipo.CarregarCombo<TipoUsuarios>(
                _RepositoryControlTipoUsuario.PesquisarTipoUsuario().ToList(),
                "CodigoTipoUsuario", "Nome");


            comboSexo.DataSource = Enum.GetValues(typeof(Sexo));
        }

        private void ValidationTextBox(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == null || textBox.Text == "" || textBox.Text == "0") errorProvider1.SetError(textBox, helpProvider1.GetHelpString(textBox));
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Pesquisar(textBox2.Text);
        }

        private void Pesquisar(string nome)
        {
            dataGridView1.AutoGenerateColumns = true;

            var Data = _RepositoryControlUsuario.PesquisarUsuario("")
                .Where(a => a.Nome.Contains(nome))
                .Select(a => new
                {
                    ID = a.CodigoUsuario
                ,
                    Nome = a.Nome
                    //,Departamento = a.Departamento.Descricao
                }
                    )
                .Take(10)
                .ToList();

            dataGridView1.DataSource = Data;

            if (Data != null)
            {
                dataGridView1.Columns["ID"].Visible = false;
                dataGridView1.Columns["ID"].ReadOnly = true;

                DataGridViewColumn coluna = dataGridView1.Columns["Del"];
                if (coluna != null)
                {
                    dataGridView1.Columns.Remove(coluna);
                }
                DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
                btnColumn.Name = "Del";
                btnColumn.HeaderText = "Ação";
                btnColumn.Text = "Excluir";
                btnColumn.UseColumnTextForButtonValue = true;
                btnColumn.CellTemplate.Style.BackColor = Color.Orange;
                dataGridView1.Columns.Add(btnColumn);
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        private void Limpar()
        {
            this.ClearControlAll();
            errorProvider1.Clear();

            btnCadastrar.Enabled = true;
            toolStripButton1.Enabled = true;

            button1.Enabled = false;
            salvarToolStripButton1.Enabled = false;


        }

        private void CompletarCampos(Usuarios usuarios)
        {

            textNome.Text = usuarios.Nome;
            textCPF.Text = usuarios.CPF;
            dateTimeNascimento.Value = usuarios.DataNascimento.Value;
            comboSexo.SelectedItem = usuarios.Sexo;

            if (usuarios.Usuario != null)
            {
                comboTipo.SelectedIndex = (int)usuarios.Usuario.CodigoTipoUsuario;
            }

            if (usuarios.Departamento != null)
            {
                comboDepartamento.SelectedIndex = (int)usuarios.Departamento.CodigoDepartamento;
            }

            textRua.Text = usuarios.Endereco.Rua;
            textCEP.Text = usuarios.Endereco.CEP;
            textBairro.Text = usuarios.Endereco.Bairro;
            textCidade.Text = usuarios.Endereco.Cidade;

            if (usuarios.Departamento != null)
            {
                comboUF.SelectedItem = usuarios.Endereco.UF.ToString();
            }
            

            textN.Text = usuarios.Endereco.Numero.ToString();
            textEmail.Text = usuarios.Email;
            textTelUm.Text = usuarios.Telefone;
            textTelDois.Text = usuarios.Celular;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //var campo = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            var campo = dataGridView1.CurrentRow.Cells["ID"].Value.ToString();

            if (e.ColumnIndex == dataGridView1.Columns["Del"].Index)
            {
                if (DialogResult.Yes == MessageBox.Show("Você deseja excluir este registro ?", "", MessageBoxButtons.YesNo))
                {
                    var retorno = _RepositoryControlUsuario.Remover(Convert.ToInt32(campo));

                    lblAtencao.Text = retorno.Propert + ": " + retorno.Message;

                    Pesquisar(textBox2.Text);
                    Limpar();
                }
            }
            else
            {
                CompletarCampos(_RepositoryControlUsuario.Pesquisar(Convert.ToInt32(campo)));
            }


            btnCadastrar.Enabled = false;
            toolStripButton1.Enabled = false;

            button1.Enabled = true;
            salvarToolStripButton1.Enabled = true;

        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {

            if (comboSexo.SelectedItem != null)
            {
                Sexo DadosSexo = (Sexo)Enum.Parse(typeof(Sexo), comboSexo.SelectedItem.ToString());

                var retorno = _RepositoryControlUsuario.CadastrarUsuario(textNome.Text, dateTimeNascimento.Value, textEmail.Text, textCPF.Text, DadosSexo, textTelUm.Text, textTelDois.Text,
                    textRua.Text, textBairro.Text, textCEP.Text, textCidade.Text, Convert.ToInt32(textN.Text), comboUF.SelectedText.ToString(),textComplemento.Text,
                    (int)comboTipo.SelectedIndex,
                    (int)comboDepartamento.SelectedIndex);

                lblAtencao.Text = retorno.Propert + ": " + retorno.Message;

                if (retorno.Status)
                {
                    Pesquisar(textBox2.Text);
                    Limpar();
                }

            }
            else
            {
                lblAtencao.Text = "Sexo" + ": " + "Selecione um sexo";
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (comboSexo.SelectedItem != null)
            {
                Sexo DadosSexo = (Sexo)Enum.Parse(typeof(Sexo), comboSexo.SelectedItem.ToString());

                //var campo = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                var campo = dataGridView1.CurrentRow.Cells["ID"].Value.ToString();

                var retorno = _RepositoryControlUsuario.AlterarUsuario(Convert.ToInt32(campo), textNome.Text, dateTimeNascimento.Value, textEmail.Text, textCPF.Text, DadosSexo, textTelUm.Text, textTelDois.Text,
                    textRua.Text, textBairro.Text, textCEP.Text, textCidade.Text, Convert.ToInt32(textN.Text), comboUF.SelectedText.ToString(), textComplemento.Text,
                    (int)comboTipo.SelectedIndex + 1,
                    (int)comboDepartamento.SelectedIndex + 1);

                lblAtencao.Text = retorno.Propert + ": " + retorno.Message;

                if (retorno.Status)
                {
                    Pesquisar(textBox2.Text);
                    Limpar();
                }
            }
            else
            {
                lblAtencao.Text = "Sexo" + ": " + "Selecione um sexo";
            }


        }

        private void salvarToolStripButton1_Click(object sender, EventArgs e)
        {

            if (comboSexo.SelectedItem != null)
            {
                Sexo DadosSexo = (Sexo)Enum.Parse(typeof(Sexo), comboSexo.SelectedItem.ToString());

                //var campo = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                var campo = dataGridView1.CurrentRow.Cells["ID"].Value.ToString();

                var retorno = _RepositoryControlUsuario.AlterarUsuario(Convert.ToInt32(campo), textNome.Text, dateTimeNascimento.Value, textEmail.Text, textCPF.Text, DadosSexo, textTelUm.Text, textTelDois.Text,
                    textRua.Text, textBairro.Text, textCEP.Text, textCidade.Text, Convert.ToInt32(textN.Text), comboUF.SelectedText.ToString(), textComplemento.Text,
                (int)comboTipo.SelectedIndex,
                (int)comboDepartamento.SelectedIndex);


                lblAtencao.Text = "• " + retorno.Propert + ": " + retorno.Message;

                if (retorno.Status)
                {
                    Pesquisar(textBox2.Text);
                    Limpar();
                    this.Refresh();
                }
            }
            else
            {
                lblAtencao.Text = "Sexo" + ": " + "Selecione um sexo";
            }


        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (comboSexo.SelectedItem != null)
            {
                Sexo DadosSexo = (Sexo)Enum.Parse(typeof(Sexo), comboSexo.SelectedItem.ToString());

                var retorno = _RepositoryControlUsuario.CadastrarUsuario(textNome.Text, dateTimeNascimento.Value, textEmail.Text, textCPF.Text, DadosSexo, textTelUm.Text, textTelDois.Text,
                    textRua.Text, textBairro.Text, textCEP.Text, textCidade.Text, StringExtension.ToInt32(textN.Text), comboUF.SelectedText.ToString(), textComplemento.Text,
                    (int)comboTipo.SelectedIndex,
                    (int)comboDepartamento.SelectedIndex);

                lblAtencao.Text = "• " + retorno.Propert + ": " + retorno.Message;

                if (retorno.Status)
                {
                    Pesquisar(textBox2.Text);
                    Limpar();
                }
            }
            else
            {

                lblAtencao.Text = "• " + "Sexo" + ": " + "Selecione um sexo";

            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        private void btnPesquisar_Click_1(object sender, EventArgs e)
        {
            Pesquisar(textBox2.Text);
        }

        public void LabelLeave(object sender, EventArgs e)
        {
            Label dina = (Label)sender;
            ExtensionSpeech.speech(dina.Text);
        }

        public void ButtonLeave(object sender, EventArgs e)
        {
            Button dina = (Button)sender;
            ExtensionSpeech.speech(dina.Text);
        }
        

    }
}
