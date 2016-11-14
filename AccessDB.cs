using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace 遗传算法框架
{
    class AccessDB
    {
        protected OleDbConnection conn = new OleDbConnection();
        protected OleDbCommand comm = new OleDbCommand();
        public string DBPath;

        public AccessDB(string strPath)
        {
            //init //打开数据库
            DBPath = strPath;

            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + DBPath;//具体要打开的Access 2007文件及路径由DBPath提供。
                //conn.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + DBPath;//用于Access 2003版本
                comm.Connection = conn;
                try
                {
                    conn.Open();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

            }
        }

        /*            ~AccessDB()  //析构，//关闭数据库
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                            conn.Dispose();
                            comm.Dispose();
                        }
                    }
        */
        public bool FindTable(string tablename)  //判断数据库中是否存在名为tablename的表（区分大小写！）
        {
            int result = 0;
            DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            if (schemaTable != null)
            {

                for (Int32 row = 0; row < schemaTable.Rows.Count; row++)
                {
                    string col_name = schemaTable.Rows[row]["TABLE_NAME"].ToString();
                    if (col_name == tablename)
                    {
                        result++;
                        break;
                    }
                }
            }
            if (result == 0)
                return false;
            return true;
        }

        public bool FindField(string tablename, string fieldname)  //判断tablename表中是否存在名为fieldname的字段（区分大小写！）
        {
            int result = 0;
            DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, tablename, null });

            if (schemaTable != null)
            {
                for (Int32 row = 0; row < schemaTable.Rows.Count; row++)
                {
                    string col_name = schemaTable.Rows[row]["COLUMN_NAME"].ToString();
                    if (col_name == fieldname)
                    {
                        result++;
                        break;
                    }
                }
            }
            if (result == 0)
                return false;
            return true;
        }

        public int executeNonQuery(string sqlstr)  //执行 SQL 语句并返回受影响的行数。对于 UPDATE、INSERT 和 DELETE 语句，返回值为该命令所影响的行数。对于所有其他类型的语句，返回值为 -1。如果发生回滚，返回值也为 -1。
        {
            try
            {
                comm.CommandType = CommandType.Text;
                comm.CommandText = sqlstr;
                return comm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }//执行sql语句

        public string excuteScalar(string sqlstr) //执行查询，并返回查询所返回的结果集中第一行的第一列或 null 引用（如果结果集为空），并将其转换为string型。忽略其他列或行。
        {
            try
            {
                comm.CommandType = CommandType.Text;
                comm.CommandText = sqlstr;
                object obj = comm.ExecuteScalar();
                if (obj != null && obj != DBNull.Value)
                    return obj.ToString();
                else
                    return "";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public OleDbDataReader dataReader(string sqlstr) //返回指定sql语句的OleDbDataReader对象，使用时请注意关闭这个对象（reader.Close();）。
        {
            OleDbDataReader dr = null;
            try
            {
                comm.CommandText = sqlstr;
                comm.CommandType = CommandType.Text;
                dr = comm.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                try
                {
                    dr.Close();
                }
                catch { }
            }
            return dr;
        }

        public DataSet dataSet(string sqlstr)//返回指定sql语句的dataset，使用时请注意关闭这个对象。
        {
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter();
            try
            {
                comm.CommandType = CommandType.Text;
                comm.CommandText = sqlstr;
                da.SelectCommand = comm;
                da.Fill(ds);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return ds;
        }

        public DataSet dataSet(string sqlstr, string tablename)//返回指定sql语句的dataset，使用时请注意关闭这个对象。
        {
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter();
            try
            {
                comm.CommandType = CommandType.Text;
                comm.CommandText = sqlstr;
                da.SelectCommand = comm;
                da.Fill(ds, tablename);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return ds;
        }

        public void UpdateDs(DataSet changedDs, string tablename, OleDbCommand updatecomm) ////数据库数据更新(传DataSet)
        //【注意】changedDs必须是使用dataSet(string sqlstr, string tablename)版创建的！即changedDs要指明tablename！
        {
            OleDbDataAdapter da = new OleDbDataAdapter();
            try
            {
                da.UpdateCommand = updatecomm;
                da.UpdateCommand.Connection = conn;
                da.Update(changedDs, tablename);
                changedDs.AcceptChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
