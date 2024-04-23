using System.ComponentModel.DataAnnotations;

namespace BlazorGroupB.Models;

public class Uranais
{
    //  うらないID
    [Key]
    public int UranaiID { get; set; }

    //  うらないの運勢
    public string? Fortune { get; set; }
}