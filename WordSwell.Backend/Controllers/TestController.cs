using Game_Adosaki.Global;
using Microsoft.AspNetCore.Mvc;
using WordSwell.ApiModels.TestCont;

namespace WordSwell.Backend.Controllers;

/// <summary>
/// 테스트용 API
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public class TestController : Controller
{
    /// <summary>
	/// 무조건 성공
	/// </summary>
	/// <returns></returns>
	[HttpGet]
    public ActionResult SuccessCall()
    {
        ObjectResult apiresult = new ObjectResult(200);

        apiresult = StatusCode(200, "성공!");
        GlobalStatic.Log.LoggerFactory.CreateLogger("TestController").LogInformation("sdafadsf");


        //string sss = "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAIBAQIBAQICAgICAgICAwUDAwMDAwYEBAMFBwYHBwcGBwcICQsJCAgKCAcHCg0KCgsMDAwMBwkODw0MDgsMDAz/2wBDAQICAgMDAwYDAwYMCAcIDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAz/wAARCAAgAB8DASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD90fi58b/CvwJ0CHU/FWrQ6TZ3EvkxExvK8r4zhUjVmOAOSBgd8V8cfBP4lfFb4v8A7eevePfDem+M7z4a6lfRaZZQy3DW+iy2ETpby3uJgqu22OaRY49siySckhWjkj/aU+BnxQ/bT/ae1SC00fUNG8MeH5G02xvdWRreyjjTcHmj4zN5siMQYwx2mIMQACPsT9n74L2P7Pnwj0fwnp9zPew6Wjb7mYBXuJHdndiB0G5jgEkhQASxGT8rluaY7HY+rF0uXDwuoyaacpJ2ur7x36dtb6HvcccC5XDK8BUWNksYpqpKFNxcVHllZSdm1JXV03rd+7ZJnZUUUV9UeCFFFFABRRRQB//Z";
        //int nComaIdx = sss.IndexOf(",") + 1;
        //byte[] bytes = Convert.FromBase64String(sss.Substring(nComaIdx));


        //날짜를 기준으로 폴더를 생성
        string BoardFileSaveFolderPath
            = Path.Combine(GlobalStatic.TimeSked.Today.Year.ToString("0000")
                            , GlobalStatic.TimeSked.Today.Month.ToString("00")
                            , GlobalStatic.TimeSked.Today.Day.ToString("00"));

        //날짜를 기준으로 url 생성
        string BoardFileSaveFolderUrl
            = string.Format("{0:0000}/{1:00}/{2:00}/"
                , GlobalStatic.TimeSked.Today.Year
                , GlobalStatic.TimeSked.Today.Month
                , GlobalStatic.TimeSked.Today.Day);


        return apiresult;
    }

    /// <summary>
    /// 데이터 입력 테스트
    /// </summary>
    /// <param name="nData1"></param>
    /// <param name="sData2"></param>
    /// <returns></returns>
    [HttpPost]
    public ActionResult<string> DataTest(
        [FromForm] int nData1
        , [FromForm] string sData2)
    {
        string sReturn
            = string.Format("Data1 : {0}, Data2 : {1}"
                            , nData1, sData2);

        return sReturn;
    }

    /// <summary>
    /// 에러 테스트
    /// </summary>
    /// <param name="nType"></param>
    /// <returns></returns>
    [HttpPut]
    public ActionResult ErrorCall([FromForm] int nType)
    {
        ObjectResult apiresult = new ObjectResult(200);

        if (0 == nType)
        {
            apiresult = StatusCode(200, "성공!");
        }
        else
        {
            apiresult = StatusCode(500, "에러!");
        }


        return apiresult;
    }

    /// <summary>
    /// 모델 주고 받기 GET
    /// </summary>
    /// <param name="TestCallModel"></param>
    /// <returns></returns>
    [HttpGet]
    public ActionResult<TestResultModel> ModelGet([FromQuery] TestCallModel TestCallModel)
    {
        TestResultModel resultNew = new TestResultModel();

        resultNew.Int = TestCallModel.Int;
        resultNew.String = TestCallModel.String;
        resultNew.DateTime = TestCallModel.DateTime;

        return resultNew;
    }

    /// <summary>
    /// 모델 주고 받기 POST
    /// </summary>
    /// <param name="TestCallModel"></param>
    /// <returns></returns>
    [HttpPost]
    public ActionResult<TestResultModel> ModelPost(TestCallModel TestCallModel)
    {
        TestResultModel resultNew = new TestResultModel();

        resultNew.Int = TestCallModel.Int;
        resultNew.String = TestCallModel.String;
        resultNew.DateTime = TestCallModel.DateTime;

        return resultNew;
    }

}
