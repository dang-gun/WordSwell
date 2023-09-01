using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ModelsContext;

/// <summary>
/// mssql전용 컨텍스트
/// </summary>
///<remarks>
/// Add-Migration InitialCreate -Context ModelsDbContext_Mariadb -OutputDir Migrations/Mariadb 
/// Remove-Migration -Context ModelsDB.ModelsDbContext_Mariadb
/// Update-Database -Context ModelsDbContext_Mariadb
/// Update-Database -Context ModelsDbContext_Mariadb -Migration 0
///</remarks>
public class ModelsDbContext_Mariadb : ModelsDbContext
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="options"></param>
	public ModelsDbContext_Mariadb(DbContextOptions<ModelsDbContext> options)
		: base(options)
	{
	}

	/// <summary>
	/// 
	/// </summary>
	public ModelsDbContext_Mariadb()
	{
	}
}

/// <summary>
///  mssql전용 컨텍스트 팩토리
/// </summary>
public class ModelsDbContext_MariadbFactory
	: IDesignTimeDbContextFactory<ModelsDbContext_Mariadb>
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="args"></param>
	/// <returns></returns>
	public ModelsDbContext_Mariadb CreateDbContext(string[] args)
	{
		DbContextOptionsBuilder<ModelsDbContext> optionsBuilder
			= new DbContextOptionsBuilder<ModelsDbContext>();

		return new ModelsDbContext_Mariadb(optionsBuilder.Options);
	}
}