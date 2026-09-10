using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChatAi
{
    public class ChatAI
    {
        /// <summary>
        /// 可以请求
        /// </summary>
        public static Func<bool> CanRequest = null;
        /// <summary>
        /// 请求开始
        /// </summary>
        public static Action RequestStart = null;
        /// <summary>
        /// 聊天响应
        /// </summary>
        public static Action<string> ChatResponse = null;
        /// <summary>
        /// 请求超时
        /// </summary>
        public static Action RequestTimeout = null;
        /// <summary>
        /// 请求失败
        /// </summary>
        public static Action RequestFailure = null;
        /// <summary>
        /// 请求结束
        /// </summary>
        public static Action RequestEnd = null;


        public static async void InputAsync(string text, int type)
        {
            if (CanRequest?.Invoke() != true) return;

            RequestStart?.Invoke();

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                string v = await Apis.Chat.InputAsync(text, type);
                if (v == null) { tContentPatch.ContentPatch.PrintTry("返回文本为[null]"); return; }
                if (v.Length < 1) { tContentPatch.ContentPatch.PrintTry("返回文本长度为[0]"); return; }

                ChatResponse?.Invoke(v);
            }
            catch (TaskCanceledException ex)
            {
                tContentPatch.ContentPatch.PrintTry($"任务超时: [{ex.Message}]");

                RequestTimeout?.Invoke();
            }
            catch (HttpRequestException ex)
            {
                tContentPatch.ContentPatch.PrintTry($"http响应失败: [{ex.Message}]");

                RequestFailure?.Invoke();
            }
            catch (Exception ex)
            {
                tContentPatch.ContentPatch.PrintTry($"其它异常: [{ex.Message}]");

                RequestFailure?.Invoke();
            }
            finally
            {
                RequestEnd?.Invoke();
            }
        }

        public static void SetApiKeyTry(string key)
        {
            try
            {
                Apis.Chat.SetApiKey(key);
                tContentPatch.ContentPatch.PrintTry($"设置api的key为: [{Apis.Chat.apiKey}]");
            }
            catch (Exception ex)
            {
                tContentPatch.ContentPatch.PrintTry($"设置api的key失败: [{ex.Message}]");
            }
        }
    }
}
