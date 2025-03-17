using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicCenterAPI.ProcedureStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using MusicCenterAPI.Data;

namespace MusicBusniess
{
    public interface IMusicCenterAPI
    {
        /// <summary>
        /// Phương thức kết nối tới SQL
        /// </summary>
        bool Connect();
        /// <summary>
        /// Phương thức khởi tạo database 
        /// </summary>
        /// <param name="indexServer"></param>
        void createDatabase();
        /// <summary>
        /// Phương thức load dữ liệu từ Database vào DataGridView
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="tableName"></param>

        List<string> getStringColumns(string tableName, string columnName);
        /// <summary>
        /// Phương thức trả về danh sách giá trị kiểu object thuộc một cột SQL
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        List<object> GetColumnValues(string tableName, string columnName);
        /// <summary>
        /// Phương thức lấy dữ liệu tại một ô SQL
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="column"></param>
        /// <param name="key"></param>
        /// <param name="valueKey"></param>
        /// <returns></returns>
        object? getValueByKey(string tableName, string column, string key, string valueKey);
        /// <summary>
        /// Phương thức thêm dữ liệu
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnValues"></param>
        /// <returns></returns>
        object InsertData(string tableName, string keyColumn, Dictionary<string, object> columnValues);
        /// <summary>
        /// Phương thức sửa dữ liệu
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="keyName"></param>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        void UpdateData(string tableName, string columnName, string keyName, string key, object newValue);
        /// <summary>
        /// Phương thức xoá dữ liệu
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool DeleteDataByValue(string tableName, string columnName, object value);
        /// <summary>
        /// Hàm truy vấn dữ liệu
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        object TruyVanDuLieu(string query);
        void ProcedureCreate(string name, string content);
        DataTable ProcedureCall(ProcedureName name, SqlParameter[]? parameters);
        DataTable getDataTable(string query);

        List<Dictionary<string, object>> CoventToDictionarysWithDataTable(DataTable dataTable);
        bool FileAdd(string folder, IFormFile file);
        IFormFile FileExport(string folder, string filename);
        AccountData Authenticate(string username, string password);
        string GetFilePathConfig(string key, string field);
        ClaimsPrincipal DecodeToken(string token);
        string GenerateToken(string userId, string username);
    }
}
