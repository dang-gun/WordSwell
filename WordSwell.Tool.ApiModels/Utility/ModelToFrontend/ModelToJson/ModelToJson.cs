using System.Reflection;
using System.Text;

using DGUtility.ProjectXml;


namespace DGUtility.ModelToFrontend;

/// <summary>
/// 모델을 Json 파일로 변환하기 위한 클래스
/// </summary>
/// <remarks>
/// 모델을 직열화하면 주석이 날아간다.<br />
/// 이 기능은 주석까지 포함하기위해 사용된다.<br />
/// 이 클래스는 프로퍼티가 아닌 필드를 처리하기위한 용도이다.<br />
/// 프로퍼티는 가급적 JsonConvert를 쓰는 것이 좋다.
/// </remarks>
public class ModelToJson
{
    /// <summary>
    /// 사용할 프로젝트Xml
    /// </summary>
    public ProjectXmlAssist ProjectXml { get; set; }
        = new ProjectXmlAssist();

    /// <summary>
	/// 지정된 모델
	/// </summary>
	private object? MyModel { get; set; } = null;

    /// <summary>
	/// 베이스가 있다면 베이스의 네임 스페이스
	/// </summary>
	public string BaseNamespace { get; private set; } = string.Empty;
    /// <summary>
    /// 베이스가 있다면 베이스의 이름
    /// </summary>
    public string BaseName { get; private set; } = string.Empty;

    /// <summary>
    /// 소속된 네임스페이스
    /// </summary>
    public string ModelNamespace { get; private set; } = string.Empty;

    /// <summary>
    /// 지정된 모델의 이름
    /// </summary>
    public string ModelName { get; private set; } = string.Empty;


    /// <summary>
    /// 분해한 맴버 데이터
    /// </summary>
    private List<JsonModelMember> ModelMember { get; set; }
        = new List<JsonModelMember>();


    /// <summary>
    /// 프로젝트 xml만 지정하여 초기화한다.
    /// </summary>
    /// <param name="projectXmlAssist"></param>
    public ModelToJson(ProjectXmlAssist projectXmlAssist)
    {
        this.ProjectXml = projectXmlAssist;
    }

    /// <summary>
    /// 사용할 모델을 설정한다.
    /// </summary>
    /// <remarks>
    /// ProjectXml은 가지고 있는 것을 쓴다.
    /// </remarks>
    /// <param name="model"></param>
    public void Data_Set(object model)
    {
        this.Reset(model, this.ProjectXml);
    }

    /// <summary>
    /// model projectXmlAssist를 저장하고 model 맴버를 분해한다.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="projectXmlAssist"></param>
    public void Reset(object model, ProjectXmlAssist? projectXmlAssist)
    {
        //원본 저장
        this.MyModel = model;
        if (null == projectXmlAssist)
        {
            this.ProjectXml = new ProjectXmlAssist();
        }
        else
        {
            this.ProjectXml = projectXmlAssist;
        }

        //이 개채의 개채 형식을 받는다.
        Type typeMy = this.MyModel.GetType();

        if (null != typeMy.BaseType)
        {//베이스가 있는지 확인
            if (null != typeMy.BaseType.Namespace)
            {//베이스의 네임스페이스가 있는지 확인
                this.BaseNamespace = typeMy.BaseType.Namespace;
            }

            this.BaseName = typeMy.BaseType.Name;
        }


        //네임스페이스 추출
        if (null != typeMy.Namespace)
        {
            this.ModelNamespace = typeMy.Namespace;
        }

        //이름 추출
        this.ModelName = typeMy.Name;


        //기존 리스트 제거
        this.ModelMember.Clear();

        IList<FieldInfo> fields = new List<FieldInfo>(typeMy.GetFields());

        //맴버 추가
        foreach (FieldInfo item in fields)
        {
            if (null != item)
            {
                JsonModelMember newJMM = new JsonModelMember();
                newJMM.Name = item.Name;

                newJMM.Value = item.GetValue(this.MyModel)!.ToString()!;
                this.ModelMember.Add(newJMM);
            }

        }//end foreach item
    }

    /// <summary>
    /// 지정된 ProjectXml리스트에서 주석정보를 찾는다.
    /// </summary>
    /// <param name="sTarget"></param>
    /// <returns></returns>
    public string ProjectXml_SummaryGet(string sTarget)
    {
        string sReturn = string.Empty;
        sReturn = this.ProjectXml.SummaryGet(sTarget);

        return sReturn;
    }

    /// <summary>
    /// 가지고 있는 정보를 json 형식으로 출력한다.
    /// </summary>
    /// <returns></returns>
    public string ToJsonString()
    {

        return this.ToJsonString(
                "{" + Environment.NewLine
                , "    \"{0}\": \"{1}\"," + Environment.NewLine
                , "}"
            );
    }

    /// <summary>
    /// json 형식으로 출력한다.
    /// </summary>
    /// <param name="sHead"></param>
    /// <param name="sItemBody"></param>
    /// <param name="sFooter"></param>
    /// <returns></returns>
    public string ToJsonString(
        string sHead
        , string sItemBody
        , string sFooter)
    {
        StringBuilder sbReturn = new StringBuilder();

        //머리 만들기*********
        //주석 검색어 만들기 - 타입 명
        string sT = string.Format("T:{0}.{1}"
                                        , this.ModelNamespace
                                        , this.ModelName);
        //필드를 검색해서 json을 만드므로 F다
        //주석 검색어 만들기 - 요소 명
        string sF = string.Format("F:{0}.{1}"
                                        , this.ModelNamespace
                                        , this.ModelName);

        //베이스 이름
        List<string> listF = new List<string>();
        listF.Add(string.Format(
                    "F:{0}.{1}"
                    , this.BaseNamespace
                    , this.BaseName));

        //머리 넣기
        sbReturn.Append(sHead);

        if (null != this.ProjectXml)
        {
            

            //요소
            for (int i = 0; i < this.ModelMember.Count; ++i)
            {
                JsonModelMember itemJMM = this.ModelMember[i];

                //검색어 완성 시키기
                string sF_Name = sF + "." + itemJMM.Name;

                //주석 ****************
                string sSummary
                    = this.ProjectXml_SummaryGet(sF_Name);

                if (string.Empty == sSummary)
                {//못찾았다.

                    //상속받은 어딘가에 있을 확률이 높다.
                    foreach (string sF_Item in listF)
                    {
                        sSummary = this.ProjectXml_SummaryGet(sF_Item + "." + itemJMM.Name);

                        if (string.Empty != sSummary)
                        {//찾았다.
                            break;
                        }
                    }
                }

                if (string.Empty != sSummary)
                {//주석 내용이 있다.
                    sbReturn.Append(string.Format(@"    /** {0} */" + Environment.NewLine
                                                    , sSummary));
                }



                //요소 추가 ****************
                sbReturn.Append(string.Format(sItemBody, itemJMM.Name, itemJMM.Value));
            }
        }

        //발 넣기
        sbReturn.Append(sFooter);

        return sbReturn.ToString();
    }

}
