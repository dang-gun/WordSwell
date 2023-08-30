using System.Text;
using System.Threading.Tasks;

using Utility.FileAssist;

using DGUtility.EnumToClass;
using DGUtility.ModelToFrontend;
using DGUtility.ProjectXml;

namespace DGU_ModelToOutFiles.App.Faculty;

/// <summary>
/// 타입 스크립트로 내보낸다
/// </summary>
internal class ObjectToOut_Typescript : ObjectToOutBase, ObjectToOutInterface
{
    /// <summary>
    /// 개체 리스트를 타입스크립트 파일로 출력하기위한 클래스
    /// </summary>
    /// <param name="sOutputPath">출력할 물리 경로(폴더)</param>
    /// <param name="ProjectXml">주석이 들어 있는 XML 개체</param>
    public ObjectToOut_Typescript(
        string sOutputPath
        , ProjectXmlAssist ProjectXml)
        : base(sOutputPath, ProjectXml)
    {

    }

    /// <summary>
    /// 타입스크립트로 저장한다.
    /// </summary>
    public override void ToTargetSave()
    {
        FileSaveAssist fileSave = new FileSaveAssist();

        //처리할 클래스 리스트
        List<ObjectOutModel> listObject
            = base.NsToClass.ClassList;

        string sTemp = string.Empty;
        //열거형을 모델로 바꾸기위한 개체
        EnumToModel etmBP_Temp = new EnumToModel(base.ProjectXml);

        //모델을 타입스크립트로 출력하기 위한 개체
        ModelToTs tsModel_Temp = new ModelToTs(base.ProjectXml);
        //임포트시 앞에 붙을 루트 지정
        tsModel_Temp.ImportRootDir = "";
        //임포트시 다른 참조가 필요하면 호출되는 콜백
        tsModel_Temp.ImportSearchCallback
            = (string sNamespace)
            => 
            {
                return listObject
                        .Where(w => w.ClassNameFull == sNamespace)
                        .Select(s =>
                            new ModelToTextImportModel()
                            {
                                Name = s.ClassName
                                , OutPhysicalFullPath 
                                    = this.DirToImportPath(
                                        s.OutPhysicalPathList
                                        , s.ClassName)
                            })
                        .ToList();
                
            };


        

        for (int i = 0; i < listObject.Count; ++i)
        {
            ObjectOutModel itemOOM = listObject[i];

            if(null != itemOOM.Instance)
            {
                //경로 생성
                itemOOM.OutPhysicalPath_Create();

                if (itemOOM.ObjectOutType == ObjectOutType.Class)
                {
                    //타입스크립트(인터페이스)로 변환
                    tsModel_Temp.TypeData_Set(itemOOM.Instance);
                    sTemp = tsModel_Temp.ToTypeScriptInterfaceString(itemOOM.ImportAdd);
                }
                else if (itemOOM.ObjectOutType == ObjectOutType.Enum)
                {

                    etmBP_Temp.TypeData_Set((Enum)itemOOM.Instance);
                    sTemp = etmBP_Temp.ToTypeScriptEnumString(true);
                }
                else if (itemOOM.ObjectOutType == ObjectOutType.Enum_ConstNo)
                {

                    etmBP_Temp.TypeData_Set((Enum)itemOOM.Instance);
                    sTemp = etmBP_Temp.ToTypeScriptEnumString(false);
                }

                fileSave
                    .FileSave(Path.Combine(base.OutputPath, itemOOM.OutPhysicalFullPath) + ".ts"
                                , sTemp + itemOOM.LastText);
            }

        }//end for i


    }

    /// <summary>
    /// 파일 경로 리스트를 임포트 패스로 변경해준다.
    /// </summary>
    /// <param name="listOutPhysicalPath"></param>
    /// <param name="sClassName"></param>
    /// <returns></returns>
    private string DirToImportPath(
        List<string> listOutPhysicalPath
        , string sClassName)
    {
        StringBuilder sbReturn = new StringBuilder();

        for (int i = 0; i < listOutPhysicalPath.Count; ++i)
        {

            string sItem = listOutPhysicalPath[i];

            if (0 != i)
            {
                //맨앞에는 구분자를 추가하지 않는다.
                //구분자 추가
                sbReturn.Append("/");
            }

            sbReturn.Append(sItem);
        }

        sbReturn.Append($"/{sClassName}");

        return sbReturn.ToString();
    }
}
