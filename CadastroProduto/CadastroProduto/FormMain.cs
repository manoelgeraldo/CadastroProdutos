using CadastroProduto.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CadastroProduto
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Display();
        }
        public void Display()
        {
            using (PracticaEntities db = new PracticaEntities())
            {
                List<Produto> listaProdutos = new List<Produto>();
                listaProdutos = db.Produtos.Select(x => new Produto
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    Marca = x.Marca,
                    Preco = (float)x.Preco,
                    Descricao = x.Descricao,
                    Estoque = (int)x.Estoque
                }).ToList();
                dgvProdutos.DataSource = listaProdutos;
            }
        }

        private void dgvProdutos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ItemSelecionado();
        }
        public void ItemSelecionado()
        {
            if (dgvProdutos.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvProdutos.SelectedRows)
                {
                    lblIdProduto.Text = row.Cells[0].Value.ToString();
                    txtNome.Text = row.Cells[1].Value.ToString();
                    cbxMarca.Text = row.Cells[3].Value.ToString();
                    txtDescricao.Text = row.Cells[2].Value.ToString();
                    txtPreco.Text = row.Cells[4].Value.ToString();
                    txtEstoque.Text = row.Cells[5].Value.ToString();
                }
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }
        public void LimparCampos()
        {
            txtNome.Text = "";
            cbxMarca.Text = "";
            txtDescricao.Text = "";
            txtPreco.Text = "";
            txtEstoque.Text = "";
        }
        public void MostrarStatus(bool resultado, string titulo)
        {
            if (resultado)
            {
                if (titulo.ToUpper() == "NOVO")
                {
                    MessageBox.Show("Produto Incluido!", "Novo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (titulo.ToUpper() == "EDITAR")
                {
                    MessageBox.Show("Produto Editado!", "Editar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Produto Excluido!", "Excluir", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Houve algum erro! Por favor tente novamente!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            LimparCampos();
            Display();
        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            Produtos produto = new Produtos();
            produto.Nome = txtNome.Text;
            produto.Marca = cbxMarca.Text;
            produto.Preco = Convert.ToDouble(txtPreco.Text);
            produto.Descricao = txtDescricao.Text;
            produto.Estoque = Convert.ToInt32(txtEstoque.Text);
            bool resultado = Incluir(produto);
            MostrarStatus(resultado, "NOVO");
        }
        public bool Incluir(Produtos produto)
        {
            bool resultado = false;
            using (PracticaEntities db = new PracticaEntities())
            {
                db.Produtos.Add(produto);
                db.SaveChanges();
                resultado = true;
            }
            return resultado;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Produtos produto = SetValues(Convert.ToInt32(lblIdProduto.Text),
                                            txtNome.Text,
                                            txtDescricao.Text,
                                            cbxMarca.Text,
                                            Convert.ToSingle(txtPreco.Text),
                                            Convert.ToInt32(txtEstoque.Text));
            bool resultado = Editar(produto);
            MostrarStatus(resultado, "EDITAR");
        }
        public bool Editar(Produtos produto)
        {
            bool resultado = false;
            using (PracticaEntities db = new PracticaEntities())
            {
                Produtos _produto = db.Produtos.Where(x => x.Id == produto.Id).Select(x => x).FirstOrDefault();
                _produto.Id = produto.Id;
                _produto.Nome = produto.Nome;
                _produto.Marca = produto.Marca;
                _produto.Preco = produto.Preco;
                _produto.Descricao = produto.Descricao;
                _produto.Estoque = produto.Estoque;
                db.SaveChanges();
                resultado = true;
            }
            return resultado;
        }
        public Produtos SetValues(int id, string nome, string descricao, string marca, float preco, int estoque)
        {
            Produtos produto = new Produtos()
            {
                Id = id,
                Nome = nome,
                Descricao = descricao,
                Marca = marca,
                Preco = preco,
                Estoque = estoque,
            };
            return produto;
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            Produtos produto = SetValues(Convert.ToInt32(lblIdProduto.Text),
                                            txtNome.Text,
                                            txtDescricao.Text,
                                            cbxMarca.Text,
                                            Convert.ToSingle(txtPreco.Text),
                                            Convert.ToInt32(txtEstoque.Text));
            bool resultado = Excluir(produto);
            MostrarStatus(resultado, "EXCLUIR");
        }
        public bool Excluir(Produtos produto)
        {
            bool resultado = false;
            using (PracticaEntities db = new PracticaEntities())
            {
                Produtos _produto = db.Produtos.Where(x => x.Id == produto.Id).Select(x => x).FirstOrDefault();
                db.Produtos.Remove(_produto);
                db.SaveChanges();
                resultado = true;
            }
            return resultado;
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
