using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using capa_datos;

namespace capa_negocios
{
    public class csDetalleGrupo
    {
        csConexion csConexion = new csConexion();
        public void cargarValoresTabla(int idUsuario, System.Windows.Forms.DataGridView dgv, string sp)
        {
            csConexion.abrirconexcion();

            csConexion.cmd = new SqlCommand(sp, csConexion.con);
            csConexion.cmd.CommandType = CommandType.StoredProcedure;
            csConexion.cmd.Parameters.Add(new SqlParameter("@id", idUsuario));

            SqlDataAdapter adapter = new SqlDataAdapter(csConexion.cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            dgv.DataSource = dt;

            csConexion.cerrarconexion();
        }
    }
}
