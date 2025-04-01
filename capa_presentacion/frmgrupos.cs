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
using capa_negocios;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace capa_presentacion
{
    public partial class frmgrupos : Form
    {
        CsUsuarios usuarios = new CsUsuarios();
        public frmgrupos()
        {
            InitializeComponent();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        CsUsuarios objUsuarios = new CsUsuarios();

        private void label4_Click(object sender, EventArgs e)
        {

        }


        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    objUsuarios.cargarvalorescombo(comboBox1.Text, comboBox1);
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(CsSesionActiva.CreadorID.ToString());
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleccione un participante para agregar.",
                               "Información requerida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Por favor, primero ingrese una cantidad para continuar.",
                               "Información requerida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int usuarioID = Convert.ToInt32(comboBox1.SelectedValue);
            string nombreCompleto = comboBox1.Text.Trim(); ;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["UsuarioID"].Value != null &&
                    Convert.ToInt32(row.Cells["UsuarioID"].Value) == usuarioID)
                {
                    MessageBox.Show("Este participante ya ha sido agregado al grupo.",
                                   "Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            decimal montoPorPersona = CalcularMontoPorParticipante();

            dataGridView1.Rows.Add(usuarioID, nombreCompleto, montoPorPersona);

            ActualizarMontosTodos();

            comboBox1.SelectedIndex = -1;
            dataGridView1.ClearSelection();
        }

        private void InicializarDataGridView()
        {
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("UsuarioID", "ID");
            dataGridView1.Columns.Add("NombreCompleto", "Nombre del Participante");
            dataGridView1.Columns.Add("MontoPagar", "Monto a Pagar");

            DataGridViewButtonColumn btnEliminar = new DataGridViewButtonColumn();
            btnEliminar.HeaderText = "Eliminar";
            btnEliminar.Text = "X";
            btnEliminar.Name = "btnEliminar";
            btnEliminar.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(btnEliminar);

            dataGridView1.Columns["UsuarioID"].Visible = false;


            dataGridView1.Columns["NombreCompleto"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.Columns["MontoPagar"].Width = 120;

            dataGridView1.Columns["btnEliminar"].Width = 80;
        }

        private decimal CalcularMontoPorParticipante()
        {
            decimal montoTotal;
            if (!decimal.TryParse(textBox3.Text, out montoTotal) || montoTotal <= 0)
            {
                return 0;
            }

            int cantidadParticipantes = dataGridView1.Rows.Count;

            if (cantidadParticipantes == 0)
            {
                return montoTotal;
            }

            return Math.Round(montoTotal / cantidadParticipantes, 2);
        }

        private void ActualizarMontosTodos()
        {
            decimal montoPorPersona = CalcularMontoPorParticipante();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["MontoPagar"].Value = montoPorPersona;
            }
        }

        private void frmgrupos_Load(object sender, EventArgs e)
        {
            InicializarDataGridView();
            AgregarUsuarioLogueadoAlDataGridView();
            csEstilosDgv.AplicarEstilos(dataGridView1);
        }

        frmParticipaciones frmpart = new frmParticipaciones();

        

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["btnEliminar"].Index && e.RowIndex >= 0)
            {
                dataGridView1.Rows.RemoveAt(e.RowIndex);

                ActualizarMontosTodos();
            }
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox1.Text) ||
                    string.IsNullOrEmpty(textBox2.Text) ||
                    string.IsNullOrEmpty(textBox3.Text))
                {
                    MessageBox.Show("Por favor complete todos los campos requeridos.",
                                   "Información incompleta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string nombreGrupo = textBox1.Text.Trim();
                string descripcionGrupo = textBox2.Text.Trim();
                decimal valorTotal = decimal.Parse(textBox3.Text);
                int creadorID = CsSesionActiva.CreadorID;

                List<int> participantes = new List<int>();
                List<string> correos = new List<string>();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        int usuarioID = Convert.ToInt32(row.Cells["UsuarioID"].Value);
                        participantes.Add(usuarioID);

                        string correo = usuarios.ObtenerCorreoElectronico(usuarioID);
                        if (!string.IsNullOrEmpty(correo))
                        {
                            correos.Add(correo);
                        }
                    }
                }

                if (participantes.Count == 0)
                {
                    MessageBox.Show("Debe agregar al menos un participante al grupo.",
                                   "Información incompleta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                CsGrupos grupoManager = new CsGrupos();

                bool resultado = grupoManager.CrearGrupo(
                    nombreGrupo: nombreGrupo,
                    descripcionGrupo: descripcionGrupo,
                    valorTotal: valorTotal,
                    creadorID: creadorID,
                    participantes: participantes
                );

                if (resultado)
                {
                    DateTime fechaCreacion = DateTime.Now;

                    foreach (string correo in correos)
                    {
                        usuarios.enviarCorreo(descripcionGrupo, correo, nombreGrupo, valorTotal, Math.Round(valorTotal / participantes.Count, 2), fechaCreacion);
                    }

                    MessageBox.Show("Grupo creado exitosamente y correos enviados.",
                                   "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    dataGridView1.Rows.Clear();
                }
                else
                {
                    MessageBox.Show("No se pudo crear el grupo. Verifique los datos ingresados.",
                                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el grupo: " + ex.Message,
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AgregarUsuarioLogueadoAlDataGridView()
        {
            try
            {
                int creadorID = CsSesionActiva.CreadorID;
                decimal montoPorPersona = CalcularMontoPorParticipante();

                dataGridView1.Rows.Add(creadorID, CsSesionActiva.Nombre.ToString() + " " + CsSesionActiva.Apellido.ToString(), montoPorPersona);

                ActualizarMontosTodos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el usuario logueado: " + ex.Message);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(textBox3.Text, out decimal valorTotal) && valorTotal > 0)
            {
                ActualizarMontosTodos();
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        row.Cells["MontoPagar"].Value = 0;
                    }
                }
            }
        }
    }
}
