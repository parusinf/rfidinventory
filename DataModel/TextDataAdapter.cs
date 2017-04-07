// File: TextDataAdapter.cs
// Program: RFIDInventory
// Author: Pavel Nikitin © 2013
// Version 1.2

using System;
using System.Text;
using System.Data;
using System.IO;
using ru.nikitin.RFIDInventory;

namespace ru.nikitin.RFIDInventory.DataModel
{
    public class TextDataAdapter : IDisposable
    {
        public const string TAB = ".tbl";
        public const int UINT_SIZE = 8;
        public const int ULONG_SIZE = 12;
        public const int DATETIME_SIZE = 19; // dd.MM.yyyy HH:mm:ss
        public const int BOOLEAN_SIZE = 1;

        private string m_path;
        private FileStream m_file;
        private DataTable m_table;
        private int m_recordSize = 2; // record size with \r\n
        private bool m_loadData = false;
        private int m_recordCount = -1;
        private DateTime m_fileDateTime;

        // DataTable assosiated with Adapter
        public DataTable Table
        {
            get
            {
                return m_table;
            }
        }

        // DataTable assosiated with Adapter
        public int Count
        {
            get
            {
                return m_recordCount;
            }
        }

        // Returns data file date and time creation
        public DateTime FileDateTime
        {
            get
            {
                return m_fileDateTime;
            }
        }

        // DataAdapter based on fixed length of row text file
        public TextDataAdapter(string path, DataTable table)
        {
            m_path = path + table.TableName;
            m_table = table;

            // calculate record size in bytes
            foreach (DataColumn column in m_table.Columns)
            {
                if (column.DataType == typeof(UInt64))
                    m_recordSize += ULONG_SIZE;

                else if (column.DataType == typeof(UInt32))
                    m_recordSize += UINT_SIZE;
                
                else if (column.DataType == typeof(DateTime))
                    m_recordSize += DATETIME_SIZE;
                
                else if (column.DataType == typeof(Boolean))
                    m_recordSize += BOOLEAN_SIZE;
                
                else
                    m_recordSize += column.MaxLength;
            }

            // open table file stream
            m_file = File.Open(m_path + TAB, FileMode.OpenOrCreate, 
                FileAccess.Read, FileShare.Read);
            m_recordCount = (int)(m_file.Length / (int)m_recordSize);

            FileInfo fileInfo = new FileInfo(m_path + TAB);
            m_fileDateTime = fileInfo.CreationTime;

            // add a RowChanged event handler for the table
            m_table.RowChanged += new DataRowChangeEventHandler(RowChanged);
        }

        // Load all records from file to DataTable
        public int Fill(System.Windows.Forms.ProgressBar progressBar, NordicKeyLightWave animation)
        {
            m_loadData = true;
            byte[] buffer = new byte[m_recordSize];
            m_file.Seek(0, SeekOrigin.Begin);

            // read table data from file stream
            while (m_file.Read(buffer, 0, (int)m_recordSize) > 0)
            {
                DataRow row = ParseDataRow(buffer);
                m_table.Rows.Add(row);
                
                if (progressBar != null)
                    progressBar.Value++;

                if (animation != null && progressBar.Value % 10 == 0)
                    animation.NextStep(this, null);
            }

            m_table.AcceptChanges();
            m_loadData = false;

            return m_table.Rows.Count;
        }

        // Select record by primary key from file
        public DataRow Select(ulong key)
        {
            DataRow[] rows = m_table.Select(m_table.PrimaryKey[0].ColumnName 
                + "=" + key.ToString());
            
            if (rows.Length == 1)
                return rows[0];
            else
                return null;
        }
        
        // Dispose file stream
        public void Dispose()
        {
            if (m_file != null)
                m_file.Dispose();
        }

        // Open table file for write
        private void OpenWriteStream()
        {
            m_file.Close();
            m_file = File.Open(m_path + TAB, FileMode.Append, 
                FileAccess.Write, FileShare.None);
        }

