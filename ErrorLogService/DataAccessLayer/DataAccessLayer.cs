using System;

using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class DataAccessLayer
    {
        /// <summary>
        /// 
        /// </summary>
        public class SqlDataAccess
        {
            private string _ConnectionString = "";

            const int CommandTimeout = 300;

            /// <summary>
            /// 
            /// </summary>
            private SqlConnection Connection { get; set; }
            public string ConfigurationManager { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ProcName"></param>
            /// <returns></returns>
            private SqlDataAdapter GetDataAdapter(string ProcName = "")
            {
                if (!IsConnectionStringValid())
                    throw new Exception("Connection string not set correctly!");

                if (this.Connection == null)
                    this.Connection = new SqlConnection(this._ConnectionString);
                else
                {
                    this.Connection.Dispose();
                    this.Connection = null;
                    this.Connection = new SqlConnection(this._ConnectionString);
                }

                return new SqlDataAdapter(ProcName, this.Connection);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ProcName"></param>
            /// <returns></returns>
            private SqlCommand GetCommand(string ProcName = "")
            {
                if (!IsConnectionStringValid())
                    throw new Exception("Connection string not set correctly!");

                if (this.Connection == null)
                    this.Connection = new SqlConnection(this._ConnectionString);
                else
                {
                    this.Connection.Dispose();
                    this.Connection = null;
                    this.Connection = new SqlConnection(this._ConnectionString);
                }

                return new SqlCommand(ProcName, this.Connection);
            }


            public SqlDataAccess()
            {
                this._ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            }

            private bool IsConnectionStringValid()
            {
                return !String.IsNullOrEmpty(this._ConnectionString);
            }



            /// <summary>
            /// 
            /// </summary>
            /// <param name="ProcName"></param>
            /// <param name="Parameters"></param>
            /// <param name="TableNames"></param>
            /// <param name="ProcReturnType"></param>
            /// <returns></returns>
            public object GetDataFromDatabase(string ProcName, ref DataAccessLayerParameterList Parameters, string[] TableNames = null, ProcReturnType ProcReturnType = ProcReturnType.JSON)
            {
                DataSet TempDataSet = new DataSet();
                SqlDataAdapter _DataAdapter = null;

                int OutputParametersCount = 0; // count Output parameters sent to stored procedure

                try
                {
                    // get new sql data adapter
                    _DataAdapter = GetDataAdapter(ProcName);

                    using (_DataAdapter)
                    {
                        // prepare procedure
                        _DataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        _DataAdapter.SelectCommand.CommandTimeout = CommandTimeout;

                        // add sql parameters to procedure
                        if (Parameters != null)
                        {
                            foreach (DataAccessLayerParameter DalParam in Parameters)
                            {
                                if (DalParam.ParameterDirection == ParameterDirection.Output)
                                    OutputParametersCount++;

                                SqlParameter SqlParam = new SqlParameter()
                                {
                                    ParameterName = DalParam.ParameterName,
                                    SqlDbType = DalParam.ParameterType,
                                    Size = DalParam.ParameterSize,
                                    Direction = DalParam.ParameterDirection,
                                    SqlValue = DalParam.ParameterValue
                                };

                                _DataAdapter.SelectCommand.Parameters.Add(SqlParam);
                            }
                        }

                        // retrieve data into datasets from stored procedure
                        _DataAdapter.Fill(TempDataSet);

                        // handle output parameters
                        if (OutputParametersCount > 0)
                        {
                            foreach (SqlParameter SqlParam in _DataAdapter.SelectCommand.Parameters)
                            {
                                if (SqlParam.Direction == ParameterDirection.Output)
                                {
                                    Parameters.Find((x) => x.ParameterName == SqlParam.ParameterName
                                                        && x.ParameterDirection == ParameterDirection.Output)
                                                        .ParameterValue = SqlParam.Value;
                                }
                            }
                        }//if(iOutputParametersCount > 0)

                        if (TableNames != null)
                        {
                            if (TableNames.Length.Equals(TempDataSet.Tables.Count))
                            {
                                short TableCounter = 0;

                                foreach (DataTable table in TempDataSet.Tables)
                                {
                                    table.TableName = TableNames[TableCounter++];
                                }
                            }
                        }// if (TableNames != null)


                    }//using _DataAdapter

                }
                catch (SqlException SqlEx)
                {

                    throw SqlEx;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (_DataAdapter != null)
                        _DataAdapter.Dispose();

                    if (Connection != null)
                        Connection.Dispose();
                }

                object ReturnValue = null;

                switch (ProcReturnType)
                {
                    case ProcReturnType.DataSet:
                        ReturnValue = TempDataSet;
                        break;
                    case ProcReturnType.JSON:
                        try
                        {
                            ReturnValue = TempDataSet.ToJSON();
                        }
                        catch (Newtonsoft.Json.JsonSerializationException jse) { throw jse; }
                        catch (Newtonsoft.Json.JsonException jex) { throw jex; }
                        catch (Exception ex) { throw ex; }
                        break;
                    case ProcReturnType.XML:
                        try
                        {
                            ReturnValue = TempDataSet.ToXml();
                        }
                        catch (Exception ex) { throw ex; }
                        break;
                }


                return ReturnValue;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="ProcName"></param>
            /// <param name="Parameters"></param>
            /// <returns></returns>
            public int SaveDataToDatabase(string ProcName, ref DataAccessLayerParameterList Parameters)
            {
                int RecordsAffected = -1;
                SqlCommand _Command = null;

                int OutputParametersCount = 0; // count Output parameters sent to stored procedure
                bool HasTransactionBegan = false;

                try
                {
                    // get new sql command
                    _Command = GetCommand(ProcName);

                    using (_Command)
                    {
                        // prepare procedure
                        _Command.CommandType = CommandType.StoredProcedure;
                        _Command.CommandTimeout = CommandTimeout;

                        // add sql parameters to procedure
                        if (Parameters != null)
                        {
                            foreach (DataAccessLayerParameter DalParam in Parameters)
                            {
                                SqlParameter SqlParam = new SqlParameter();
                                if (DalParam.ParameterDirection == ParameterDirection.Output)
                                {
                                    SqlParam.Size = DalParam.ParameterSize;
                                    OutputParametersCount++;
                                }
                                
                                SqlParam.ParameterName = DalParam.ParameterName;
                                SqlParam.SqlDbType = DalParam.ParameterType;
                                SqlParam.Direction = DalParam.ParameterDirection;
                                SqlParam.SqlValue = DalParam.ParameterValue;


                                _Command.Parameters.Add(SqlParam);
                            }
                        }

                        // take care of transaction business

                        _Command.Connection.Open();

                        _Command.Transaction = _Command.Connection.BeginTransaction();
                        HasTransactionBegan = true;

                        // executes procedure to insert/update/delete data
                        RecordsAffected = _Command.ExecuteNonQuery();

                        _Command.Transaction.Commit();

                        // handle output parameters
                        if (OutputParametersCount > 0)
                        {
                            foreach (SqlParameter SqlParam in _Command.Parameters)
                            {
                                if (SqlParam.Direction == ParameterDirection.Output)
                                {
                                    Parameters.Find((x) => x.ParameterName == SqlParam.ParameterName
                                                        && x.ParameterDirection == ParameterDirection.Output)
                                                        .ParameterValue = SqlParam.Value;
                                }
                            }
                        }
                    }//using _Command
                }
                catch (SqlException SqlEx) { throw SqlEx; }
                catch (Exception ex) { throw ex; }
                finally
                {
                    if (_Command != null && _Command.Transaction != null && HasTransactionBegan)
                        _Command.Transaction.Rollback();

                    if (_Command.Connection.State != ConnectionState.Closed)
                        _Command.Connection.Close();


                }

                return RecordsAffected;
            }
        }
    }
}
