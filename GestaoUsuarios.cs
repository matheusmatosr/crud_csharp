﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace novo_projeto_anker
{
    public partial class GestaoUsuarios : Form
    {
        // Referência ao Form1 para comunicação entre formulários
        public Form1 Form1Reference { get; set; }

        public GestaoUsuarios()
        {
            InitializeComponent();
        }

        // Carrega os dados dos usuários no DataGridView ao carregar o formulário
        private void GestaoUsuarios_Load(object sender, EventArgs e)
        {
            dgv_usuarios.DataSource = Banco.ObterTodosUsuariosIdNomes();
            dgv_usuarios.Columns[0].Width = 60;
            dgv_usuarios.Columns[1].Width = 185;
        }

        // Atualiza os campos de texto com os dados do usuário selecionado no DataGridView
        private void dgv_usuarios_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            int contlinhas = dgv.SelectedRows.Count;

            if (contlinhas > 0)
            {
                DataTable dt = new DataTable();
                string vid = dgv.SelectedRows[0].Cells[0].Value.ToString();
                dt = Banco.ObterDadosUsuários(vid);

                tb_id.Text = dt.Rows[0].Field<Int64>("N_IDUSUARIO").ToString();
                tb_name.Text = dt.Rows[0].Field<string>("T_NOMEUSUARIO").ToString();
                tb_senha.Text = dt.Rows[0].Field<string>("T_SENHA").ToString();
                cb_status.Text = dt.Rows[0].Field<string>("T_STATUSUSUARIO").ToString();
                n_nivel.Value = dt.Rows[0].Field<Int64>("N_NIVELUSUARIO");
            }
        }

        // Abre o formulário para adicionar um novo usuário
        private void btn_novo_Click(object sender, EventArgs e)
        {
            NovoUsuario novoUsuario = new NovoUsuario();
            novoUsuario.ShowDialog();
            dgv_usuarios.DataSource = Banco.ObterTodosUsuariosIdNomes();
        }

        // Salva as alterações feitas no usuário selecionado
        private void btn_salvar_Click(object sender, EventArgs e)
        {
            int linha = dgv_usuarios.SelectedRows[0].Index; // grava a linha que está selecionada

            // Pegando os dados atualizados
            Usuario u = new Usuario();
            u.id = Convert.ToInt32(tb_id.Text);
            u.nome = tb_name.Text;
            u.senha = tb_senha.Text;
            u.status = cb_status.Text;
            u.nivel = Convert.ToInt32(Math.Round(n_nivel.Value, 0));

            Banco.AtualizarUsuario(u);
            dgv_usuarios.DataSource = Banco.ObterTodosUsuariosIdNomes();
            dgv_usuarios.CurrentCell = dgv_usuarios[0, linha]; // mantém na seleção que está sendo modificada o usuário.

            MessageBox.Show("Alterações salvas com sucesso!");
        }

        // Exclui o usuário selecionado após confirmação
        private void btn_excluir_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Deseja realmente excluir este usuário?", "Excluir?", MessageBoxButtons.YesNo); // confirmar exclusão

            if (res == DialogResult.Yes)
            {
                Banco.DeletarUsuario(tb_id.Text);
                dgv_usuarios.Rows.Remove(dgv_usuarios.CurrentRow);
                MessageBox.Show("Usuário excluído com sucesso!");
            }
        }

        private void btn_fechar_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void dgv_usuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgv_usuarios.SelectedRows.Count > 0)
            {
                string idUsuarioSelecionado = dgv_usuarios.SelectedRows[0].Cells[0].Value.ToString();
                DataTable dt = Banco.ObterDadosUsuários(idUsuarioSelecionado);
                string nomeUsuarioSelecionado = dt.Rows[0].Field<string>("T_NOMEUSUARIO");

                // Verifique se a referência ao Form1 é nula
                if (Form1Reference != null)
                {
                    // Configure as informações do usuário diretamente do Form1
                    string nomeUsuarioLogado = Form1Reference.lb_nomeUsuario.Text;
                    string idUsuarioLogado = Form1Reference.lb_acesso.Text;

                    // Comparar se o usuário logado é o mesmo que está sendo visualizado
                    if (nomeUsuarioSelecionado == nomeUsuarioLogado)
                    {
                        // Criar uma instância do formulário Horarios
                        Horarios horariosForm = new Horarios();

                        // Definir informações do usuário
                        horariosForm.NomeUsuario = nomeUsuarioSelecionado;
                        horariosForm.IdUsuario = idUsuarioLogado;

                        // Mostrar o formulário Horarios
                        horariosForm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Você só pode visualizar rotinas do seu próprio usuário.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione um usuário antes de criar uma rotina.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void visuRotinas_Click(object sender, EventArgs e)
        {
            if (dgv_usuarios.SelectedRows.Count > 0)
            {
                string idUsuarioSelecionado = dgv_usuarios.SelectedRows[0].Cells[0].Value.ToString();
                DataTable dt = Banco.ObterDadosUsuários(idUsuarioSelecionado);
                string nomeUsuarioSelecionado = dt.Rows[0].Field<string>("T_NOMEUSUARIO");

                // Verifique se a referência ao Form1 é nula
                if (Form1Reference != null)
                {
                    // Configure as informações do usuário diretamente do Form1
                    string nomeUsuarioLogado = Form1Reference.lb_nomeUsuario.Text;

                    // Comparar se o usuário logado é o mesmo que está sendo visualizado
                    if (nomeUsuarioSelecionado == nomeUsuarioLogado)
                    {
                        VerRotina rotinas = new VerRotina();
                        rotinas.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Você só pode visualizar rotinas do seu próprio usuário.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

    }
}