        // Open table file for read
        private void OpenReadStream()
        {
            m_file.Close();
            m_file = File.Open(m_path + TAB, FileMode.Open, 
                FileAccess.Read, FileShare.Read);
        }

        // Parse byte buffer, construct DataRow
        private DataRow ParseDataRow(byte[] buffer)
        {
            DataRow row = m_table.NewRow();
            int offset = 0;

            foreach (DataColumn column in m_table.Columns)
            {
                string value = "";

                try
                {
                    if (column.DataType == typeof(UInt64))
                    {
                        value = Encoding.ASCII.GetString(
                            buffer, offset, ULONG_SIZE);
                        row[column] = UInt64.Parse(value);
                        offset += ULONG_SIZE;
                    }
                    else if (column.DataType == typeof(UInt32))
                    {
                        value = Encoding.ASCII.GetString(
                            buffer, offset, UINT_SIZE);
                        row[column] = UInt32.Parse(value);
                        offset += UINT_SIZE;
                    }
                    else if (column.DataType == typeof(DateTime))
                    {
                        value = Encoding.ASCII.GetString(
                            buffer, offset, DATETIME_SIZE);
                        row[column] = DateTime.ParseExact(value,
                            "dd.MM.yyyy HH:mm:ss", 
                            new System.Globalization.DateTimeFormatInfo());
                        offset += DATETIME_SIZE;
                    }
                    else if (column.DataType == typeof(Boolean))
                    {
                        row[column] = (char)buffer[offset] == '1' ? true : false;
                        offset += BOOLEAN_SIZE;
                    }
                    else
                    {
                        row[column] = Tools.GetString( // Encoding.GetEncoding(1251).GetString
                            buffer, offset, column.MaxLength).TrimEnd();
                        offset += column.MaxLength;
                    }
                }
                catch (FormatException)
                {
                    throw new FormatException(FormatExceptionMessage(
                        m_table.TableName, column.ColumnName, value));
                }
            }

            return row;
        }

        // Format exception message
        private string FormatExceptionMessage(
            string table, string column, string value)
        {
            return "FormatException: Table: " +
                    table + "; Column: " +
                    column + "; Value: " +
                    value;
        }

        // Write data row to table file
        private void WriteDataRow(FileStream stream, DataRow row)
        {
            byte[] buffer;

            foreach (DataColumn column in row.Table.Columns)
            {
                if (column.DataType == typeof(UInt64))
                {
                    ulong id = (ulong)row[column];
                    buffer = Encoding.ASCII.GetBytes(
                        id.ToString().PadLeft(ULONG_SIZE, '0'));
                }
                else if (column.DataType == typeof(UInt32))
                {
                    uint id = (uint)row[column];
                    buffer = Encoding.ASCII.GetBytes(
                        id.ToString().PadLeft(UINT_SIZE, '0'));
                }
                else if (column.DataType == typeof(DateTime))
                {
                    buffer = Encoding.ASCII.GetBytes(
                        ((DateTime)row[column]).ToString("dd.MM.yyyy HH:mm:ss"));
                }
                else if (column.DataType == typeof(Boolean))
                {
                    buffer = Encoding.ASCII.GetBytes(
                        (Boolean)row[column] ? "1" : "0");
                }
                else
                    buffer = Tools.GetBytes( // Encoding.GetEncoding(1251).GetBytes
                        ((string)row[column]).PadRight(column.MaxLength));

                stream.Write(buffer, 0, buffer.Length);
            }

            byte[] endl = { 13, 10 };
            stream.Write(endl, 0, endl.Length);
        }

        // Write record to table file
        private void RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (m_loadData)
                return;

            DataTable table = (DataTable)sender;
            
            if (table.PrimaryKey[0].ReadOnly)
                throw new NotSupportedException("Table " + 
                    table.TableName + " is read only.");

            // DataRow object added
            if (e.Action == DataRowAction.Add)
            {
                OpenWriteStream();
                WriteDataRow(m_file, e.Row);
                OpenReadStream();
                table.AcceptChanges();
            }
        }
    }
}
