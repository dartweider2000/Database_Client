using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DataBase_Client
{
    public partial class Form1 : Form
    {
        private SqlConnection _connection;
        private List<string> tables;
        private string activeTable; 
        public Form1()
        {
            InitializeComponent();

            this.InitTables();
            this.FillSelectTables();
            this.CreateConnection();

            dataGridView1.AutoGenerateColumns = true;
            bindingSource1.ResetBindings(false);
        }

        public void InitTables()
        {
            this.tables = new List<string>()
            {
                Table.Pasport, Table.Reapir, Table.Producer, Table.ReapirType, 
                Table.Place, Table.Type, Table.Property, Table.PasportProperty
            };

            this.activeTable = this.tables[0];
        }

        public void FillSelectTables()
        {
            this.selectTables.DropDownStyle = ComboBoxStyle.DropDownList;
            this.selectTables.DataSource = this.tables;
        }

        public  void CreateConnection()
        {
            const string connectStr = @"Data Source=.\SQLEXPRESS01; Initial Catalog=Enterprise; Integrated Security=True";

            this._connection = new SqlConnection(connectStr);
        }

        public void OpenDatabase()
        {         
            _connection.Open();     
        }

        public void CloseDatabase()
        {
            this._connection.Close();
        }

        public SqlDataReader GetReader(string sqlCommand)
        {
            return new SqlCommand(sqlCommand, this._connection).ExecuteReader();
        }

        public List<ITableData> GetReferenceList()
        {
            string sqlCommand = $@"SELECT [name] FROM {this.activeTable}";
            SqlDataReader reader = this.GetReader(sqlCommand);

            List<ITableData> referenceList = new List<ITableData>();

            while (reader.Read())
            {
                Reference reference = new Reference(reader.GetString(0));

                referenceList.Add(reference);
            }

            return referenceList;
        }

        public List<ITableData> GetRepairList()
        {
            const string sqlCommand = @"
                    SELECT pasport_number, rp.[name], [date]
                    FROM Repair r JOIN Repair_type rp ON r.repair_type_id = rp.repair_type_id
                    ORDER BY [date] DESC
            ";
            SqlDataReader reader = this.GetReader(sqlCommand);

            List<ITableData> repairList = new List<ITableData>();

            while (reader.Read())
            {
                Repair repair = new Repair(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetDateTime(2)
                );

                repairList.Add(repair);
            }

            return repairList;
        }

        public List<ITableData> GetPasportList()
        {
            const string sqlCommand = @"
                SELECT pasport_number, pas.[name], prod.[name], t.[name], p.[name], birthday
                FROM Pasport pas JOIN producer prod ON pas.producer_id = prod.producer_id
                JOIN [type] t ON pas.[type_id] = t.[type_id]
                JOIN place p ON pas.place_id = p.place_id
                ORDER BY birthday DESC
            ";
            SqlDataReader reader = this.GetReader(sqlCommand);

            List<ITableData> pasportList = new List<ITableData>();

            while (reader.Read())
            {
                Pasport pasport = new Pasport(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetDateTime(5)
                );

                pasportList.Add(pasport);
            }

            return pasportList;
        }

        public List<ITableData> GetPasportPropertyList()
        {
            const string sqlCommand = @"
                SELECT pasport_number, prop.[name] FROM Pasport_Property pp
                JOIN Property prop ON pp.property_id = prop.property_id
            ";
            SqlDataReader reader = this.GetReader(sqlCommand);

            List<ITableData> pasportPropertyList = new List<ITableData>();

            while (reader.Read())
            {
                PasportProperty pasport = new PasportProperty(
                    reader.GetString(0),
                    reader.GetString(1)                  
                );

                pasportPropertyList.Add(pasport);
            }

            return pasportPropertyList;
        }

        public void Output<T>() where T : class, ITableData
        {
            List<T> performList = new List<T>();

            foreach (var data in this.GetList())
                performList.Add(data as T);

            bindingSource1.DataSource = performList;
        }

        public List<ITableData> GetList()
        {
            if (this.activeTable == Table.Pasport)
                return this.GetPasportList();
            else if (this.activeTable == Table.Reapir)
                return this.GetRepairList();
            else if (this.activeTable == Table.Producer || this.activeTable == Table.Place ||
                this.activeTable == Table.Type || this.activeTable == Table.Property ||
                this.activeTable == Table.ReapirType)
                return this.GetReferenceList();
            else if (this.activeTable == Table.PasportProperty)
                return this.GetPasportPropertyList();

            return null;
        }

        public List<Pasport> MapToPasport(List<ITableData> list)
        {
            List<Pasport> pasportList = new List<Pasport>();

            foreach (var data in list)
                pasportList.Add(data as Pasport);

            return pasportList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.OpenDatabase();

            if (this.activeTable == Table.Pasport)
                this.Output<Pasport>();
            else if (this.activeTable == Table.Reapir)
                this.Output<Repair>();
            else if (this.activeTable == Table.Producer || this.activeTable == Table.Place ||
                this.activeTable == Table.Type || this.activeTable == Table.Property ||
                this.activeTable == Table.ReapirType)
                this.Output<Reference>();
            else if (this.activeTable == Table.PasportProperty)
                this.Output<PasportProperty>();


            this.CloseDatabase();
        }

        private void selectTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.activeTable = this.tables[this.selectTables.SelectedIndex];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.OpenDatabase();

            const string sqlCommand = @"
                SELECT prod.[name] AS [Производитель], 
	            COUNT(*) AS [Кол-во плановых ремонтов оборудования данного производителя (болше 2-х)] 
                FROM producer prod 
	            JOIN pasport pas ON pas.producer_id = prod.producer_id
	            JOIN Repair r ON r.pasport_number = pas.pasport_number
                WHERE r.repair_type_id = (SELECT repair_type_id FROM Repair_type WHERE [name] = 'Плановый')
                GROUP BY prod.producer_id, prod.[name]
                HAVING COUNT(*) > 2
                ORDER BY COUNT(*) DESC;
            ";
            SqlDataReader reader = this.GetReader(sqlCommand);

            List<FirstQuery> firstQueryList = new List<FirstQuery>();

            while (reader.Read())
            {
                FirstQuery pasport = new FirstQuery(
                    reader.GetString(0),
                    reader.GetInt32(1)
                );

                firstQueryList.Add(pasport);
            }

            bindingSource1.DataSource = firstQueryList;


            this.CloseDatabase();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.OpenDatabase();

            const string sqlCommand = @"
                SELECT pas.pasport_number AS [Одновременно холодоучтойчивое и жароустойчивое оборудование, произведённое начиня с 2015-го года], 
	            COUNT(DISTINCT rep.rapair_id) AS [Кол-во ремонтов], 
	            MIN(rep.[date]) AS [Первый ремонт], MAX(rep.[date]) AS [Последний ремонт] 
                FROM Repair rep 
	            JOIN Pasport pas ON rep.pasport_number = pas.pasport_number
	            JOIN Pasport_Property pp ON pas.pasport_number = pp.pasport_number  
                WHERE pp.property_id IN (SELECT property_id FROM Property WHERE [name] IN ('Холодоустойчивый', 'Жароустойчивый')) AND
	            pas.[birthday] >= '20150101'
                GROUP BY pas.pasport_number, pas.[name]
                ORDER BY COUNT(DISTINCT rep.rapair_id) DESC;
            ";
            SqlDataReader reader = this.GetReader(sqlCommand);

            List<SecondQuery> secondQueryList = new List<SecondQuery>();

            while (reader.Read())
            {
                SecondQuery pasport = new SecondQuery(
                    reader.GetString(0),
                    reader.GetInt32(1),
                    reader.GetDateTime(2),
                    reader.GetDateTime(3)
                );

                secondQueryList.Add(pasport);
            }

            bindingSource1.DataSource = secondQueryList;


            this.CloseDatabase();
        }
    }
}
