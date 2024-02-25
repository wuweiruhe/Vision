using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision_NBB.Model.DB
{

    public class DBConfigStroage : IDBConfigStroage
    {
        public const string DbFileName = "WindAppConfig.sqlite3";
 
        public static string ConfigDbPath = AppDomain.CurrentDomain.BaseDirectory + "UserFile";



        private static SQLiteAsyncConnection _connection;

        public  static SQLiteAsyncConnection Connection
        {
            get
            {
                Directory.CreateDirectory(ConfigDbPath);
                string databaseFileName = Path.Combine(ConfigDbPath, DbFileName);

                //LV:10   7   -12

                if (_connection == null)
                {
                    _connection= new SQLiteAsyncConnection(databaseFileName);
                }

                return _connection;
            }
        }

        public async Task InitializaAsync()
        {
            try
            {

                await Connection.CreateTableAsync<DBConfig>();
             
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}");
            }
        }

        public async Task<int> AddAsync(DBConfig dbConfig)
        {
            return await Connection.InsertAsync(dbConfig);
        }



        public async Task<int> UpdateAsync(DBConfig dbConfig)
        {

            return await Connection.UpdateAsync(dbConfig);
        }

        public async Task<IEnumerable<DBConfig>> ListAsync()
        {
            return await Connection.Table<DBConfig>().ToListAsync();
        }






        /// <summary>
        /// 查询返回
        /// </summary>
        /// <param name="query_sql"></param>
        /// <param name="dbConfig"></param>
        /// <returns>查询成功正常返回；查询失败返回一个空的集合</returns>
        public async Task<IEnumerable<DBConfig>> ListAsync(string query_sql, DBConfig dbConfig = null)
        {
            try
            {
                dbConfig = null;
                return await Connection.QueryAsync<DBConfig>(query: query_sql, args: dbConfig);
            }
            catch (Exception ex)
            {
                //查询出现异常并返回一个空集合
                string msg = ex.Message;
                return new List<DBConfig>();
            }
        }



        public async Task<int> ExecuteAsync(string str_sql)
        {
            return await Connection.ExecuteAsync(str_sql);
        }



    }
}
