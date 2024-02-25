using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision_NBB.Model.DB
{

    public interface IDBConfigStroage
    {
        //规定具有的功能

        /// <summary>
        /// 初始化数据库
        /// </summary>
        Task InitializaAsync();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="poetry"></param>
        Task<int> AddAsync(DBConfig dbConfig);


        /// <summary>
        /// 更新 需要有主键才能使用
        /// </summary>
        /// <param name="poetry"></param>
        Task<int> UpdateAsync(DBConfig dbConfig);

        /// <summary>
        /// 执行 SQL语句
        /// </summary>
        /// <param name="str_sql">SQL 语句</param>
        /// <returns>改变数据的数据量</returns>
        Task<int> ExecuteAsync(string str_sql);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<DBConfig>> ListAsync();







        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<DBConfig>> ListAsync(string query_sql, DBConfig dbConfig = null);
      
    }
}
