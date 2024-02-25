using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision_NBB.Model.DB
{
    [Table(Info.TableName)]
   public class Model_UserConfog
    {
       internal struct Info
        {
            internal const string TableName = "UserConfig";
        }
        /// <summary>
        /// 主键
        /// </summary>
        [Column("Id")]
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }

        public string Level { get; set; }
    } 
}
