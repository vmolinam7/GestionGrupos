using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using capa_negocios;

namespace capa_presentacion
{
    public partial class frmRegistrase: Form
    {
        CsUsuarios usuarios;
        public frmRegistrase()
        {
            InitializeComponent();
        }

        private void frmRegistrase_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(100, 0, 0, 0);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

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

        //private Form frmactivo = null;
        private void abrifrm(Form frmprinci)
        {
            //if (frmactivo != null) frmactivo.Close();
            //frmactivo = frmprinci;
            //panel_central.Controls.Add(frmprinci);
            //panel_central.Tag = frmprinci;
            //frmprinci.BringToFront();
            //frmprinci.Dock = DockStyle.Fill;
            //frmprinci.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            usuarios = new CsUsuarios();
            string nombres = textBox1.Text.Trim();
            string apellidos = textBox3.Text.Trim();
            string telefono = textBox2.Text.Trim();
            string email = textBox4.Text.Trim();
            string nombreUsuario = textBox5.Text.Trim();
            string contrasenia = textBox6.Text.Trim();

            if (string.IsNullOrEmpty(nombres) || string.IsNullOrEmpty(apellidos) ||
                string.IsNullOrEmpty(telefono) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(contrasenia))
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!usuarios.EsEmailValido(email))
            {
                MessageBox.Show("El correo electrónico no tiene un formato válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!usuarios.EsTelefonoValido(telefono))
            {
                MessageBox.Show("El número de teléfono debe contener solo dígitos y tener entre 7 y 15 caracteres.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!usuarios.EsContraseniaSegura(contrasenia))
            {
                MessageBox.Show("La contraseña debe tener al menos 8 caracteres, incluir una mayúscula, un número y un carácter especial.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool exito = usuarios.registrarse(nombres, apellidos, telefono, email, nombreUsuario, contrasenia, 1);

            if (exito)
            {
                MessageBox.Show("Se ha registrado correctamente " + nombres + ". ¡Bienvenido!");
            }
            else
            {
                MessageBox.Show("No se pudo registrar. Verifique sus datos.");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            usuarios = new CsUsuarios();
            string nombres = textBox1.Text.Trim();
            string apellidos = textBox3.Text.Trim();
            string telefono = textBox2.Text.Trim();
            string email = textBox4.Text.Trim();
            string nombreUsuario = textBox5.Text.Trim();
            string contrasenia = textBox6.Text.Trim();

            if (string.IsNullOrEmpty(nombres) || string.IsNullOrEmpty(apellidos) ||
                string.IsNullOrEmpty(telefono) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(contrasenia))
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!usuarios.EsEmailValido(email))
            {
                MessageBox.Show("El correo electrónico no tiene un formato válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!usuarios.EsTelefonoValido(telefono))
            {
                MessageBox.Show("El número de teléfono debe contener solo dígitos y tener entre 7 y 10 caracteres.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!usuarios.EsContraseniaSegura(contrasenia))
            {
                MessageBox.Show("La contraseña debe tener al menos 8 caracteres, incluir una mayúscula, un número y un carácter especial.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool exito = usuarios.registrarse(nombres, apellidos, telefono, email, nombreUsuario, contrasenia, 1);

            if (exito)
            {
                MessageBox.Show("Se ha registrado correctamente " + nombres + ". ¡Bienvenido!");
            }
            else
            {
                MessageBox.Show("No se pudo registrar. Verifique sus datos.");
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }
}
