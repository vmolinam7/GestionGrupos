using capa_datos;
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
    public partial class frmPagos: Form
    {
        csDetalleGrupo detallesg=new csDetalleGrupo();
        CsPagos pagos = new CsPagos();
        public frmPagos()
        {
            InitializeComponent();
        }

        private void frmPagos_Load(object sender, EventArgs e)
        {
            detallesg.cargarValoresTabla(CsSesionActiva.CreadorID, dataGridView1, "spObtenerPagosUsuario");
            csEstilosDgv.AplicarEstilos(dataGridView1);

            dataGridView1.Columns["PagoID"].Visible = false;

            if (!dataGridView1.Columns.Contains("Pagar"))
            {
                DataGridViewButtonColumn btnPagar = new DataGridViewButtonColumn();
                btnPagar.HeaderText = "Acción";
                btnPagar.Name = "Pagar";
                btnPagar.UseColumnTextForButtonValue = false;
                dataGridView1.Columns.Add(btnPagar);
            }

            dataGridView1.DataBindingComplete += (s, ev) => ActualizarBotonesAccion();
        }
        private void ActualizarBotonesAccion()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["EstadoPago"].Value.ToString() == "Pagado")
                {
                    row.Cells["Pagar"].Value = "Detalles";
                    row.Cells["Pagar"].Style.BackColor = Color.OrangeRed;
                }
                else
                {
                    row.Cells["Pagar"].Value = "Pagar";
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["Pagar"].Index)
            {
                int pagoID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["PagoID"].Value);
                string estadoPago = dataGridView1.Rows[e.RowIndex].Cells["EstadoPago"].Value.ToString();

                if (estadoPago == "Pagado")
                {
                    string detallesPago = pagos.ObtenerDetallesPago(pagoID);
                    MessageBox.Show(detallesPago, "Detalles del Pago", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    double cant = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["ValorAPagar"].Value);
                    using (frmRegPago formularioPago = new frmRegPago(pagoID, cant))
                    {
                        formularioPago.ShowDialog();
                    }
                    detallesg.cargarValoresTabla(CsSesionActiva.CreadorID, dataGridView1, "spObtenerPagosUsuario");
                    ActualizarBotonesAccion();
                }
            }
        }
    }
}
