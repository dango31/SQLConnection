using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlazorGroupB.Models;

public class Messages
{
    //  メッセージID
    [Key]
    public int MessageID { get; set; }

    //  スレッドID
    public int ThreadID { get; set; }

    //  ユーザーID
    public string? UserID { get; set; }

    //  書き込み主（手打ちの名前）
    public string? WriteName { get; set; }

    //  メッセージ本文
    public string? DetailMessage { get; set; }

    ////  投稿日時
    //public DateTime PostTime { get; set; }

    //  投稿日時
    public string? PostTime { get; set; }

}
