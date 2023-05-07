using System;
using System.Net.Http;
using System.Collections.Generic;

/// <summary>
/// 使用HttpClient，對Api發送request
/// </summary>
public class HttpClientApi
{
    /// <summary>
    /// 對Shibe Api發送request，並將取得的JSON，轉為List<string>回傳，內容為圖片網址
    /// </summary>
    /// <returns></returns>
    public List<string> GetDogImageUrl()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                // 設定請求時間
                client.Timeout = TimeSpan.FromSeconds(30);
                // 設定請求方法為Get、目標Uri
                HttpResponseMessage response = client.GetAsync($"http://shibe.online/api/shibes?count=1&urls=true").GetAwaiter().GetResult();
                // 如果不是200，會跳到ex
                response.EnsureSuccessStatusCode();
                // 將response body轉為string
                //string apiResult = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                //List<string> listResult = JsonSerializer.Deserialize<List<string>>(apiResult);
                List<string> listResult = response.Content.ReadAsAsync<List<string>>().GetAwaiter().GetResult();
                return listResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    /// <summary>
    /// 對Shibe Api取得的網址，發送request，並將回傳的圖片轉為byte[]，同時回傳表示資源類型的header欄位
    /// </summary>
    /// <param name="listResult"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    public byte[] GetDogImage(List<string> listResult, out string contentType)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                client.Timeout = TimeSpan.FromSeconds(30);
                HttpResponseMessage response = client.GetAsync(listResult[0]).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                // 將response body轉為byte[]
                byte[] imageByteArray = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                // 取得表示資源類型的media type
                contentType = response.Content.Headers.ContentType.MediaType;

                return imageByteArray;
            }
            catch (Exception ex)
            {
                contentType = null;
                return null;
            }
        }
    }
}