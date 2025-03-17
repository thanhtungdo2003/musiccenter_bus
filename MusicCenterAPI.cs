using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MusicBusniess;
using MusicCenterAPI.Data;
using MusicCenterAPI.ProcedureStorage;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.DataProtection;
using MuscicCenter.Storage;
using TT_DBLib.Storage;
using TT_DBLib.Storage.Database;

namespace MusicCenterAPI
{
    public class MusicCenterAPI : DatabaseStruct, IFileManager, IMusicCenterAPI
    {
        public static IFileManager fileManager;
        public static string baseDirectoryPath;
        public static IDataManager dataManager;
        private static bool hasLoadData = false;
        public Dictionary<string, TableInfo> tables = new Dictionary<string, TableInfo>();
        public ProcedureManager procedure;
        private IConfiguration _configuration;
        private string keySecret = "";

        
        public MusicCenterAPI(IConfiguration configuration)
        {
            _configuration = configuration;
            Console.OutputEncoding = Encoding.UTF8;
            baseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            List<TableInfo> tablesInfo = getTables();
            foreach (var item in tablesInfo)
            {
                tables[item.TableName] = item;
            }
            keySecret = configuration["JwtSettings:Key"];
            dataManager = new DatabaseManager(_configuration["SQLServer:Server"], _configuration["SQLServer:Database"], tablesInfo);
            dataManager.Connect();
            fileManager = new FileManager();
            if (!hasLoadData)
            {
                hasLoadData = true;
            }
            procedure = new ProcedureManager(this);
        }
        public string BasePath
        {
            get { return baseDirectoryPath; }
        }


        public static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public List<Dictionary<string, object>> CoventToDictionarysWithDataTable(DataTable dataTable)
        {
            if (dataTable == null) return new List<Dictionary<string, object>>() { new Dictionary<string, object>(){
                
            }
            };
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            foreach (DataRow row in dataTable.Rows)
            {
                Dictionary<string, object> rowDict = new Dictionary<string, object>();

                foreach (DataColumn column in dataTable.Columns)
                {
                    rowDict[column.ColumnName] = row[column];
                }
                result.Add(rowDict);
            }
            return result;
        }
        public bool FileAdd(string projectDirectory, IFormFile file)
        {
            return fileManager.FileAdd(projectDirectory, file);

        }

        public IFormFile FileExport(string projectDirectory, string filename)
        {
            return fileManager.FileExport(projectDirectory, filename);
        }
        public string GenerateToken(string userId, string username)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public ClaimsPrincipal DecodeToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(keySecret);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                // Giải mã token và trả về thông tin ClaimsPrincipal
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                return principal;
            }
            catch (Exception)
            {
                // Trả về null nếu token không hợp lệ
                return null;
            }
        }

        public AccountData Authenticate(string username, string password)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@username", SqlDbType.NVarChar) {Value = username},
                new SqlParameter("@hashpass", SqlDbType.VarChar) {Value = password}
            };
            var user = CoventToDictionarysWithDataTable(ProcedureCall(ProcedureName.LOGIN.GetValue(), parameters));
            // return null if user not found
            if (user == null)
                return null;
            AccountData account = new AccountData(this, username);

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(keySecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, account.UserName),
                    new Claim("PremiumEx", account.PremiumEx)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            account.Token = tokenHandler.WriteToken(token);
            return account;
        }

        public bool Connect()
        {
            return dataManager.Connect();
        }

        public void createDatabase()
        {
            dataManager.createDatabase();
        }

        public List<string> getStringColumns(string tableName, string columnName)
        {
            return dataManager.getStringColumns(tableName, columnName);
        }

        public List<object> GetColumnValues(string tableName, string columnName)
        {
            return dataManager.GetColumnValues(tableName, columnName);
        }

        public object InsertData(string tableName, string keyColumn, Dictionary<string, object> columnValues)
        {
            return dataManager.InsertData(tableName, keyColumn, columnValues);
        }

        public void UpdateData(string tableName, string columnName, string keyName, string key, object newValue)
        {
            dataManager.UpdateData(tableName, columnName, keyName, key, newValue);
        }

        public bool DeleteDataByValue(string tableName, string columnName, object value)
        {
            return dataManager.DeleteDataByValue(tableName, columnName, value);
        }

        public object TruyVanDuLieu(string query)
        {
            return dataManager.TruyVanDuLieu(query);
        }

        public DataTable getDataTable(string query)
        {
            return dataManager.getDataTable(query);
        }

        public void ProcedureCreate(string name, string content)
        {
            dataManager.ProcedureCreate(name, content);
        }

        public DataTable ProcedureCall(string name, SqlParameter[]? parameters)
        {
            return dataManager.ProcedureCall(name, parameters);
        }
        public DataTable ProcedureCall(ProcedureName name, SqlParameter[]? parameters)
        {
            return procedure.ProcedureCall(name, parameters);
        }
        public object? getValueByKey(string tableName, string column, string key, string valueKey)
        {
            return dataManager.getValueByKey(tableName, column, key, valueKey);
        }


        public string GetFilePathConfig(string key, string field)
        {
            return _configuration[$"{key}:{field}"];
        }
    }
}
