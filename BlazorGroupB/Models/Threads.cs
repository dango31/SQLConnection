using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlazorGroupB.Models;

public class Threads
{
    [Key]
    public int ThreadID { get; set; }
    public string? ThreadName { get; set; }
    public string UserID { get; set; }
    public DateTime ThreadCreateDate { get; set; }
    public DateTime? LastPostTime { get; set; }

}
