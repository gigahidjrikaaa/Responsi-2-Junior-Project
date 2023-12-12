using System.Data;
using System.Windows.Forms;
using Npgsql;
using Responsi_2_Junior_Project;

namespace Responsi_2_Junior_Project
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            StartPosition = FormStartPosition.CenterScreen;

            InitializeComponent();
        }
        private NpgsqlConnection conn;
        private NpgsqlCommand cmd;
        private string sql;
        private DataGridViewRow selectedRow;
        private string host = "localhost";
        private string port = "5433";
        private string user = "postgres";
        private string password = "postgres";
        private string database = "Responsi";
        public string connectionString()
        {
            return "Host=" + host + "; Port=" + port + "; Username=" + user + "; Password=" + password + "; Database=" + database;
        }
        public void MainMenu_Load(object sender, EventArgs e)
        {

            conn = new NpgsqlConnection(connectionString());
            try
            {
                conn.Open();
                MessageBox.Show("Koneksi Berhasil");
                conn.Close();
                btnLoad_Click(sender, e);
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }
        private int departementID()
        {
            if (comboDep.Text == "HR")
            {
                return 1;
            }
            else if (comboDep.Text == "ENG")
            {
                return 2;
            }
            else if (comboDep.Text == "DEV")
            {
                return 3;
            }
            else if (comboDep.Text == "PM")
            {
                return 4;
            }
            else if (comboDep.Text == "FIN")
            {
                return 5;
            }
            else
            {
                return -1;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectionString());
            try
            {
                conn.Open();
                Karyawan karyawan = new Karyawan(txtNama.Text, comboDep.Text, departementID());
                // Example SQL QUERY
                // INSERT INTO public.Karyawan("nama", "id_dep") VALUES ('Rizky', '1')
                NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO public.Karyawan(\"nama\", \"id_dep\") VALUES ('" + karyawan.Nama + "', '" + karyawan.Id_departemen + "')", conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Berhasil Ditambahkan");
                conn.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
            btnLoad_Click(sender, e);
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectionString());
            try
            {
                try
                {
                    selectedRow = dataGridView1.SelectedRows[0];
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    MessageBox.Show("Pilih data yang ingin diubah");
                    return;
                }

                conn.Open();
                Karyawan karyawan = new Karyawan(txtNama.Text, comboDep.Text, departementID());
                // Example SQL QUERY
                // UPDATE INTO public.Karyawan("nama", "id_dep") VALUES ('Nathan', '1')
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE public.Karyawan SET \"nama\" = '" + karyawan.Nama + "', \"id_dep\" = '" + karyawan.Id_departemen + "' WHERE \"id_karyawan\" = '" + selectedRow.Cells[0].Value.ToString() + "'", conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Berhasil Diubah");
                conn.Close();

            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
            btnLoad_Click(sender, e);
        }

        private void txtNama_TextChanged(object sender, EventArgs e)
        {
            // check if the text box is empty
            if (txtNama.Text == "")
            {
                btnInsert.Enabled = false;
            }
            else
            {
                btnInsert.Enabled = true;
            }
        }

        private void comboDep_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show(comboDep.Text + " Selected!");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                selectedRow = dataGridView1.SelectedRows[0];
            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Pilih data yang ingin dihapus");
                return;
            }

            conn = new NpgsqlConnection(connectionString());
            try
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM public.\"Departemen\" WHERE \"ID_Departemen\" = '" + selectedRow.Cells[0].Value.ToString() + "'", conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Berhasil Dihapus");
                conn.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
            btnLoad_Click(sender, e);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connectionString());
            try
            {
                conn.Open();
                sql = "SELECT * FROM public.karyawan";
                cmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                conn.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }
    }
}