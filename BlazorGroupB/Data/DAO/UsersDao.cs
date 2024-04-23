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

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public UsersDao(NpgsqlConnection connection)
    {
        conn = connection;
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

    public string Insert(Users userdata)
    {
        NpgsqlTransaction transaction = null;
        string result = "";
        try
        {

            //  DTOの確認
            if (userdata == null)
                throw new Exception("ユーザー情報の取得に失敗しました");

            //  重複の排除
            userdata.UserID = UserAgentContain(userdata);
            if (userdata.UserID != null)
                return userdata.UserID;

            string newUserId;
            while (true)
            {
                //  既存のものがなかったら新しく作成する
                newUserId = UserIdRandom();

                //  奇しくも重複していた時に再度UserIDを作成する。被っていないならbreak
                if (!UserIDContain(newUserId))
                    break;
            }

            //  値を入れる
            userdata.UserID = newUserId;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
        }


        try
        {

            //  接続確認
            if (!Connection())
                throw new Exception("NpgSqlの接続に失敗しました");

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
            cmd.ExecuteScalar();

            //  TRANSACTIONコミット
            transaction.Commit();

            return userdata.UserID;
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
    public string UserAgentContain(Users userdata)
    {

        //  接続確認
        if (!Connection())
            throw new Exception("NpgSqlの接続に失敗しました");

        StringBuilder sql = new StringBuilder();
        sql.Append("SELECT * FROM \"Users\" ")
            .Append("WHERE \"UserAgent\" = @user_agent");
        try
        {
            NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), conn);

            //  パラメーターのセット
            cmd.Parameters.AddWithValue("@user_agent", userdata.UserAgent);

            //  Readerを開く
            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)//  データを持っているかbool型)
                {
                    while (reader.Read())   //  読込む
                    {

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            //  列名と値を取り出す
                            string columnName = reader.GetName(i);
                            object columnValue = reader[i];

                            Debug.Write($"{columnName} : {columnValue.ToString()}  ");

                        }

                        Users dto1 = new Users()
                        {
                            UserID = (string)reader["UserID"],
                            UserAgent = (string)reader["UserAgent"],
                            UserCreateDate = (DateTime)reader["UserCreateDate"]
                        };
                        return dto1.UserID;
                    }

                }
            }
        }
        catch (NpgsqlException ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
            throw new Exception("セレクトエラー");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
            throw new Exception("セレクト処理エラー");
        }
        finally
        {
            conn?.Close();
        }


        return null;
    }

    public bool UserIDContain(string userid)
    {

        //  接続確認
        if (!Connection())
            throw new Exception("NpgSqlの接続に失敗しました");

        StringBuilder sql = new StringBuilder();
        sql.Append("SELECT * FROM \"Users\" ")
            .Append("WHERE \"UserID\" = @user_id");

        try
        {
            NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), conn);

            //  パラメーターのセット
            cmd.Parameters.AddWithValue("@user_id", userid);

            //  Readerを開く
            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)//  データを持っているかbool型)
                {
                    while (reader.Read())   //  読込む
                    {

                        return true;
                    }

                }
            }
        }
        catch (NpgsqlException ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
            throw new Exception("セレクトUserIDエラー");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
            throw new Exception("セレクトUserID処理エラー");
        }
        finally
        {
            conn?.Close();
        }


        return false;
    }
    public string UserIdRandom()
    {
        var random = new Random();
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string newUserID = new string(Enumerable.Repeat(chars, 10)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        return newUserID;
    }

}
