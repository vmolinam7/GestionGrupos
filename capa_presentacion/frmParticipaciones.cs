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

namespace capa_presentacion
{

    public partial class frmParticipaciones: Form
    {
        csDetalleGrupo objDetalleGrupo = new csDetalleGrupo();
        public frmParticipaciones()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmParticipaciones_Load(object sender, EventArgs e)
        {
            string sp = "spListarGruposUsuario";
            objDetalleGrupo.cargarValoresTabla(CsSesionActiva.CreadorID, dgvListaGrupos, sp);
            dgvListaGrupos.Columns[0].Visible = false;
            csEstilosDgv.AplicarEstilos(dgvListaGrupos);
            dgvListaGrupos.ClearSelection();
            csEstilosDgv.AplicarEstilos(dgvListaDetalle);
            dgvListaDetalle.ClearSelection();
        }

        private void dgvListaGrupos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int idGrupo = Convert.ToInt32(dgvListaGrupos.Rows[e.RowIndex].Cells["GRUPOID"].Value);
                detalledgv(idGrupo);
            }
        }

        private void detalledgv(int id)
        {
            string sp = "spListarDetalleGrupo";
            objDetalleGrupo.cargarValoresTabla(id, dgvListaDetalle, sp);
        }
    }
}
