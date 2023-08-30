using System.Reflection;

using DGU_ModelToOutFiles.Global.Attributes;

namespace DGU_ModelToOutFiles.App.Faculty;

/// <summary>
/// 네임스페이스에 소속된 개체 리스트
/// </summary>
internal class NamespaceToClassList
{
    public List<ObjectOutModel> ClassList { get; set; }
        = new List<ObjectOutModel>();

    public NamespaceToClassList()
    {
    }



    /// <summary>
    /// 네임스페이스 기준으로 클래스를 생성하고 기존리스트에 추가한다,.
    /// </summary>
    /// <remarks>
    /// 생성된 오브젝트의 물리경로를 찾는 방법을 사용하고 싶었으나....
    /// 이렇게하면 외부 참조된 개체는 dll 위치로 생성되는 문제가 있다.
    /// </remarks>
    /// <param name="sAssemblyName">로드할 어셈블리 이름</param>
    /// <param name="arrNamespace">허용할 네임스페이스 리스트</param>
    /// <exception cref="Exception"></exception>
    public void ClassListAdd(
        string sAssemblyName
        , string[] arrNamespace)
    {
        //출력 안함 설정이 되었는지 체크하는 개체
        OutputNoAttributeCheck attrchkON = new OutputNoAttributeCheck();

        Assembly asm = Assembly.Load(sAssemblyName);


        var groups
            = asm.GetTypes()
                .Where(w1 =>
                {
                    bool bReturn = false;
                    if ((true == w1.IsClass || true == w1.IsEnum)  
                        && null != w1.Namespace)
                    {//클래스거나 열거형이면
                        //네임스페이스가 있으면


                        
                        //허용 리스트와 비교
                        for(int i = 0; i < arrNamespace.Length; ++i)
                        {
                            string sItem = arrNamespace[i];
                            if (w1.Namespace.Length >= sItem.Length
                                && w1.Namespace.Substring(0, sItem.Length) == sItem)
                            {//허용 리스트에 있다.
                                bReturn = true;
                                break;
                            }
                        }
                             
                    }

                    return bReturn;
                })
                .GroupBy(gb => gb.Namespace);
                     
                     
        foreach (var group in groups)
        {
            Console.WriteLine("Namespace: {0}", group.Key);
            foreach (var type in group)
            {
                Console.WriteLine("  {0}", type.Name);
            }

            //네임스페이스 추출
            string sNamespace = null == group.Key ? string.Empty : group.Key;
            //네임스페이스에서 어셈블리 네임스페이스 제외
            string sNamespace_Cut = sNamespace.Replace(sAssemblyName, "");
            //네임스페이스를 자르고
            string[] arrNs = sNamespace_Cut.Split('.');
            //자른 네임스페이스로 물리경로를 만들어 준다.
            List<string> listOutPhysicalPath = new List<string>();
            string sOutPhysicalPath = string.Empty;
            foreach (string itemNS in arrNs)
            {
                if(string.Empty != itemNS)
                {
                    listOutPhysicalPath.Add(itemNS);
                    sOutPhysicalPath += Path.Combine(sOutPhysicalPath, itemNS);
                }
            }

            ClassList.AddRange(
                group
                    //출력 안함 설정이 안되는 항목만 추출
                    .Where(w=> false == OutputNoCheck(attrchkON, w))
                    .Select(s =>
                        new ObjectOutModel()
                        {
                            Assembly = asm
                            , Namespace = sNamespace
                            , Namespace_Cut = sNamespace_Cut
                            , ClassName = s.Name
                            , ObjectOutType = ObjectOutTypeGet(s)
                            , OutPhysicalPathList = listOutPhysicalPath
                            , OutPhysicalPath = sOutPhysicalPath
                        }
                    ));
        }

        //https://stackoverflow.com/questions/223952/create-an-instance-of-a-class-from-a-string
        foreach (ObjectOutModel itemClass in ClassList)
        {
            //Type? t = asm.GetType(sClassName);
            Type? t = itemClass.Assembly
                        .GetType(itemClass.ClassNameFull);
            if (null != t)
            {
                itemClass.Instance = Activator.CreateInstance(t);
            }
            else
            {//소속된 어셈블리를 모를때

                //위에서 클래스를 찾지 못했다는 소리는 소속된 어셈블리에 대상이 없다는 의미다.
                //이 프로젝트에서는 명시적으로 소속된 어셈블리를 지정해야 한다.
                //그러니 아래 코드는 필요 없을 것으로 판단된다.
                //foreach (var asm2 in AppDomain.CurrentDomain.GetAssemblies())
                //{
                //    t = asm2.GetType(sClassName);

                //    if (t != null)
                //        listObj.Add(Activator.CreateInstance(t)!);
                //}

                throw new Exception("소속된 어셈블리를 찾지 못함");
            }
        }
    }

    /// <summary>
    /// 출력 안함 설정이 되어있는지 확인
    /// </summary>
    /// <param name="attrchkON"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool OutputNoCheck(
        OutputNoAttributeCheck attrchkON
        , Type type)
    {
        bool bReturn = false;

        OutputNoAttribute? temp = attrchkON.Check(type);
        if (null != temp)
        {
            bReturn = temp.OutputNoIs;
        }

        return bReturn;
    }

    /// <summary>
    /// 타입으로 오브젝트 출력 타입으로 바꾼다.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private ObjectOutType ObjectOutTypeGet(Type type)
    {

        ObjectOutType ooReturn = ObjectOutType.None;

        if (true == type.IsClass)
        {
            ooReturn = ObjectOutType.Class;
        }
        else if (true == type.IsEnum)
        {
            EnumTypeAttributeCheck newETA = new EnumTypeAttributeCheck();
            EnumTypeAttribute? temp = newETA.Check(type);

            if (null == temp
                || false == temp.TypeScript_EnumNoConstIs)
            {
                //const를 붙인다.
                ooReturn = ObjectOutType.Enum;
            }
            else
            {
                //const를 붙이지 않는다.
                ooReturn = ObjectOutType.Enum_ConstNo;
            }
            
        }

        return ooReturn;
    }

}
