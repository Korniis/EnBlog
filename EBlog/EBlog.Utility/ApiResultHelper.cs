﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBlog.Utility
{
    public static class ApiResultHelper
    {
        /// <summary>
        /// 成功后返回数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult Success( dynamic data)
        {
            return new ApiResult
            {
                Code = 200,
                Data = data,
                Message = "操作成功",
                Total = 0,
            };
        }
        /// <summary>
        /// 失败后返回数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult Error(string msg)
        {
            return new ApiResult
            {
                Code = 500,
                Data = null,
                Message = msg,
                Total = 0,
            };
        }
        public static ApiResult AuthError(string msg)
        {
            return new ApiResult
            {
                Code = 401,
                Data = null,
                Message = msg,
                Total = 0,
            };
        }


    }
}
