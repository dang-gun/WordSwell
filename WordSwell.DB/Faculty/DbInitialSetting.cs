using Microsoft.EntityFrameworkCore;
using ModelsContext;
using ModelsDB;
using WordSwell.DB;

namespace DbAssist.Faculty;

/// <summary>
/// 초기 세팅 지원
/// </summary>
public class DbInitialSetting
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sDbType">사용할 DB의 종류. 내부적으로는 소문자로 처리됨</param>
    /// <param name="ConnectString">DB 커낵션 문자열</param>
    public DbInitialSetting(
        string sDbType
        , string ConnectString)
    {
        //사용하려는 DB타입
        switch (sDbType.ToLower())
        {
            case "mssql":
                GlobalDb.DBType = UseDbType.Mssql;
                break;
            case "postgresql":
                GlobalDb.DBType = UseDbType.Postgresql;
                break;

            case "mariadb":
            case "mysql":
                GlobalDb.DBType = UseDbType.Mariadb;
                break;

            default://기본
                GlobalDb.DBType = UseDbType.Memory;
                break;
        }


        //DB 커낵션
        GlobalDb.DBString = ConnectString;

        //db 마이그레이션 적용
        switch (GlobalDb.DBType)
        {
            case UseDbType.Mssql:
                using (ModelsDbContext_Mssql db1 = new ModelsDbContext_Mssql())
                {
                    //db1.Database.EnsureCreated();
                    db1.Database.Migrate();
                }
                break;

            case UseDbType.Postgresql:
                using (ModelsDbContext_Postgresql db1 = new ModelsDbContext_Postgresql())
                {
                    //db1.Database.EnsureCreated();
                    db1.Database.Migrate();
                }
                break;

            case UseDbType.Mariadb:
                using (ModelsDbContext_Mariadb db1 = new ModelsDbContext_Mariadb())
                {
                    //db1.Database.EnsureCreated();
                    db1.Database.Migrate();
                }
                break;

            default://기본
                using (ModelsDbContext db1 = new ModelsDbContext())
                {
                    //db1.Database.EnsureCreated();
                    db1.Database.Migrate();
                }
                break;
        }
    }
}
