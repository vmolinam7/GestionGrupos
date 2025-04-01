using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using capa_datos;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace capa_presentacion
{
    public partial class fmrLogin: Form
    {
        csConexion csConexion;
        public fmrLogin()
        {
            InitializeComponent();
        }

        private void fmrLogin_Load(object sender, EventArgs e)
        {
            panel2.BackColor = Color.FromArgb(100, 0, 0, 0);
            textBox1.Focus();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 principal = Application.OpenForms["Form1"] as Form1;

            if (principal != null)
            {
                principal.volverlogin();
                this.Close();
            }
            else
            {
                MessageBox.Show("Error: No se pudo obtener la referencia del formulario principal.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            validar_usuario();
        }

        private void validar_usuario()
        {
            csConexion = new csConexion();
            if (csConexion.ValidarUsuario(textBox1.Text.ToString(), textBox3.Text.ToString()))
            {
                MessageBox.Show("Acceso correcto");
                Form1 principal = Application.OpenForms["Form1"] as Form1;
                principal.modificar_nombre_user();
                principal.mostrar();
                this.Close();
            }
            else
            {
                MessageBox.Show("Credenciales incorrectas");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 principal = Application.OpenForms["Form1"] as Form1;

            if (principal != null)
            {
                principal.recuperarcontra();
                this.Close();
            }
            else
            {
                MessageBox.Show("Error: No se pudo obtener la referencia del formulario principal.");
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                textBox3.Focus();
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                validar_usuario();
            }
        }
    }
}
