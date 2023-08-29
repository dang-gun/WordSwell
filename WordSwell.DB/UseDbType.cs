
namespace DbAssist;

/// <summary>
/// 사용하는 DB 타입
/// </summary>
public enum UseDbType
{
	/// <summary>
	/// 없음
	/// </summary>
	None = 0,

	/// <summary>
	/// In Memory
	/// </summary>
	Memory,

    /// <summary>
    /// Sqlite - 이 프로젝트는 Sqlite를 사용하지 않음
    /// </summary>
    Sqlite,

	/// <summary>
	/// MS Sql
	/// </summary>
	Mssql,

	/// <summary>
	/// 포스트그래스 sql
	/// </summary>
	Postgresql,

	/// <summary>
	/// 마리아 db, 마이sql
	/// </summary>
	Mariadb,
}
