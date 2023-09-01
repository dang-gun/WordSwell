using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ModelsContext;

/// <summary>
/// Postgresql전용 컨텍스트
/// </summary>
///<remarks>
/// Add-Migration InitialCreate -Context ModelsDbContext_Postgresql -OutputDir Migrations/Postgresql
/// Remove-Migration -Context ModelsDB.ModelsDbContext_Postgresql
/// Update-Database -Context ModelsDbContext_Postgresql
/// Update-Database -Context ModelsDbContext_Postgresql -Migration 0
///</remarks>
public class ModelsDbContext_Postgresql : ModelsDbContext
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="options"></param>
	public ModelsDbContext_Postgresql(DbContextOptions<ModelsDbContext> options)
		: base(options)
	{
        //https://duongnt.com/datetime-net6-postgresql/
        //https://stackoverflow.com/questions/69961449/net6-and-datetime-problem-cannot-write-datetime-with-kind-utc-to-postgresql-ty
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    /// <summary>
    /// 
    /// </summary>
    public ModelsDbContext_Postgresql()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}

/// <summary>
///  Postgresql전용 컨텍스트 팩토리
/// </summary>
public class ModelsDbContext_PostgresqlFactory
    : IDesignTimeDbContextFactory<ModelsDbContext_Postgresql>
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="args"></param>
	/// <returns></returns>
	public ModelsDbContext_Postgresql CreateDbContext(string[] args)
	{
		DbContextOptionsBuilder<ModelsDbContext> optionsBuilder
			= new DbContextOptionsBuilder<ModelsDbContext>();

        //optionsBuilder.tr

        return new ModelsDbContext_Postgresql(optionsBuilder.Options);
	}
}