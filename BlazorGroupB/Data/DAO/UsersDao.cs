using BlazorGroupB.Data;
using BlazorGroupB.Models;
using BlazorGroupB.Pages.bbs;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlazorGroupB.Data.DAO;

public class UsersDao
{
    private NpgsqlConnection conn;
    BlazorGroupBDbContext _contextUsers;
    //List<Users> users = new List<Users>();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public UsersDao(NpgsqlConnection connection)
    {
        conn = connection;
        //users = _contextUsers.Users.ToList<Users>();

    }

    public bool Connection()
    {
        //  接続の確認
        try
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            Debug.WriteLine("接続成功");
            return true;

        }
        catch (NpgsqlException ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
            throw new Exception("コネクションオープンエラー");

        }
        catch (NullReferenceException ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
            throw new Exception("コネクション処理エラー");

        }

        return false;
    }

    public int Insert(Users userdata)
    {
        NpgsqlTransaction transaction = null;
        int result = 0;

        //  接続確認
        if (!Connection())
            return result;

        //  DTOの確認
        if (userdata == null)
            return result;

        //  重複の排除
        if (UsersContain(userdata))
            return result;

        try
        {
            //  INSERT文
            //  アカウントの部分だけ編集を行う

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO \"Users\"(")
                .Append(" \"UserID\", \"UserAgent\", \"UserCreateDate\")")
                .Append("VALUES( ")
                .Append(" @user_id, @user_agent, @user_create_date")
                .Append(") RETURNING \"UserID\"");

            //  書き込みを行う
            NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), conn);

            //  パラメーターのセット
            cmd.Parameters.AddWithValue("@user_id", userdata.UserID);
            cmd.Parameters.AddWithValue("@user_agent", userdata.UserAgent);
            cmd.Parameters.AddWithValue("@user_create_date", userdata.UserCreateDate);

            //  TRANSACTION
            transaction = conn.BeginTransaction();
            int lastid = (int)cmd.ExecuteScalar();

            //  TRANSACTIONコミット
            transaction.Commit();

            return lastid;
        }
        catch (NpgsqlException ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
            throw new Exception("インサートエラー");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
            throw new Exception("インサート処理エラー");
        }
        finally
        {
            conn?.Close();
        }

    }

    //  Usersテーブルの重複の排除　既にあればtrueなければfalse
    public bool UsersContain(Users userdata)
    {
        StringBuilder sql = new StringBuilder();
        sql.Append("SELECT * FROM \"Users\" ")
            .Append("WHERE \"UserAgent\" = @user_agent)");

        NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), conn);

        //  パラメーターのセット
        cmd.Parameters.AddWithValue("@user_agent", userdata.UserAgent);

        //  セレクト文の発行
        var command = conn.CreateCommand();
        command.CommandText = sql.ToString();

        //  Readerを開く
        using (NpgsqlDataReader reader = command.ExecuteReader())
        {
            if (reader.HasRows)//  データを持っているかbool型)
            {
                return true;
            }
        }
        return false;
    }
}
