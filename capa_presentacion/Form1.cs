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
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel2.BackColor = Color.FromArgb(100, 0, 0, 0);
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panel2.Visible = false;
            panel_central.BackgroundImage = null;

            frmRegistrase frmregis = new frmRegistrase();
            frmregis.MdiParent = this;
            abrifrm(frmregis);
            //ocultar();

        }

        private Form frmactivo = null;
        private void abrifrm(Form frmprinci)
        {
            if (frmactivo != null) frmactivo.Close();
            frmactivo = frmprinci;
            panel_central.Controls.Add(frmprinci);
            panel_central.Tag = frmprinci;
            frmprinci.BringToFront();
            frmprinci.Dock = DockStyle.Fill;
            frmprinci.Show();
        }

        public void AbrirFormularioDesdeHijo(Form formulario)
        {
            abrifrm(formulario);
        }

        public void MostrarPanelLogin()
        {

            fmrLogin frmlogin = new fmrLogin();
            frmlogin.MdiParent = this;
            abrifrm(frmlogin);
        }

        public void volverlogin()
        {
            frmRegistrase frmregis = new frmRegistrase();
            frmregis.MdiParent = this;
            abrifrm(frmregis);
        }

    }
}
