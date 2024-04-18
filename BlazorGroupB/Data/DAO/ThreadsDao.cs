using BlazorGroupB.Models;
using BlazorGroupB.Pages.bbs;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace BlazorGroupB.Data.DAO;

public class ThreadsDao
{
    private NpgsqlConnection conn;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ThreadsDao(NpgsqlConnection connection)
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

    public int Insert(Threads ths)
    {
        NpgsqlTransaction transaction = null;
        int result = 0;

        //  接続確認
        if (!Connection())
            return result;


        //  DTOの確認
        if (ths == null)
            return result;

        try
        {
            //  INSERT文
            //  アカウントの部分だけ編集を行う

            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO \"Threads\"(")
                .Append(" \"ThreadName\", \"UserID\", \"ThreadCreateDate\" )")
                .Append("VALUES( ")
                .Append(" @thread_name, @user_id, @thread_create_date")
                .Append(") RETURNING \"ThreadID\"");
            
            //  書き込みを行う
            NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), conn);

            //  パラメーターのセット
            cmd.Parameters.AddWithValue("@thread_name", ths.ThreadName);
            cmd.Parameters.AddWithValue("@user_id", ths.UserID);
            cmd.Parameters.AddWithValue("@thread_create_date", ths.ThreadCreateDate);

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

}