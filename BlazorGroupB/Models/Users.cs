using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlazorGroupB.Models;

public class Users
{

    //  ユーザーID
    [Key]
    public string UserID { get; set; }

    //  ユーザーエージェント
    public string UserAgent { get; set; }

    //  作成日
    public DateTime UserCreateDate { get; set; }


}