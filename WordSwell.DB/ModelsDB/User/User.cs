using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsDB.User;

/// <summary>
/// 유저
/// </summary>
public class User
{
    /// <summary>
    /// 게시판 고유번호
    /// </summary>
    [Key]
    public long idUser { get; set; }
}
