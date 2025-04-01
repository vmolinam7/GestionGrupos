using capa_datos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using capa_datos;

namespace capa_negocios
{
    public class CsGrupos
    {
        csConexion csConexion=new csConexion();
        public virtual bool CrearGrupo(string nombreGrupo, string descripcionGrupo, decimal valorTotal, int creadorID, List<int> participantes)
        {
            string sp = "spCrearGrupoCompleto";

            try
            {
                decimal porcentajeIndividual = 100.0m / participantes.Count;

                csConexion.abrirconexcion();

                using (SqlCommand cmd = new SqlCommand(sp, csConexion.con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NombreGrupo", nombreGrupo);
                    cmd.Parameters.AddWithValue("@DescripcionGrupo", descripcionGrupo);
                    cmd.Parameters.AddWithValue("@ValorTotal", valorTotal);
                    cmd.Parameters.AddWithValue("@CreadorID", creadorID);

                    StringBuilder participantesString = new StringBuilder();
                    foreach (int usuarioID in participantes)
                    {
                        if (participantesString.Length > 0)
                            participantesString.Append(",");
                        participantesString.Append(usuarioID);
                    }
                    cmd.Parameters.AddWithValue("@Participantes", participantesString.ToString());

                    decimal montoDeuda = Math.Round(valorTotal / participantes.Count, 2);
                    cmd.Parameters.AddWithValue("@MontoDeuda", montoDeuda);

                    cmd.Parameters.AddWithValue("@PorcentajeIndi", porcentajeIndividual);

                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        int grupoID = Convert.ToInt32(result);
                        return true;
                    }

                    return false; 
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al crear el grupo: " + ex.Message);
                return false;
            }
            finally
            {
                csConexion.cerrarconexion();
            }
        }
    }
}
