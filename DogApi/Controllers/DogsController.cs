using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace DogApi.Controllers
{
    public class DogsController : ApiController
    {
        /// <summary>
        /// GET api/dogs
        /// 透過Shibe Api取得圖片，並直接顯示於畫面
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            // 使用httpClientApi中的方法
            HttpClientApi httpClientApi = new HttpClientApi();
            List<string> listResult = httpClientApi.GetDogImageUrl();
            string contentType;
            byte[] imageByteArray = httpClientApi.GetDogImage(listResult, out contentType);

            // 設定回傳給client端的response內容
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            // body以byte array的形式回傳
            response.Content = new ByteArrayContent(imageByteArray);
            // body以來源網頁取得的media type讀取
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            return response;
        }
    }
}