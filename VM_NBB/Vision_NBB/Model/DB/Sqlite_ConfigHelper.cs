using Apps.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vision_NBB.Model.DB
{

    /// <summary>
    /// 配置文件
    /// </summary>
    public static class Sqlite_ConfigHelper
    {
        private static IDBConfigStroage _DBConfigStroage = new DBConfigStroage();


        private const string ConfigTableName = "APPConfig";

        public static async void Init()
        {
            await _DBConfigStroage.InitializaAsync();
        }

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="selection_param">选择参数</param>
        /// <param name="key_param">键 参数</param>
        /// <returns></returns>
        public static async Task<string> GetConfigValue(string selection_param, string key_param)
        {
            string result = string.Empty;
            string message = $"选择器:{selection_param}-键:{key_param}";
            try
            {
                string str_sql = $"select * from {ConfigTableName} where Selection='{selection_param}' and Key='{key_param}'";
                var config_result = await _DBConfigStroage.ListAsync(query_sql: str_sql, dbConfig: new DBConfig());

                //检测是否包含元素
                if (config_result.Any())
                {
                    var value = config_result.First().Value;

                    if (value != null)
                    {
                        result = value;
                    }
                }
                else
                {
                    //没有这个元素
                    await SetConfigValue(selection_param, key_param, string.Empty);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"{message}获取值;\r\n出现异常{ex}");
                result = "Config_Expection";
            }

            return result;
        }

        /// <summary>
        /// 设置键值
        /// </summary>
        /// <param name="selection_param">选择 参数</param>
        /// <param name="key_param">键</param>
        /// <param name="value_param">值</param>
        public static async Task<bool> SetConfigValue(string selection_param, string key_param, string value_param)
        {
            bool result_flag = false;
            string message = $"选择器:{selection_param}-键:{key_param}-值:{value_param}";
            try
            {
                //需要先判断有没有
                //有-》直接更新；没有-》需要重新创建；
                string querySql = $"select * from {ConfigTableName} where Selection='{selection_param}' and Key='{key_param}'";
                var query_result = await _DBConfigStroage.ListAsync(query_sql: querySql, dbConfig: new DBConfig());

                var update_object = new DBConfig()
                {
                    Selection = selection_param,
                    Key = key_param,
                    Value = value_param
                };
                //检测是否包含元素
                if (query_result.Any())
                {
                    update_object.Id = query_result.First().Id;
                    //有这个元素  更新
                    if (await _DBConfigStroage.UpdateAsync(update_object) > 0)
                    {
                        result_flag = true;
                    }
                    else
                    {
                        LogHelper.Error($"{message}->更新数据失败;");
                    }
                }
                else
                {
                    //没有这个元素 添加
                    if (await _DBConfigStroage.AddAsync(update_object) > 0)
                    {
                        result_flag = true;
                    }
                    else
                    {
                        LogHelper.Error($"{message}->添加数据失败;");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"{message}->添加数据失败;\r\n异常信息:{ex}");
                //if (ex.Message.Contains("CannotOpen"))
                //{
                //    await _DBConfigStroage.InitializaAsync();
                //    await GetConfigValue(selection_param, key_param);
                //}

                if (ex.Message.Contains("no such table: APPConfig"))
                {
                    await _DBConfigStroage.InitializaAsync();
                    await SetConfigValue(selection_param, key_param, value_param);
                }
            }
            return result_flag;
        }


        public static async Task<bool> SetUserConfigValue(Model_UserConfog modelUser)
        {
            bool result_flag = false;
            string message = $"";
            try
            {
                //需要先判断有没有
                //有-》直接更新；没有-》需要重新创建；

                string querySql = $"select * from {Model_UserConfog.Info.TableName} where {nameof(modelUser.UserName) }= '{modelUser.UserName}' ";
                var query_result = await _DBConfigStroage.ListAsync(query_sql: querySql, dbConfig: new DBConfig());

                var update_object = modelUser;
                //检测是否包含元素
                if (query_result.Any())
                {
                    update_object.Id = query_result.First().Id;
                    //有这个元素  更新

                    if (await DBConfigStroage.Connection.UpdateAsync(update_object) > 0)
                    {
                        result_flag = true;
                    }
                    else
                    {
                        LogHelper.Error($"{message}->更新数据失败;");
                    }
                }
                else
                {
                    //没有这个元素 添加
                    if (await DBConfigStroage.Connection.InsertAsync(update_object) > 0)
                    {
                        result_flag = true;
                    }
                    else
                    {
                        LogHelper.Error($"{message}->添加数据失败;");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"{message}->添加数据失败;\r\n异常信息:{ex}");
                //if (ex.Message.Contains("CannotOpen"))
                //{
                //    await _DBConfigStroage.InitializaAsync();
                //    await GetConfigValue(selection_param, key_param);
                //}

                if (ex.Message.Contains("no such table"))
                {
                    await DBConfigStroage.Connection.CreateTableAsync<Model_UserConfog>();
                    await SetUserConfigValue(modelUser);
                }
            }
            return result_flag;
        }

         

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="selection_param">选择参数</param>
        /// <param name="key_param">键 参数</param>
        /// <returns></returns>
        public static async Task<List<Model_UserConfog>> GetUserConfigValue()
        {
            var result = new List<Model_UserConfog>();
            string message = $"";
            try
            {
                result =  await   DBConfigStroage. Connection.Table<Model_UserConfog>().ToListAsync();
            }
            catch (Exception ex)
            {
                LogHelper.Error($"{message}获取值;\r\n出现异常{ex}");             
            }
            return result;
        }


        //-----------------------------------------------------------------
        /// <summary>
        /// 设置配置参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="selection_param">选择器参数</param>
        /// <returns></returns>
        private static async Task<bool> SetConfigValue<T>(T t, string selection_param)
        {
            bool result = true;
            if (t == null)
            {
                result = false;
                return result;
            }

            try
            {
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo property in propertys)
                {
                    var t_type = property.PropertyType.Name.ToUpper();
                    if (t_type == "STRING" || t_type == "BOOL" || t_type == "INT" ||
                       t_type == "DOUBLE" || t_type == "FLOAT")
                    {
                        var t_name = property.Name;
                        var t_value = property.GetValue(t, null);
                        var tmp_result = await SetConfigValue
                                (selection_param: selection_param, key_param: t_name, value_param: $"{t_value}");
                        if (!tmp_result)
                        {
                            result = false;
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 设置配置参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        private static async Task<T> GetConfigValue<T>(T t, string selection_param)
        {
            if (t == null)
            {
                return t;
            }
            try
            {
                PropertyInfo[] propertys = t.GetType().GetProperties();
                for (int i = 0; i < propertys.Length; i++)
                {

                    Console.WriteLine("i" + propertys[i]);
                }

                foreach (PropertyInfo property in propertys)
                {
                    var t_type = property.PropertyType.Name.ToUpper();

                    if (t_type == "STRING" || t_type == "BOOL" || t_type == "INT" ||
                        t_type == "DOUBLE" || t_type == "FLOAT")
                    {
                        var t_name = property.Name;
                        var t_value = await GetConfigValue(selection_param: selection_param, key_param: t_name);
                        property.SetValue(t, t_value, null);
                    }


                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"获取配置参数出现异常:{ex}");
            }

            return t;
        }
    }
}
