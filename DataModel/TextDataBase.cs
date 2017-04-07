// File: TextDataBase.cs
// Program: RFIDInventory
// Author: Pavel Nikitin © 2013
// Version 1.2

using System;
using System.Collections.Generic;
using System.IO;
using System.Data;

namespace ru.nikitin.RFIDInventory.DataModel
{

    public class TextDataBase : IDisposable
    {
        public const string SCHEMA_XML = "Schema.xml";
        private DataSet m_dataSet = new DataSet();
        private Dictionary<DataTable, TextDataAdapter> m_adapters;
        
        // DataSet from Schema.xml
        public DataSet DataSet 
        { 
            get 
            { 
                return m_dataSet; 
            } 
        }

        // Get TextDataAdapter by table name
        public TextDataAdapter this[string tableName]
        {
            get
            {
                return m_adapters[m_dataSet.Tables[tableName]];
            }
        }

        // Database from directory
        public TextDataBase(string path)
        {
            // parse Schema.xml
            if (!File.Exists(path + SCHEMA_XML))
                throw new IOException("File " + SCHEMA_XML + " not found");

            // construct DataSet
            m_dataSet.ReadXmlSchema(path + SCHEMA_XML);

            // construct Dictionary of TextDataAdapter
            m_adapters = new Dictionary<DataTable, TextDataAdapter>(m_dataSet.Tables.Count);

            foreach (DataTable table in m_dataSet.Tables)
                m_adapters[table] = new TextDataAdapter(path, table);
        }

        // Dispose data adapters
        public void Dispose()
        {
            foreach (DataTable table in m_dataSet.Tables)
                if (m_adapters.ContainsKey(table))
                    m_adapters[table].Dispose();
        }
    }
}
