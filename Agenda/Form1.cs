using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Agenda
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Leer();
        }
        private void Leer() {
            try
            {
                string cadenaConexion = "Server =localhost\\SQLEXPRESS; Database=db_agenda; Trusted_Connection=True ";

                SqlConnection obj_conexion = new SqlConnection(cadenaConexion);

                //abrir
                obj_conexion.Open();
                //consulta sql
                string query = "select * from contactos";

                //objeto de tipo
                SqlCommand obj_cmd = new SqlCommand(query, obj_conexion);

               

                //lector leer la base de datos
                SqlDataReader lector;

                obj_cmd.CommandText = query;

                //ejecutar lectura
                lector= obj_cmd.ExecuteReader();

                //datos en memoria
                DataTable tabla = new DataTable();

                tabla.Load(lector);

                //control del formulario
                dgvContactos.DataSource= tabla;

                obj_conexion.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                
            }

            

        }
    }
}
