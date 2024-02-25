using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision_NBB.Model.DB
{
    [Table("APPConfig")]
    public class DBConfig
    {

        /// <summary>
        /// 主键
        /// </summary>
        [Column("Id")]
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// 选择
        /// </summary>
        public string Selection { get; set; }

        /// <summary>
        /// 键
        /// </summary>
        [Unique]
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        //生成的一个虚拟属性，和数据库建立关联  
     

        private string _snippet;

        [Ignore]
        public string Snippet => _snippet = $"{Key}:{Value}";
    }


}
