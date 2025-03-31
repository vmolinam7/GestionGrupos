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
            csConexion = new csConexion();
            if (csConexion.ValidarUsuario(textBox1.Text.ToString(), textBox3.Text.ToString()))
            {
                MessageBox.Show("Acceso correcto");
            }else
            {
                MessageBox.Show("Credenciales incorrectas");
            }
        }
    }
}
