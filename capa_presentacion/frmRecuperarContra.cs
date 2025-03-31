using capa_negocios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace capa_presentacion
{
    public partial class frmRecuperarContra: Form
    {
        public frmRecuperarContra()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox3.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Por favor, ingrese un correo electrónico.");
                return;
            }

            CsUsuarios csUsuarios = new CsUsuarios();

            if(!csUsuarios.EsEmailValido(email))
            {
                MessageBox.Show("Por favor, ingrese un correo válido.");
                return;
            }
            bool exito = csUsuarios.recuperarContrasenia(email);

            if (exito)
            {
                MessageBox.Show("Se ha enviado la contraseña a su correo.");
            }
            else
            {
                MessageBox.Show("El correo ingresado no está registrado.");
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 principal = Application.OpenForms["Form1"] as Form1;

            if (principal != null)
            {
                principal.MostrarPanelLogin();
                this.Close();
            }
            else
            {
                MessageBox.Show("Error: No se pudo obtener la referencia del formulario principal.");
            }
        }

        private void frmRecuperarContra_Load(object sender, EventArgs e)
        {
            panel2.BackColor = Color.FromArgb(100, 0, 0, 0);
        }
    }
}
