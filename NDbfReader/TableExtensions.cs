﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace NDbfReader
{
    /// <summary>
    /// Extensions for for the <see cref="Table"/> class.
    /// </summary>
    public static class TableExtensions
    {
        /// <summary>
        /// Loads the DBF table into a <see cref="DataTable"/> with the default UTF-8 encoding.
        /// </summary>
        /// <param name="table">The DBF table to load.</param>
        /// <returns>A <see cref="DataTable"/> loaded from the DBF table.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="table"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Another reader of the DBF table is opened.</exception>
        /// <exception cref="ObjectDisposedException">The DBF table is disposed.</exception>
        public static DataTable AsDataTable(this Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            DataTable dataTable = CreateDataTable(table.Columns);
            FillData(table.Columns, dataTable, table.OpenReader());
            return dataTable;
        }

        /// <summary>
        /// Loads the DBF table into a <see cref="DataTable"/> with the default UTF-8 encoding.
        /// </summary>
        /// <param name="table">The DBF table to load.</param>
        /// <param name="columnNames">The names of columns to load.</param>
        /// <returns>A <see cref="DataTable"/> loaded from the DBF table.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="table"/> is <c>null</c> or one of the column names is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Another reader of the DBF table is opened.</exception>
        /// <exception cref="ObjectDisposedException">The DBF table is disposed.</exception>
        public static DataTable AsDataTable(this Table table, params string[] columnNames)
        {
            return AsDataTable(table, Table.DefaultEncoding, columnNames);
        }

        /// <summary>
        /// Loads the DBF table into a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="table">The DBF table to load.</param>
        /// <param name="encoding">The encoding that is used to load the rows content.</param>
        /// <returns>A <see cref="DataTable"/> loaded from the DBF table.</returns>
        /// <exception cref="InvalidOperationException">Another reader of the DBF table is opened.</exception>
        /// <exception cref="ObjectDisposedException">The DBF table is disposed.</exception>
        public static DataTable AsDataTable(this Table table, Encoding encoding)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            DataTable dataTable = CreateDataTable(table.Columns);
            FillData(table.Columns, dataTable, table.OpenReader(encoding));
            return dataTable;
        }

        /// <summary>
        /// Loads the DBF table into a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="table">The DBF table to load.</param>
        /// <param name="encoding">The encoding that is used to load the rows content.</param>
        /// <param name="columnNames">The names of columns to load.</param>
        /// <returns>A <see cref="DataTable"/> loaded from the DBF table.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="table"/> is <c>null</c> or <paramref name="encoding"/> is <c>null</c> or one of the column names is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Another reader of the DBF table is opened.</exception>
        /// <exception cref="ObjectDisposedException">The DBF table is disposed.</exception>
        public static DataTable AsDataTable(this Table table, Encoding encoding, params string[] columnNames)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }
            if (columnNames.Length == 0)
            {
                throw new ArgumentException("No column names specified. Specify at least one column.", nameof(columnNames));
            }

            List<IColumn> selectedColumns = GetSelectedColumns(table, columnNames);
            DataTable dataTable = CreateDataTable(selectedColumns);
            FillData(selectedColumns, dataTable, table.OpenReader(encoding));
            return dataTable;
        }

        /// <summary>
        /// Loads the DBF table into a <see cref="DataTable"/> with the default UTF-8 encoding.
        /// </summary>
        /// <param name="table">The DBF table to load.</param>
        /// <returns>A <see cref="DataTable"/> loaded from the DBF table.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="table"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Another reader of the DBF table is opened.</exception>
        /// <exception cref="ObjectDisposedException">The DBF table is disposed.</exception>
        public static async Task<DataTable> AsDataTableAsync(this Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            DataTable dataTable = CreateDataTable(table.Columns);
            await FillDataAsync(table.Columns, dataTable, table.OpenReader()).ConfigureAwait(false);
            return dataTable;
        }

        /// <summary>
        /// Loads the DBF table into a <see cref="DataTable"/> with the default UTF-8 encoding.
        /// </summary>
        /// <param name="table">The DBF table to load.</param>
        /// <param name="columnNames">The names of columns to load.</param>
        /// <returns>A <see cref="DataTable"/> loaded from the DBF table.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="table"/> is <c>null</c> or one of the column names is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Another reader of the DBF table is opened.</exception>
        /// <exception cref="ObjectDisposedException">The DBF table is disposed.</exception>
        public static Task<DataTable> AsDataTableAsync(this Table table, params string[] columnNames)
        {
            return AsDataTableAsync(table, Table.DefaultEncoding, columnNames);
        }

        /// <summary>
        /// Loads the DBF table into a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="table">The DBF table to load.</param>
        /// <param name="encoding">The encoding that is used to load the rows content.</param>
        /// <returns>A <see cref="DataTable"/> loaded from the DBF table.</returns>
        /// <exception cref="InvalidOperationException">Another reader of the DBF table is opened.</exception>
        /// <exception cref="ObjectDisposedException">The DBF table is disposed.</exception>
        public static async Task<DataTable> AsDataTableAsync(this Table table, Encoding encoding)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            DataTable dataTable = CreateDataTable(table.Columns);
            await FillDataAsync(table.Columns, dataTable, table.OpenReader(encoding)).ConfigureAwait(false);
            return dataTable;
        }

        /// <summary>
        /// Loads the DBF table into a <see cref="DataTable"/>.
        /// </summary>
        /// <param name="table">The DBF table to load.</param>
        /// <param name="encoding">The encoding that is used to load the rows content.</param>
        /// <param name="columnNames">The names of columns to load.</param>
        /// <returns>A <see cref="DataTable"/> loaded from the DBF table.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="table"/> is <c>null</c> or <paramref name="encoding"/> is <c>null</c> or one of the column names is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Another reader of the DBF table is opened.</exception>
        /// <exception cref="ObjectDisposedException">The DBF table is disposed.</exception>
        public static async Task<DataTable> AsDataTableAsync(this Table table, Encoding encoding, params string[] columnNames)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }
            if (columnNames.Length == 0)
            {
                throw new ArgumentException("No column names specified. Specify at least one column.", nameof(columnNames));
            }

            List<IColumn> selectedColumns = GetSelectedColumns(table, columnNames);
            DataTable dataTable = CreateDataTable(selectedColumns);
            await FillDataAsync(selectedColumns, dataTable, table.OpenReader(encoding)).ConfigureAwait(false);
            return dataTable;
        }

        private static DataTable CreateDataTable(IEnumerable<IColumn> columns)
        {
            var dataTable = new DataTable
            {
                Locale = CultureInfo.CurrentCulture
            };

            foreach (IColumn column in columns)
            {
                Type columnType = Nullable.GetUnderlyingType(column.Type) ?? column.Type;
                dataTable.Columns.Add(column.Name, columnType);
            }

            return dataTable;
        }

        private static void FillData(IEnumerable<IColumn> columns, DataTable dataTable, Reader reader)
        {
            while (reader.Read())
            {
                dataTable.Rows.Add(LoadRow(dataTable, columns, reader));
            }
        }

        private static async Task FillDataAsync(IEnumerable<IColumn> columns, DataTable dataTable, Reader reader)
        {
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                dataTable.Rows.Add(LoadRow(dataTable, columns, reader));
            }
        }

        private static List<IColumn> GetSelectedColumns(Table table, string[] columnNames)
        {
            var selectedColumns = new List<IColumn>(columnNames.Length);
            foreach (string columnName in columnNames)
            {
                IColumn column = table.Columns.FindByName(columnName);
                if (column == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(columnNames), columnName, "The table does not have a column with this name.");
                }
                selectedColumns.Add(column);
            }
            return selectedColumns;
        }

        private static DataRow LoadRow(DataTable dataTable, IEnumerable<IColumn> columns, Reader reader)
        {
            DataRow row = dataTable.NewRow();
            foreach (IColumn column in columns)
            {
                row[column.Name] = reader.GetValue(column) ?? DBNull.Value;
            }
            return row;
        }
    }
}