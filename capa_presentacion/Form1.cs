using capa_datos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Drawing.Printing;
using capa_negocios;
using Microsoft.Reporting.Map.WebForms.BingMaps;

namespace capa_presentacion
{
    public partial class Form1: Form
    {
        csConexion csConexion;
        private DataTable dt;
        private int rowIndex = 0;
        private Image logo;
        public Form1()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        public void ocultar()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            lblNombreUser.Visible = false;
        }

        public void mostrar()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            lblNombreUser.Visible = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //CsSesionActiva.CreadorID = 1;
            panel2.BackColor = Color.FromArgb(100, 0, 0, 0);
            fmrLogin frmlogin = new fmrLogin();
            frmlogin.MdiParent = this;
            abrifrm(frmlogin);
            lblNombreUser.Text = lblNombreUser.Text.ToUpper();
            logo = Properties.Resources.personas;
            ocultar();
        }

        public void modificar_nombre_user()
        {
            lblNombreUser.Text = CsSesionActiva.User;
            lblNombreUser.Text = lblNombreUser.Text.ToUpper();
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

        public void recuperarcontra()
        {
            frmRecuperarContra frmrecu = new frmRecuperarContra();
            frmrecu.MdiParent = this;
            abrifrm(frmrecu);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            csConexion = new csConexion();
            if (csConexion.ValidarUsuario(textBox1.Text.ToString(), textBox3.Text.ToString()))
            {
                MessageBox.Show("Acceso correcto");
            }
            else
            {
                MessageBox.Show("Credenciales incorrectas");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmgrupos frmgrup = new frmgrupos();
            frmgrup.MdiParent = this;
            abrifrm(frmgrup);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmParticipaciones frmpart = new frmParticipaciones();
            frmpart.MdiParent = this;
            abrifrm(frmpart);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmPagos frmpa = new frmPagos();
            frmpa.MdiParent = this;
            abrifrm(frmpa);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int id = CsSesionActiva.CreadorID; 
            CsGrupos csGrupos = new CsGrupos();
            dt = csGrupos.ObtenerDatosGrupo(id);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para este grupo.");
                return;
            }
            PrintPreviewDialog printPreview = new PrintPreviewDialog();
            PrintDocument printDocument = new PrintDocument();

            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
            printPreview.Document = printDocument;

            printPreview.ShowDialog();
        }
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Configuración de márgenes y dimensiones - usando márgenes equilibrados
            int marginLeft = 40;
            int marginRight = 40;
            int marginTop = 50;
            int y = marginTop;
            int pageWidth = e.PageBounds.Width - (marginLeft + marginRight);
            Font regularFont = new Font("Arial", 10);
            Font titleFont = new Font("Arial", 16, FontStyle.Bold);
            Font headerFont = new Font("Arial", 10, FontStyle.Bold);  // Reducido ligeramente el tamaño
            StringFormat stringFormat = new StringFormat()
            {
                Trimming = StringTrimming.EllipsisCharacter,
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            // Preparar medidas de celdas según el número de columnas
            int numColumns = dt.Columns.Count;
            int[] columnWidths = new int[numColumns];

            // Definir anchos relativos para cada columna (porcentajes del ancho disponible)
            // Esto nos ayudará a distribuir el espacio de manera más proporcional
            Dictionary<string, float> columnWidthPercentages = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase)
    {
        { "GRUPOID", 0.08f },        // 8% del ancho total
        { "NOMBRE_GRUPO", 0.15f },   // 15% del ancho total
        { "DESCRIPCIÓN", 0.22f },    // 22% del ancho total
        { "FECHACREACION", 0.15f },  // 15% del ancho total
        { "ESTADO", 0.10f },         // 10% del ancho total
        { "VALORTOTAL", 0.10f },     // 10% del ancho total
        { "CREADOR", 0.20f }         // 20% del ancho total
    };

            // Primer paso: asignar anchos basados en porcentajes
            for (int i = 0; i < numColumns; i++)
            {
                string columnName = dt.Columns[i].ColumnName;
                if (columnWidthPercentages.ContainsKey(columnName))
                {
                    columnWidths[i] = (int)(pageWidth * columnWidthPercentages[columnName]);
                }
                else
                {
                    // Si no está definido, asignar un porcentaje predeterminado
                    columnWidths[i] = (int)(pageWidth * 0.14f); // ~14% como valor predeterminado
                }
            }

            // Segundo paso: asegurarse de que el ancho total coincida con el ancho de página disponible
            int totalCalculatedWidth = columnWidths.Sum();
            if (totalCalculatedWidth != pageWidth)
            {
                // Ajustar la última columna para compensar cualquier diferencia
                columnWidths[numColumns - 1] += (pageWidth - totalCalculatedWidth);
            }

            // Dibujar el logo y el título en la misma fila
            if (logo != null)
            {
                int logoHeight = 70;
                int logoWidth = 70;

                // Dibujar logo a la izquierda
                e.Graphics.DrawImage(logo, marginLeft, y, logoWidth, logoHeight);

                // Dibujar título centrado verticalmente junto al logo
                e.Graphics.DrawString("Reporte de Grupos", titleFont, Brushes.Black,
                    marginLeft + logoWidth + 20, y + (logoHeight / 2) - (titleFont.Height / 2));

                y += logoHeight + 20; // Espacio después del logo y título
            }
            else
            {
                // Solo título si no hay logo
                e.Graphics.DrawString("Reporte de Grupos", titleFont, Brushes.Black, marginLeft, y);
                y += titleFont.Height + 20;
            }

            // Dibujar fecha y hora del reporte
            string dateTime = "Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            e.Graphics.DrawString(dateTime, regularFont, Brushes.Black, marginLeft, y);
            y += regularFont.Height + 15;

            // Dibujar encabezados de columnas
            int x = marginLeft;
            int headerHeight = 35; // Altura razonable para las cabeceras

            for (int i = 0; i < numColumns; i++)
            {
                Rectangle headerRect = new Rectangle(x, y, columnWidths[i], headerHeight);

                // Dibujar fondo y borde
                e.Graphics.FillRectangle(Brushes.LightGray, headerRect);
                e.Graphics.DrawRectangle(Pens.Black, headerRect);

                // Obtener el texto de la cabecera y aplicar formato si es necesario
                string headerText = dt.Columns[i].ColumnName;

                // Para FECHACREACION, dividir en dos líneas para mejor presentación
                if (headerText.Equals("FECHACREACION", StringComparison.OrdinalIgnoreCase))
                {
                    headerText = "FECHA-\r\nCREACION";
                }
                else if (headerText.Equals("VALORTOTAL", StringComparison.OrdinalIgnoreCase))
                {
                    headerText = "VALOR\r\nTOTAL";
                }

                // Formato para permitir múltiples líneas en cabeceras
                StringFormat headerFormat = new StringFormat()
                {
                    Trimming = StringTrimming.EllipsisCharacter,
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                    FormatFlags = StringFormatFlags.NoClip
                };

                e.Graphics.DrawString(headerText, headerFont, Brushes.Black,
                    new RectangleF(headerRect.X + 2, headerRect.Y + 2, headerRect.Width - 4, headerRect.Height - 4),
                    headerFormat);

                x += columnWidths[i];
            }

            y += headerHeight;

            // Imprimir filas de datos
            int rowHeight = 25; // Reducir ligeramente la altura predeterminada
            for (; rowIndex < dt.Rows.Count; rowIndex++)
            {
                // Verificar si queda espacio en la página
                if (y + rowHeight > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }

                x = marginLeft;

                // Calcular altura de fila basada en contenido
                int maxCellHeight = rowHeight;
                for (int i = 0; i < numColumns; i++)
                {
                    string cellText = dt.Rows[rowIndex][i].ToString();
                    SizeF textSize = e.Graphics.MeasureString(cellText, regularFont, columnWidths[i] - 10);
                    int requiredHeight = (int)textSize.Height + 6; // Reducir padding vertical
                    maxCellHeight = Math.Max(maxCellHeight, requiredHeight);
                }

                // Dibujar celdas con altura adaptativa
                for (int i = 0; i < numColumns; i++)
                {
                    string cellText = dt.Rows[rowIndex][i].ToString();
                    Rectangle cellRect = new Rectangle(x, y, columnWidths[i], maxCellHeight);

                    // Dibujar borde
                    e.Graphics.DrawRectangle(Pens.Black, cellRect);

                    // Crear formato para multilinea si es necesario
                    StringFormat cellFormat = new StringFormat()
                    {
                        Trimming = StringTrimming.EllipsisCharacter,
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    // Dibujar texto
                    e.Graphics.DrawString(cellText, regularFont, Brushes.Black,
                        new RectangleF(x + 5, y + 2, columnWidths[i] - 10, maxCellHeight - 4),
                        cellFormat);

                    x += columnWidths[i];
                }

                y += maxCellHeight;
            }

            // Agregar pie de página con numeración
            string pageFooter = "Página " + (pageNumber + 1);
            SizeF footerSize = e.Graphics.MeasureString(pageFooter, regularFont);
            e.Graphics.DrawString(pageFooter, regularFont, Brushes.Black,
                e.PageBounds.Width - marginRight - footerSize.Width,
                e.PageBounds.Height - marginTop - footerSize.Height);

            if (rowIndex >= dt.Rows.Count)
            {
                e.HasMorePages = false;
                rowIndex = 0;    // Reiniciar para futuras impresiones
                pageNumber = 0;  // Reiniciar contador de páginas
            }
            else
            {
                e.HasMorePages = true;
                pageNumber++;    // Incrementar contador de páginas
            }
        }

        // Variable para seguimiento de páginas
        private int pageNumber = 0;

        private void button6_Click(object sender, EventArgs e)
        {
            fmrLogin frmlogin = new fmrLogin();
            frmlogin.MdiParent = this;
            abrifrm(frmlogin);
            ocultar();
        }
    }
}
