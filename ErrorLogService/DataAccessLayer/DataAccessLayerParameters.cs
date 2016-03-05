using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DataAccessLayer
{


    public class DataAccessLayerParameter
    {
        /// <summary>
        /// 
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SqlDbType ParameterType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ParameterDirection ParameterDirection { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object ParameterValue { get; set; }

        public int ParameterSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DataAccessLayerParameter()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_ParameterName"></param>
        /// <param name="_ParameterType"></param>
        /// <param name="_ParameterDirection"></param>
        /// <param name="_ParameterValue"></param>
        public DataAccessLayerParameter(string _ParameterName, SqlDbType _ParameterType, ParameterDirection _ParameterDirection = ParameterDirection.Input, object _ParameterValue = null)
        {
            this.ParameterName = _ParameterName;
            this.ParameterType = _ParameterType;
            this.ParameterDirection = _ParameterDirection;
            this.ParameterValue = _ParameterValue;
        }
    }
    public class DataAccessLayerParameterList : List<DataAccessLayerParameter>
    {
        public DataAccessLayerParameterList()
        {
            // initialize collection
        }
    }

    public enum ProcReturnType
    {
        DataSet,
        JSON,
        XML
    }

    public static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TempDataSet"></param>
        /// <returns></returns>
        public static string ToXml(this DataSet TempDataSet)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    try
                    {
                        var xmlSerializer = new XmlSerializer(typeof(DataSet));
                        xmlSerializer.Serialize(streamWriter, TempDataSet);
                        return Encoding.UTF8.GetString(memoryStream.ToArray());
                    }
                    catch (IOException iex) { throw iex; }
                    catch (Exception ex) { throw ex; }

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TempDataSet"></param>
        /// <returns></returns>
        public static string ToJSON(this DataSet TempDataSet)
        {

            return JsonConvert.SerializeObject(TempDataSet, Formatting.Indented);
        }

        public static string ToJSON(this DataTable TempDataTable)
        {

            return JsonConvert.SerializeObject(TempDataTable, Formatting.Indented);
        }

    }
}
