using AlibabaCloud.OpenApiClient.Models;
using AlibabaCloud.SDK.Dysmsapi20170525;
using AlibabaCloud.TeaUtil;
using AlibabaCloud.TeaUtil.Models;
using AlibabaCloud.SDK.Dysmsapi20170525.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tea;

namespace EBlog.Utility
{ 
    public class SMSHelper
    {
       public static Client CreateClient(string accessKeyId, string accessKeySecret)
        {
            Config config = new Config()
            {
                AccessKeyId = "LTAI5tAqFDbasdasdrUa3GyaM8vH6C",
                AccessKeySecret = "bGiaLJk2BaghahBvNhSud332ZGj0rmydryeKdt"

            };
            config.Endpoint = "dysmsapi.aliyuncs.com";
            return new Client(config);
        }
        public static async void UseSMS(string phone, string token)
        {
            Client client = CreateClient(@$"你的密钥", $@"你的安全密钥");

            SendSmsRequest sendSmsRequest = new SendSmsRequest
            {
                PhoneNumbers = phone,
                SignName = "模板_Code",
                
                TemplateCode = "2", //模板Code
                TemplateParam = $"{{\"code\":\"{token}\"}}",
            };
            try
            {
                // 复制代码运行请自行打印 API 的返回值
             var k = await  client.SendSmsWithOptionsAsync(sendSmsRequest, new RuntimeOptions());
                Console.WriteLine(k);
            }
            catch (TeaException error)
            {
                // 如有需要，请打印 error
                Common.AssertAsString(error.Message);
            }
            catch (Exception _error)
            {
                TeaException error = new TeaException(new Dictionary<string, object>
              {
                  { "message", _error.Message }
              });
                // 如有需要，请打印 error
                Common.AssertAsString(error.Message);
            }
        }

    }
}
