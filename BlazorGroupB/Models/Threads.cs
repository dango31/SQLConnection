using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlazorGroupB.Models;

public class Threads
{
    //  スレッドID
    [Key]
    public int ThreadID { get; set; }

    //  スレッドタイトル
    public string? ThreadName { get; set; }

    //  ユーザーID
    public string UserID { get; set; }

    //スレッドの作成日時
    //public DateTime ThreadCreateDate { get; set; }

    //スレッドの更新日時
    //public DateTime? LastPostTime { get; set; }

    //  スレッドの作成日時
    public string ThreadCreateDate { get; set; }

    //  スレッドの更新日時
    public string? LastPostTime { get; set; }

}
