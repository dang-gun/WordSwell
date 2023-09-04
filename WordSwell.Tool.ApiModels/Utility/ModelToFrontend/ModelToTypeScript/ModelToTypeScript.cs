using DGU_ModelToOutFiles.Global.Attributes;
using DGUtility.ProjectXml;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace DGUtility.ModelToFrontend;

/// <summary>
/// 모델을 타입스크립트용 파일로 변환하기위한 클래스
/// </summary>
public class ModelToTs
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
	private List<TypeScriptModelMember> ModelMember { get; set; }
		= new List<TypeScriptModelMember>();

	
	/// <summary>
	/// 임포트에 사용할 루트 경로 지정
	/// </summary>
	/// <remarks>
	/// 임포트 라인을 생성할때 맨 앞의 경로로 사용될 문자열이다.<br />
	/// </remarks>
	public string ImportRootDir { get; set; } = string.Empty;
	/// <summary>
	/// 임포트 라인을 생성할때 사용될 아이템 리스트
	/// </summary>
	/// <remarks>
	/// 외부에 표시될 이름, 임포트 경로(ImportRootDir에서 지정한 경로 제외한 경로)
	/// </remarks>
	public Dictionary<string, string> ImportItem
		= new Dictionary<string, string>();


	/// <summary>
	/// 네임스페이스로 임포트할 경로를 찾는 대리자
	/// </summary>
	/// <param name="sNamespace"></param>
	/// <returns></returns>
    public delegate List<ModelToTextImportModel> ImportSearchDelegate(string sNamespace);
    /// <summary>
    /// 아이템 검색시 참조를 추가해야 할때 호출되는 콜백
    /// </summary>
    public ImportSearchDelegate ImportSearchCallback = (string sNamespace)
        => { return new List<ModelToTextImportModel>(); };


