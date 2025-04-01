using capa_negocios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace capa_presentacion
{
    public partial class frmRegPago : Form
    {
        private int _pagoID;
        private double cant;
        CsPagos pagos= new CsPagos();
        CsUsuarios usas= new CsUsuarios();
        public frmRegPago(int pagoID, double cantidad)
        {
            InitializeComponent();
            _pagoID = pagoID;
            cant = cantidad;

            CargarMetodosPago();

            panelPaypal.Visible = false;
            panelTarjeta.Visible = false;
            panelEfectivo.Visible = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmRegPago_Load(object sender, EventArgs e)
        {
            textBox6.Text = cant.ToString();
            textBox6.Enabled= false;
            textBox7.Text = cant.ToString();
            textBox7.Enabled = false;
            textBox8.Text = cant.ToString();
            textBox8.Enabled = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void CargarMetodosPago()
        {
            try
            {
                DataTable dt = pagos.ObtenerMetodosPago();

                comboBox1.DisplayMember = "NOMBRE";
                comboBox1.ValueMember = "METODOID";
                comboBox1.DataSource = dt;

                comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los métodos de pago: " + ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string metodoSeleccionado = comboBox1.Text.Trim().ToLower();
            panelPaypal.Visible = false;
            panelEfectivo.Visible = false;
            panelTarjeta.Visible = false;

            int anchoPanel = this.ClientSize.Width / 2;
            int altoPanel = panelPaypal.Height;

            int posicionX = (this.ClientSize.Width - anchoPanel) / 2;

            int posicionY = 50; 

            if (metodoSeleccionado == "paypal")
            {
                panelPaypal.Size = new Size(anchoPanel, altoPanel);
                panelPaypal.Location = new Point(posicionX, posicionY);
                panelPaypal.Visible = true;
            }
            else if (metodoSeleccionado == "efectivo")
            {
                panelEfectivo.Size = new Size(anchoPanel, altoPanel);
                panelEfectivo.Location = new Point(posicionX, posicionY);
                panelEfectivo.Visible = true;
            }
            else if (metodoSeleccionado == "tarjeta de crédito")
            {
                panelTarjeta.Size = new Size(anchoPanel, altoPanel);
                panelTarjeta.Location = new Point(posicionX, posicionY);
                panelTarjeta.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int metodoID = Convert.ToInt32(comboBox1.SelectedValue);
            try
            {
                string correo = textBox1.Text.Trim();
                string contraseña = textBox2.Text.Trim();


                if (string.IsNullOrEmpty(correo))
                {
                    MessageBox.Show("Por favor, ingrese el correo de PayPal.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!usas.EsEmailValido(correo))
                {
                    MessageBox.Show("Ingrese un correo de PayPal válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(contraseña))
                {
                    MessageBox.Show("Por favor, ingrese su contraseña de PayPal.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (contraseña.Length < 6)
                {
                    MessageBox.Show("La contraseña debe tener al menos 6 caracteres.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                pagos.RegistrarPagoPayPal(_pagoID, metodoID, correo, cant);

                MessageBox.Show("Pago registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Formato incorrecto en los datos ingresados.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Error en la base de datos: " + sqlEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar el pago con PayPal: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int metodoID = Convert.ToInt32(comboBox1.SelectedValue);
            try
            {
                string numeroTarjeta = textBox4.Text.Trim();
                string titular = textBox5.Text.Trim();
                string cantidadStr = textBox6.Text.Trim();

                if (string.IsNullOrEmpty(numeroTarjeta))
                {
                    MessageBox.Show("Por favor, ingrese el número de tarjeta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (numeroTarjeta.Length != 16 || !long.TryParse(numeroTarjeta, out _))
                {
                    MessageBox.Show("Ingrese un número de tarjeta válido (16 dígitos numéricos).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(titular))
                {
                    MessageBox.Show("Por favor, ingrese el nombre del titular de la tarjeta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(cantidadStr))
                {
                    MessageBox.Show("Por favor, ingrese la cantidad a pagar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(cantidadStr, out decimal cantidad) || cantidad <= 0)
                {
                    MessageBox.Show("Ingrese una cantidad válida mayor a 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DateTime fechaVence = DateTime.Now.AddMonths(1);

                pagos.RegistrarPagoTarjeta(textBox3.Text.ToString(),_pagoID, metodoID, numeroTarjeta, fechaVence, titular, cantidad);

                MessageBox.Show("Pago registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Formato incorrecto en los datos ingresados.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Error en la base de datos: " + sqlEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar el pago con tarjeta de crédito: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int metodoID = Convert.ToInt32(comboBox1.SelectedValue); // Obtener el ID del método de pago
            try
            {
                string cantidadStr = textBox8.Text.Trim();

                if (string.IsNullOrEmpty(cantidadStr))
                {
                    MessageBox.Show("Por favor, ingrese la cantidad a pagar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(cantidadStr, out decimal cantidad) || cantidad <= 0)
                {
                    MessageBox.Show("Ingrese una cantidad válida mayor a 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                pagos.RegistrarPagoEfectivo(_pagoID, metodoID, cantidad);
                MessageBox.Show("Pago registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Formato incorrecto en los datos ingresados.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Error en la base de datos: " + sqlEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar el pago en efectivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }
}
