using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utility.ProjectXml;


using WordSwell.ApiModels.TestCont;
using ModelsDB.Board;
using ModelsDB.User;

namespace WordSwell.Tool.ApiModels.Faculty.ObjectToOut;

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

        Assembly asm = Assembly.Load(sAssemblyName);


        var groups
            = asm.GetTypes()
                .Where(w1 =>
                {
                    bool bReturn = false;
                    if (true == w1.IsClass && null != w1.Namespace)
                    {//클래스이면서
                        //네임스페이스가 있으면

                        //허용 리스트와 비교
                        for(int i = 0; i < arrNamespace.Length; ++i)
                        {
                            if(-1 < w1.Namespace.IndexOf(arrNamespace[i]))
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
                group.Select(s =>
                    new ObjectOutModel()
                    {
                        Assembly = asm
                        , Namespace = sNamespace
                        , Namespace_Cut = sNamespace_Cut
                        , ClassName = s.Name
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


}