/// <summary>
/// 프로젝트 xml만 지정하여 초기화한다.
/// </summary>
/// <param name="projectXmlAssist"></param>
public ModelToTs(ProjectXmlAssist projectXmlAssist)
	{
		this.ProjectXml = projectXmlAssist;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="model"></param>
	public ModelToTs(object model)
	{
		this.Reset(model, null);
	}

	/// <summary>
	/// 사용할 모델을 설정한다.
	/// </summary>
	/// <remarks>
	/// ProjectXml은 가지고 있는 것을 쓴다.
	/// </remarks>
	/// <param name="model"></param>
	public void TypeData_Set(object model)
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

		//맴버 추가
		foreach (var item in typeMy.GetProperties())
		{
			if (null != item)
			{
				//중단점 잡을 개체
				if (item.Name == "String2")
				{
					Debug.WriteLine(item.Name);
				}

				//변수 타입 이름
				string sType = item.PropertyType.Name;
				string sNameFull = null != item.PropertyType.FullName ? item.PropertyType.FullName : string.Empty;
                string sArrayType = string.Empty;
				
				//널 허용 여부
				bool bNullable = false;
				//출력하지 않는 프로퍼티인지 여부
				bool bModelOutputNo = false;
				//변수형 강제 지정
				string sVarTypeEnforce = string.Empty;

                if (item.PropertyType.Name == "List`1")
				{//리스트 타입이다.

					//리스트 타입인걸 알리고
					sType = "List";
					//배열이 가지고 있는 타입을 저장한다.
					sArrayType = item.PropertyType.GenericTypeArguments[0].Name;

                    if (null != item.PropertyType.GenericTypeArguments[0].FullName)
                    {
                        sNameFull = item.PropertyType.GenericTypeArguments[0].FullName!;
                    }
                }
                else if (item.PropertyType.Name == "ICollection`1")
                {//리스트 타입이다.

                    //리스트 타입인걸 알리고
                    sType = "List";
                    //배열이 가지고 있는 타입을 저장한다.
                    sArrayType = item.PropertyType.GenericTypeArguments[0].Name;

					//네임스페이스 전체 이름 재정의
					if(null != item.PropertyType.GenericTypeArguments[0].FullName)
					{
                        sNameFull = item.PropertyType.GenericTypeArguments[0].FullName!;
                    }
                }
                else if (item.PropertyType.Name == "Nullable`1")
				{
					//널 허용
					bNullable = true;

                    //원본이 가지고 있는 타입을 저장한다.
                    sType = item.PropertyType.GenericTypeArguments[0].Name;

                    //네임스페이스 전체 이름 재정의
                    if (null != item.PropertyType.GenericTypeArguments[0].FullName)
                    {
                        sNameFull = item.PropertyType.GenericTypeArguments[0].FullName!;
                    }
                }
				else if("[]" == sType.Substring(sType.Length - 2))
                {//개체 배열이다.

					//배열 개체를 지정하고
					sArrayType = sType.Substring(0, sType.Length - 2);
                    //리스트 타입인걸 알리고
                    sType = "List";

                    //네임스페이스 전체 이름 재정의
                    sNameFull = sNameFull.Substring(0, sNameFull.Length - 2);
                }


                //커스텀 속성 체크
                this.CustomAttributesFind(
                    item
                    , ref bNullable
                    , ref bModelOutputNo
					, ref sVarTypeEnforce);

                this.ModelMember.Add(new TypeScriptModelMember()
				{
					Name = item.Name
					, NameFull = sNameFull
					//강제 변수형이 지정되었다면 사용
                    , Type = sVarTypeEnforce == string.Empty ? sType : sVarTypeEnforce
                    , ArrayType = sArrayType
					, NullableIs = bNullable
					, ModelOutputNoIs = bModelOutputNo
				});
			}
		}

		//임포트 내용 초기화
		this.ImportClear();


	}//end Reset



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
	/// 임포트 관련 정보를 초기화 한다.
	/// </summary>
	public void ImportClear()
	{
		//this.ImportRootDir = string.Empty;
		this.ImportItem.Clear();
	}

	/// <summary>
	/// 가지고 있는 임포트 아이템 정보를 가지고 임포트 문자열을 만든다.
	/// </summary>
	/// <returns></returns>
	public string ToTypeScriptImportString()
	{
		StringBuilder sb = new StringBuilder();

		foreach (KeyValuePair<string, string> item in this.ImportItem.ToArray())
		{
            //임포트 라인 1개 생성
            sb.AppendLine(this.ToTypeScriptImportString(item.Key, item.Value));
		}//end foreach item

		if (0 < sb.Length)
		{//생성된 임포트 값이 있으면

			//한줄 더 추가
			sb.AppendLine();
		}

		return sb.ToString();
	}

    /// <summary>
    /// 전달받은 sKey와 sValue로 임포트 라인을 완성한다.
    /// </summary>
    /// <param name="sKey"></param>
    /// <param name="sValue"></param>
    /// <returns></returns>
    public string ToTypeScriptImportString(
		string sKey
		, string sValue)
	{
        StringBuilder sb = new StringBuilder();
        //임포트 라인 1개 생성
        sb.Append($"import {{ {sKey} }} from '{this.ImportRootDir}{sValue}';");

        return sb.ToString();
    }

    /// <summary>
    /// 가지고있는 임포트 정보로 타입스크립트를 생성한다.
    /// </summary>
    /// <remarks>
    /// 임포트 라인을 생성하지 않으려면 ImportClear();를 미리 호출하여 임포트 정보를 지우거나
    /// ToTypeScriptInterfaceString("") 로 호출하면된다.
    /// </remarks>
    /// <returns></returns>
    public string ToTypeScriptInterfaceString()
	{
		return this.ToTypeScriptInterfaceString(this.ToTypeScriptImportString());
	}

	/// <summary>
	/// 타입 스크립트에서 사용하는 인터페이스 타입으로 선언하는 코드를 생성한다.
	/// </summary>
	/// <param name="sImport">
	///	임포트 영역에 출력할 내용.<br />
	///	입력한 그대로 출력되므로 필요한 전체 내용을 넣는다.<br />
	///	이 부분은 자동화할 방법이 없으므로 직접 입력해야 한다.
	/// </param>
	/// <returns></returns>
	public string ToTypeScriptInterfaceString(string sImport)
	{

		return this.ToTypeScriptString(
				sImport + Environment.NewLine + Environment.NewLine
				, "export interface {0} " + Environment.NewLine
						+ "{{" + Environment.NewLine
				, @"    {0}: {1}," + Environment.NewLine
				, "}}"
			);
	}

	/// <summary>
	/// 스크립트 형태의 문자열을 생성한다.
	/// </summary>
	/// <param name="sImport">
	///	임포트 영역에 출력할 내용.<br />
	///	입력한 그대로 출력되므로 필요한 전체 내용을 넣는다.<br />
	///	이 부분은 자동화할 방법이 없으므로 직접 입력해야 한다.
	/// </param>
	/// <param name="sHead">첫 줄 열기로 사용할 문자열 포맷</param>
	/// <param name="sItemBody">아이템 바디로 사용할 문자열 포맷</param>
	/// <param name="sFooter">마지막 줄 닫기로 사용할 문자열 포맷</param>
	/// <returns></returns>
	public string ToTypeScriptString(
		string sImport
		, string sHead
		, string sItemBody
		, string sFooter)
	{

        ModelToTextModel mtmReturn = new ModelToTextModel();

        //임포트 영역
        mtmReturn.Import.Append(sImport);


		//머리 만들기*********
		//주석 검색어 만들기 - 타입 명
		string sT = string.Format("T:{0}.{1}"
										, this.ModelNamespace
										, this.ModelName);
		//주석 검색어 만들기 - 요소 명
		string sP = string.Format("P:{0}.{1}"
										, this.ModelNamespace
										, this.ModelName);

		//베이스 이름
		List<string> listP = new List<string>();
		listP.Add(string.Format(
					"P:{0}.{1}"
					, this.BaseNamespace
					, this.BaseName));



		//머리 주석
		if (null != this.ProjectXml)
		{
			string sHeadSummary
				= this.ProjectXml_SummaryGet(sT);

			if (string.Empty != sHeadSummary)
			{//주석 내용이 있다.
                mtmReturn.HeadSummary.Append(string.Format("/** {0} */" + Environment.NewLine
												, sHeadSummary));
			}
		}

        //머리 이름
        mtmReturn.HeadName.Append(string.Format(sHead, this.ModelName));


		//요소
		for (int i = 0; i < this.ModelMember.Count; ++i)
		{
			TypeScriptModelMember itemMM = this.ModelMember[i];

			if(true == itemMM.ModelOutputNoIs)
			{//출력 안함이다.
				continue;
			}

            ModelToTextItemModel newMTTI = new ModelToTextItemModel();

            //검색어 완성 시키기
            string sF_Name = sP + "." + itemMM.Name;

			//주석
			if (null != this.ProjectXml)
			{
				string sSummary
					= this.ProjectXml_SummaryGet(sF_Name);

				if (string.Empty == sSummary)
				{//못찾았다.

					//상속받은 어딘가에 있을 확률이 높다.
					foreach (string sF_Item in listP)
					{
						sSummary = this.ProjectXml_SummaryGet(sF_Item + "." + itemMM.Name);

						if (string.Empty != sSummary)
						{//찾았다.
							break;
						}
					}
				}

				if (string.Empty != sSummary)
				{//주석 내용이 있다.
                    newMTTI.Summary.Append(string.Format(@"    /** {0} */" + Environment.NewLine
													, sSummary));
				}
			}

			//요소 추가
			//타입을 타입스크립트에 맞게 변환한다.
			//변환이 안되면 그냥 그대로 넣는다.
			string sType = string.Empty;
			bool bResult = false;
			switch (itemMM.Type)
			{
				case "List"://리스트다
					{
						string sArrayType = string.Empty;
						bResult = this.TypeToTs(itemMM.ArrayType, out sArrayType);
						//배열로 선언
						sType = string.Format("{0}[]", sArrayType);
					}
                    break;
				case "Byte[]":
                    sType = "ArrayBuffer";
                    break;
				default:
                    bResult = this.TypeToTs(itemMM.Type, out sType);
                    break;
			}

            if (false == bResult)
            {//일치하는 타입이 없다.

				//외부에 참조 위치를 요청한다.
				List<ModelToTextImportModel> listImport
					= this.ImportSearchCallback(itemMM.NameFull);

                //일치하는 타입이 없다는건 별도 참조가 있다는 뜻이다.
                foreach (ModelToTextImportModel item in listImport)
				{
                    //타입스크립트 임포트 만들기
                    string sImportAddOne
                        = this.ToTypeScriptImportString(
                            item.Name
                            , item.OutPhysicalFullPath);

					if (null == mtmReturn.ImportAdd
								.Where(w => w == sImportAddOne)
								.FirstOrDefault())
					{//같은 이름의 참조가 없다.

						//리스트에 추가
						mtmReturn.ImportAdd.Add(sImportAddOne);
					}
                    
                }
            }

            //이름 백업
            string sItemName = itemMM.Name;


            if (true == itemMM.NullableIs)
			{//널 허용
                sItemName += "?";
            }

            newMTTI.Item.Append(
				string.Format(sItemBody, sItemName, sType));

			//아이템 추가
			mtmReturn.ItemList.Add(newMTTI);

        }//end for i


        //꼬리 만들기
        mtmReturn.Footer.Append(string.Format(sFooter, this.ModelName));


        return mtmReturn.ToString();
	}


    /// <summary>
    /// .NET 타입을 타입스크립트 타입으로 변환한다.
    /// </summary>
    /// <remarks>
    /// 변환되지 않으면 그대로 출력된다.
    /// </remarks>
    /// <param name="sType"></param>
    /// <param name="sReturn"></param>
    /// <returns>타입스크립트로 변환 성공여부. 성공하지 못했으면 개체타입일 확률이 높다.</returns>
    private bool TypeToTs(
		string sType
		, out string sReturn)
	{
		bool bReturn = true;
		sReturn = sType;

		switch (sType)
		{
			case "String":
				sReturn = "string";
				break;

			case "Int32":
			case "Int16":
			case "Int64":
			case "Double":
			case "Single":
            case "Decimal":
                sReturn = "number";
				break;

			case "DateTime":
				sReturn = "Date";
				break;

			case "Bool":
			case "Boolean":
                sReturn = "boolean";
                break;

			default:
				bReturn = false;
                break;
		}

		return bReturn;
	}

    /// <summary>
    /// 지정된 아이템에서 커스텀 어트리 뷰트를 검색하고 
    /// 지정된 어트리뷰트가 있으면 알려준다.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="bNullableAttribute"></param>
    /// <param name="bModelOutputNoAttribute"></param>
	/// <param name="sVarTypeEnforce"></param>
    private void CustomAttributesFind(
		PropertyInfo item
		, ref bool bNullableAttribute
        , ref bool bModelOutputNoAttribute
		, ref string sVarTypeEnforce)
	{

        //원래를 커스텀 속성을 읽어서 NullableAttribute를 찾으면 되는데
        //이유를 모르겠지만 특정조건에서 계속 NullableAttribute를 못찾는 현상이 있다.
        //널을 허용하지 않는 다른 string가 있으면 이 현상이 없는것으로 보아 버그가 아닌가 싶다.
        //그래서 아래 코드를 변경했다.

        //      if (null != item.CustomAttributes
        //				.Where(w => w.AttributeType.Name == "NullableAttribute")
        //				.FirstOrDefault())
        //{//널 속성이 있다.
        //	bNullableAttribute = true;
        //      }

        //.net .NET 6 Preview 7 이후 부터 사용할 수 있는 기능
        //https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-7/#libraries-reflection-apis-for-nullability-information
        NullabilityInfoContext _nullabilityContext = new NullabilityInfoContext();
        var nullabilityInfo = _nullabilityContext.Create(item);
        if (nullabilityInfo.WriteState == NullabilityState.Nullable)
        {
            bNullableAttribute = true;
        }




        if (null != item.CustomAttributes
                        .Where(w => w.AttributeType.Name == "ModelOutputNoAttribute")
                        .FirstOrDefault())
        {//프로퍼티 출력 안함 속성이 있다.
            bModelOutputNoAttribute = true;
        }


        CustomAttributeData? cadVarTypeEnforce
                = item.CustomAttributes
                        .Where(w => w.AttributeType.Name == "VarTypeEnforceAttribute")
                        .FirstOrDefault();
        if (null != cadVarTypeEnforce)
        {//변수형 강제 지정

			object? temp = cadVarTypeEnforce.ConstructorArguments[0].Value;
			if (null != temp)
			{
                sVarTypeEnforce = temp.ToString()!;
            }
        }
    }
}
